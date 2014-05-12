using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Media.Imaging;
using ServiceStack;
using Microsoft.VisualBasic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace CloudStorage
{
    public partial class FunctionScreen : MetroFramework.Forms.MetroForm
    {
        //public static string CLOUD_SERVICE_ENDPOINT = "https://128.84.216.68";
        public static string CLOUD_SERVICE_ENDPOINT = File.ReadAllText("ConfigFile.txt");
        ClientInfo ConnectedUser;
        public TreeNode File_Node = new TreeNode ("_", "_", "_");
        public List<TreeNode> Disp_List;
        public List<TreeNode> Share_List = new List<TreeNode>();
        public List<TreeNode> SharedBy_List = new List<TreeNode>();
        public List<TreeNode> Recent_List = new List<TreeNode>();
        public UserFileSystem ClientFileSystem = new UserFileSystem();
        public UserFileSystemMetaData FileList = new UserFileSystemMetaData();
        public class ShareFileWithUser { }
        public class UnShareFileWithUser { }
        public class DeleteFile { }
        
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            //Do something to check the certificate is valid. 
            return true;
        }

        public FunctionScreen(string UserId, string Password)
        {
            ConnectedUser = new ClientInfo(UserId, Password);
            InitializeComponent();
            this.UserIdTextBox.Text = ConnectedUser.ClientId;
            this.UserNameDisp_textBox.Text = ConnectedUser.ClientId;
            this.GetFiles();
            this.UserIdTextBox.Text = UserId;                
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            var BrowseWin = new System.Windows.Forms.OpenFileDialog();
            BrowseWin.Filter = "All Files (*.*)|*.*";
            var ret = BrowseWin.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                long FileVer = -1;
                string FileName = DirectoryTextBox.Text + BrowseWin.SafeFileName;
                foreach (FileMetaData file in FileList.fileMDList)
                {
                    if (file.filepath == FileName)
                    {
                        FileVer = 1 + file.versionNumber;
                        break;
                    }
                }
                if(FileVer == -1)
                {// false
                    FileVer = 1;                    
                }


                UpdateFile arg = new UpdateFile();
                string LocalFilePath = BrowseWin.FileName;
                UserFile FileToUpload = new UserFile(FileName, ConnectedUser.ClientId);

                //byte[] fileStream = File.ReadAllBytes(LocalFilePath);`
                string fileStream = File.ReadAllText(LocalFilePath);
                //bool done = FileToUpload.SetFileContent(fileStream, (long)0);
                FileToUpload.SetFileContent(Encoding.UTF8.GetBytes(fileStream), FileVer);
                arg.file = FileToUpload;
                string requestUrl = string.Format("/updatefile/{0}/{1}", ConnectedUser.ClientId, ConnectedUser.Password);
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                JsonServiceClient Updateclient = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);
                //Updateclient.ContentType = "application/json";
                try
                {
                    Updateclient.Post<Object>(requestUrl, arg);
                    ClientFileSystem.addFileSynchronized(FileToUpload);             // update clientfilesystem on upload
                }
                catch(Exception ex)
                {                    
                        MessageBox.Show("This File has been changed by another client. Please download the latest copy and try to uplaod");                    
                }

                //send file for Upload
                
                this.UpdateFileList();
            }
        }
        private void UpdateFileList()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            JsonServiceClient client = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);
            List<TreeNode> newShared = new List<TreeNode>();
            List<TreeNode> newSharedBy = new List<TreeNode>();

            string GetUrl = string.Format("/getUserFileSystemInfo/{0}/{1}", ConnectedUser.ClientId, ConnectedUser.Password);
            try
            {
                FileList = client.Get<UserFileSystemMetaData>(GetUrl);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            List<StringValue> List_File = new List<StringValue>();
            foreach (FileMetaData file in FileList.fileMDList)
            {
                if (file.markedForDeletion == false)
                {
                    List_File.Add(new StringValue(file.filepath));
                    string[] F_name = file.filepath.Split('_');
                    string Val_Param = string.Format(F_name[F_name.Length - 1] + System.Environment.NewLine + "Owner: " + file.owner + " Size: " + file.filesize);
                    TreeNode.AddNodeToTree(File_Node, file.filepath, Val_Param);
                }
                else
                {
                    if (FileList.fileMDList.Contains(file))
                    {
                        ClientFileSystem.filemap.Remove(file.filepath);
                        string[] F_name = file.filepath.Split('_');
                        TreeNode.deleteNodeAndGetAllAffected(F_name[F_name.Length - 1], File_Node);
                    }
                }
            }            
            Disp_List = TreeNode.fetcAllChildren(this.DirectoryTextBox.Text, File_Node);
            this.Manage_RecentList(File_Node);
            
            foreach (SharedFile file in FileList.sharedFileList)
            {
                string[] F = file.filename.Split('_');
                string ShareVal = string.Format(F[F.Length -1] + System.Environment.NewLine + "Owner: "+file.owner);
                newShared.Add(new TreeNode(file.filename, ShareVal, file.filename));                                                    // shared with me     
            }
            Share_List = newShared;
                  
            foreach (FileMetaData file in FileList.fileMDList)
            {                
                if (file.sharedwithclients.Count() > 0)
                {
                    string[] F = file.filepath.Split('_');
                    string ShareVal = string.Format(F[F.Length - 1] + System.Environment.NewLine + "You shared with: ");
                    foreach (string id in file.sharedwithclients)
                    {
                        ShareVal = string.Format(ShareVal + id + " ");
                    }
                    newSharedBy.Add(new TreeNode(file.filepath, ShareVal, file.filepath));
                }
            }
            SharedBy_List = newSharedBy;
            SharedByDataList.DataSource = SharedBy_List;
            FileList_dataGridView.DataSource = Disp_List;
            SharedWithDataList.DataSource = Share_List;
            
            SharedByDataList.AutoResizeColumns();
            FileList_dataGridView.AutoResizeColumns();
            SharedWithDataList.AutoResizeColumns();
            SharedByDataList.AutoResizeRows();
            FileList_dataGridView.AutoResizeRows();
            SharedWithDataList.AutoResizeRows();

            this.UsageProgressBar.Value = (int)FileList.userMetaData.totalFileSystemSizeBytes;
            int per = this.UsageProgressBar.Value/100000000;
            this.UsageTextBox.Text = string.Format(per + " of 10 GB");
        }

        private void GetFiles()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
             JsonServiceClient client = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);
            
            string GetUrl = string.Format("/getUserFileSystemInfo/{0}/{1}", ConnectedUser.ClientId, ConnectedUser.Password);
            try 
            {
                FileList = client.Get<UserFileSystemMetaData>(GetUrl);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            List<StringValue> List_File = new List<StringValue>();
            foreach (FileMetaData file in FileList.fileMDList)
            {
                if (file.markedForDeletion == false)
                {
                    List_File.Add(new StringValue(file.filepath));
                    string[] F_name = file.filepath.Split('_');
                    string Val_Param = string.Format(F_name[F_name.Length - 1] + System.Environment.NewLine + "Owner: " + file.owner + " Size: " + file.filesize);
                    TreeNode.AddNodeToTree(File_Node, file.filepath, Val_Param);
                }
                else
                {
                    if (FileList.fileMDList.Contains(file))
                    {
                        ClientFileSystem.filemap.Remove(file.filepath);
                        string[] F_name = file.filepath.Split('_');
                        TreeNode.deleteNodeAndGetAllAffected(F_name[F_name.Length - 1], File_Node);
                    }
                }
            }            
            
            Disp_List = TreeNode.fetcAllChildren(this.DirectoryTextBox.Text, File_Node);
            this.Manage_RecentList(File_Node);
             FileList_dataGridView.DataSource = Disp_List;
             if (Disp_List != null)
             {
                 FileList_dataGridView.Font = new Font("Segoe UI", 10);
                 FileList_dataGridView.Columns["nodekey"].Visible = false;
                 FileList_dataGridView.Columns["nodePath"].Visible = false;
             }
                 DataGridViewColumn blank = new DataGridViewColumn();
                 blank.Width = 4000;

                 blank.CellTemplate = new DataGridViewTextBoxCell();
                 FileList_dataGridView.Columns.Add(blank);
                 DataGridViewImageColumn btnDownload = new DataGridViewImageColumn();
                 btnDownload.Width = 35;
                 Image downloadImg = Image.FromFile("C:\\Users\\Karthikeya\\Desktop\\file_download.png");
                 btnDownload.Image = downloadImg;
                 FileList_dataGridView.Columns.Add(btnDownload);

                 DataGridViewImageColumn btnShare = new DataGridViewImageColumn();
                 btnShare.Width = 35;
                 Image ShareImg = Image.FromFile("C:\\Users\\Karthikeya\\Desktop\\file_share.png");
                 btnShare.Image = ShareImg;
                 FileList_dataGridView.Columns.Add(btnShare);

                 DataGridViewImageColumn btnDelete = new DataGridViewImageColumn();
                 btnDelete.Width = 35;
                 Image DeleteImg = Image.FromFile("C:\\Users\\Karthikeya\\Desktop\\del.png");
                 btnDelete.Image = DeleteImg;
                 FileList_dataGridView.Columns.Add(btnDelete);
            // for share with you list
            foreach (SharedFile file in FileList.sharedFileList)
            {
                string[] F = file.filename.Split('_');
                string ShareVal = string.Format(F[F.Length - 1] + System.Environment.NewLine + "Owner: " + file.owner);
                Share_List.Add(new TreeNode(file.filename, ShareVal, file.filename));                                                    // shared with me     
            }
            SharedWithDataList.DataSource = Share_List;
            if (Share_List != null)
            {
                SharedWithDataList.Font = new Font("Segoe UI", 10);
                SharedWithDataList.Columns["nodekey"].Visible = false;
                SharedWithDataList.Columns["nodePath"].Visible = false;
            }            
 
            DataGridViewImageColumn btnSDownload = new DataGridViewImageColumn();
            btnSDownload.Width = 35;            
            btnSDownload.Image = downloadImg;
            SharedWithDataList.Columns.Add(btnSDownload);
            
            // for shared by you list
            foreach (FileMetaData file in FileList.fileMDList)
            {
                if (file.sharedwithclients.Count() > 0)
                {
                    string[] F = file.filepath.Split('_');
                    string ShareVal = string.Format(F[F.Length - 1] + System.Environment.NewLine + "You shared with: ");
                    foreach (string id in file.sharedwithclients)
                    {
                        ShareVal = string.Format(ShareVal + id + " ");
                    }
                    SharedBy_List.Add(new TreeNode(file.filepath, ShareVal, file.filepath));
                }
            }
            SharedByDataList.DataSource = SharedBy_List;
            if (SharedBy_List != null)
            {
                SharedByDataList.Font = new Font("Segoe UI", 10);
                SharedByDataList.Columns["nodekey"].Visible = false;
                SharedByDataList.Columns["nodePath"].Visible = false;
            }
            
            DataGridViewImageColumn btnBDownload = new DataGridViewImageColumn();
            btnBDownload.Width = 35;
            btnBDownload.Image = downloadImg;
            SharedByDataList.Columns.Add(btnBDownload);

            DataGridViewImageColumn btnUnBShare = new DataGridViewImageColumn();
            btnUnBShare.Width = 35;
            Image UnShareImg = Image.FromFile("C:\\Users\\Karthikeya\\Desktop\\file_Unshare.png");
            btnUnBShare.Image = UnShareImg;
            SharedByDataList.Columns.Add(btnUnBShare);

            SharedByDataList.AutoResizeColumns();
            FileList_dataGridView.AutoResizeColumns();
            SharedWithDataList.AutoResizeColumns();
            SharedByDataList.AutoResizeRows();
            FileList_dataGridView.AutoResizeRows();
            SharedWithDataList.AutoResizeRows();

            this.UsageProgressBar.Value = (int)FileList.userMetaData.totalFileSystemSizeBytes;
            int per = this.UsageProgressBar.Value / 100000000;
            this.UsageTextBox.Text = string.Format(per + " of 10 GB");
        }
       
        private void DownLoadFile(string FileName, string owner)
        {
            Stream FileStream;
            var SaveWin = new System.Windows.Forms.SaveFileDialog();
            string[] F_name = FileName.Split('_');
            SaveWin.FileName = F_name[F_name.Length - 1];               // to mane the file names same
            var ret = SaveWin.ShowDialog();
            if (ret == DialogResult.OK)
            {
                if ((FileStream = SaveWin.OpenFile()) != null)
                {                    
                    UserFile FileToSave = new UserFile("DownloadedFile", ConnectedUser.ClientId);
                    // Code to write the stream goes here.
                    if(ClientFileSystem.filemap.ContainsKey(FileName))
                    {
                        FileToSave = ClientFileSystem.filemap[FileName];
                    }
                    else
                    {
                        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                        JsonServiceClient client = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);

                        string GetFileUrl = string.Format("/file/{0}/{1}/{2}/{3}", ConnectedUser.ClientId, ConnectedUser.Password, FileName, owner);
                        try
                        {
                            FileToSave = client.Get<UserFile>(GetFileUrl);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        ClientFileSystem.addFileSynchronized(FileToSave);
                    }
                    FileStream.Write(Encoding.UTF8.GetString(FileToSave.filecontent));
                    FileStream.Close();
                }
            }
        }

        private void DownLoadFileShared(string FileName)
        {
            Stream FileStream;
            var SaveWin = new System.Windows.Forms.SaveFileDialog();
            string[] F_name = FileName.Split('_');
            SaveWin.FileName = F_name[F_name.Length - 1];               // to mane the file names same
            var ret = SaveWin.ShowDialog();
            if (ret == DialogResult.OK)
            {
                if ((FileStream = SaveWin.OpenFile()) != null)
                {
                    UserFile FileToSave = new UserFile("DownloadedFile", ConnectedUser.ClientId);
                    // Code to write the stream goes here.
                    if (ClientFileSystem.filemap.ContainsKey(FileName))
                    {
                        FileToSave = ClientFileSystem.filemap[FileName];
                    }
                    else
                    {
                        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                        JsonServiceClient client = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);
                        foreach (SharedFile file in FileList.sharedFileList)
                        {
                            if (file.filename == FileName)
                            {
                                string owner = file.owner;
                                string GetFileUrl = string.Format("/file/{0}/{1}/{2}/{3}", ConnectedUser.ClientId, ConnectedUser.Password, FileName, owner);
                                try
                                {
                                    FileToSave = client.Get<UserFile>(GetFileUrl);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                ClientFileSystem.addFileSynchronized(FileToSave);
                            }
                            FileStream.Write(Encoding.UTF8.GetString(FileToSave.filecontent));
                            FileStream.Close();
                        }
                    }
                }
            }
        }


        private void DeleteFileFromFS(string FileName)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            JsonServiceClient Delclient = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);
            string DeleteFileUrl = string.Format("/deletefile/{0}/{1}/{2}", ConnectedUser.ClientId, ConnectedUser.Password, FileName);
            try
            {
                Delclient.Post<object>(DeleteFileUrl, new DeleteFile());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (ClientFileSystem.filemap.ContainsKey(FileName))
            {
                ClientFileSystem.filemap[FileName].filecontent = new byte[0];
            }
            this.UpdateFileList();
        }

        private void ShareFileWith(string Id, string FileName)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            JsonServiceClient ShareClient = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);            
                        
            foreach (FileMetaData file in FileList.fileMDList)
            {
                if (file.filepath == FileName)
                {
                    if (file.sharedwithclients.Contains(Id))
                    {
                        MessageBox.Show(string.Format("File already shared with " + Id));
                    }
                    else
                    {
                        string ShareFileUrl = string.Format("/shareFile/{0}/{1}/{2}/{3}", ConnectedUser.ClientId, ConnectedUser.Password, FileName, Id);
                        try
                        {
                            ShareClient.Post<object>(ShareFileUrl, new ShareFileWithUser());                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }                                                             
            this.UpdateFileList();
        }
        private void FileList_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            switch (e.ColumnIndex)
            {
                case 1:
                    // code to download file 
                    DownLoadFile(this.Disp_List[e.RowIndex].nodePath, ConnectedUser.ClientId);
                    break;
                case 2:
                    // code to share file     
                    string ShareID =  Microsoft.VisualBasic.Interaction.InputBox("Enter Client ID to share file with", "Share File", "", 600, 400);
                    if (ShareID.Length > 0)
                    {                        
                        ShareFileWith(ShareID, this.Disp_List[e.RowIndex].nodePath);                        
                    }
                    break;
                case 3:
                    // code to delete file
                    if (MessageBox.Show("Are you sure you want to delete this File?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // Delete the file in the file server     
                        DeleteFileFromFS(this.Disp_List[e.RowIndex].nodePath); ;
                    }
                    break;
                default:
                    break;
            }
        }

        private void FileList_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //var senderGrid = (DataGridView)sender;
            string Text = this.Disp_List[e.RowIndex].nodePath;
            TreeNode temp = this.Disp_List[e.RowIndex];
            Disp_List = TreeNode.fetcAllChildren(Text, File_Node);
            if (Disp_List.Count == 0)
            {               
                //this.DirectoryTextBox.Text = string.Format(this.DirectoryTextBox.Text + this.Disp_List[e.RowIndex].nodeKey);
            }
            else
            {
                this.DirectoryTextBox.Text = Text;
                //Disp_List = TreeNode.fetcAllChildren(this.DirectoryTextBox.Text, File_Node);
                FileList_dataGridView.DataSource = Disp_List;
                Manage_RecentList(temp);
            }            
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            this.DirectoryTextBox.Text = "";
            Disp_List = TreeNode.fetcAllChildren(this.DirectoryTextBox.Text, File_Node);
            FileList_dataGridView.DataSource = Disp_List;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {            
            string[] directories = this.DirectoryTextBox.Text.Split('_');
            int i = 0;

            this.DirectoryTextBox.Text = "";
            foreach (var dir in directories )                    
            {
                if(i < directories.Length - 2)
                {
                    if (dir != "")
                    {
                        this.DirectoryTextBox.Text = string.Format(this.DirectoryTextBox.Text + dir + "_");                            
                    }
                    i++;
                }
            }
            Disp_List = TreeNode.fetcAllChildren(this.DirectoryTextBox.Text, File_Node);
            FileList_dataGridView.DataSource = Disp_List;     
       // how to traverse from recent
        }

        private void BtnRecent_Click(object sender, EventArgs e)
        {
            //change File listing to  recent_list  
            Disp_List = Recent_List;
            FileList_dataGridView.DataSource = Disp_List;   
        }
        // for recent file listing
        private void Manage_RecentList(TreeNode Dir)
        {                       
            if (Recent_List.Contains(Dir))
            {
                //BringToFront node To end
                Recent_List.Remove(Dir);
                Recent_List.Add(Dir);
            }
            else
            {
                // add node                
                Recent_List.Add(Dir);
            }

        }

        private void SignOutLink_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Clear_Data();           
        }
        private void Clear_Data()
        {
            this.UserNameDisp_textBox.Text = this.ConnectedUser.ClientId;
            this.CurrPasswordInp_textBox.Text = "";
            this.NewPassInp_textBox.Text = "";
            this.ConfPasswordInp_textBox.Text = "";
            this.EMailInp_textBox.Text = "";  
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (this.CurrPasswordInp_textBox.Text == "")
            {
                MessageBox.Show("Please enter details to be changed", "Enter details", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Clear_Data();
            }
            if (this.NewPassInp_textBox.Text != "")
            {
                if (this.CurrPasswordInp_textBox.Text == this.NewPassInp_textBox.Text)
                {
                    MessageBox.Show("New Password is same as the Current Password", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.CurrPasswordInp_textBox.Text = "";
                    this.NewPassInp_textBox.Text = "";
                    this.ConfPasswordInp_textBox.Text = "";
                }
                else if (this.NewPassInp_textBox.Text != this.ConfPasswordInp_textBox.Text)
                {
                    MessageBox.Show("Passwords do not match", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.CurrPasswordInp_textBox.Text = "";
                    this.NewPassInp_textBox.Text = "";
                    this.ConfPasswordInp_textBox.Text = "";
                }
            }            
                //Update user settings            
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.UpdateFileList();
        }

        private void BtnWith_Click(object sender, EventArgs e)
        {
            this.UpdateFileList();                        
        }

        private void BtnBy_Click(object sender, EventArgs e)
        {
            this.UpdateFileList();           
        }
        private void UnShareFile(string Id, string FileName)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            JsonServiceClient ShareUnClient = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);    
            foreach (FileMetaData file in FileList.fileMDList)
            {
                if (file.filepath == FileName)
                {
                    if (file.sharedwithclients.Contains(Id))
                    {
                        string ShareFileUrl = string.Format("/unShareFile/{0}/{1}/{2}/{3}", ConnectedUser.ClientId, ConnectedUser.Password, FileName, Id);
                        try
                        {
                            ShareUnClient.Post<object>(ShareFileUrl, new UnShareFileWithUser());                          
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {                        
                        //MessageBox.Show(string.Format("File already shared with " + Id));
                    }
                }
            }
            this.UpdateFileList();
        }
        private void SharedWithDataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            switch (e.ColumnIndex)
            {
                case 0:
                    // code to download file 
                    DownLoadFileShared(this.Share_List[e.RowIndex].nodePath);
                    break;                
                default:
                    break;
            }
        }

        private void SharedByDataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            switch (e.ColumnIndex)
            {
                case 0:
                    // code to download file 
                    DownLoadFile(this.SharedBy_List[e.RowIndex].nodePath, ConnectedUser.ClientId);
                    break;
                case 1:
                    // code to Unshare file   
                    string ShareID = Microsoft.VisualBasic.Interaction.InputBox("Enter Client ID to Unshare file with", "UnShare File", "", 600, 400);
                    UnShareFile(ShareID, this.SharedBy_List[e.RowIndex].nodePath);                    
                    break;                
                default:
                    break;
            }
        } 
    }
}
