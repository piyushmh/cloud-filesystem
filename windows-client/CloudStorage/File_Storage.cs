using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using ServiceStack;

namespace CloudStorage
{    
    public partial class File_Storage : Form
    {
        public FileItem fileSel = new FileItem();
        public static string CLOUD_SERVICE_ENDPOINT = "http://128.84.216.57:8080";
        public File_Storage()
        {
            InitializeComponent();            
            this.GetFiles();
        }

        private void BtnGetFile_Click(object sender, EventArgs e)
        {
            
            // Option to save selected file on local disk
            string FilePath = "D://CloudStorage//"+this.fileSel.Name ;                        
            
            try
            {
                // Create the REST request.                
                string requestUrl = string.Format("http://localhost:53003/files/GetFile/{0}", this.fileSel.Name);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);

                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        byte[] buffer = new byte[32768];
                        MemoryStream ms = new MemoryStream();
                        int bytesRead, totalBytesRead = 0;
                        do
                        {
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            totalBytesRead += bytesRead;

                            ms.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);

                        ms.Position = 0;
                        FileStream Sel_file = new FileStream(FilePath, FileMode.Create, System.IO.FileAccess.Write);
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        Sel_file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore while retrieving File: " + ex.Message, "CloudServer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }         
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            // open browse window to select file to upload
            CloudStorage.Cloud_Upload u = new CloudStorage.Cloud_Upload();
            u.Owner = this;
            var ret = u.ShowDialog();

            if (u.DialogResult == DialogResult.OK)
            {
                // Update File List
                this.GetFiles();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // Show Message to confirm delete selected file
                // show message if no selection is made

            // Send Message to file server to remove file from storage
                //http://FileServer_IP:Port/user/clientName/Remove/{File_ID}

            // receive confirmation after file removed
        }

        private void GetFiles()
        {
   //         string FS_Url = CloudStorage.Login.responseText ;
            //string FS_requestUrl = string.Format("{0}/files/GetFiles", FS_Url);
            string FS_requestUrl = string.Format("http://localhost:53003/files/GetFiles");            
            JsonServiceClient client = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);

            List<string> FileList = new List<string>();
            FileList = client.Get<List<string>>("/filelist/karthik/abc");

           WebRequest request = WebRequest.Create(FS_requestUrl);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(FileItem[]));
                    FileItem[] results = (FileItem[])dcs.ReadObject(stream);

                    // Adjust date/time zone
                    foreach (var file in results)
                    {
                        file.UploadedOn = new DateTime(file.UploadedOn.Ticks, DateTimeKind.Utc).ToLocalTime();
                        //dgFiles.Name = file.Name;                        
                    }
            BindingList<string> FileNameList = new BindingList<string>(FileList);
            dgFiles.DataSource = FileNameList;                           
                }
            }         
        }

        private void dgFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            this.fileSel.FileID = (int)dgFiles.Rows[e.RowIndex].Cells[0].Value;
            this.fileSel.Name = (string)dgFiles.Rows[e.RowIndex].Cells[1].Value;
            this.fileSel.FileType = (string)dgFiles.Rows[e.RowIndex].Cells[2].Value;
            this.fileSel.UploadedOn = (DateTime)dgFiles.Rows[e.RowIndex].Cells[3].Value;           
        }
    }
}
