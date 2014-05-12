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
using ServiceStack;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CloudStorage
{
    public partial class UserWindow : MetroFramework.Forms.MetroForm
    {
        public static string CLOUD_SERVICE_ENDPOINT = "http://128.84.216.57:8080";
        public UserWindow()
        {
            InitializeComponent();
            this.GetFiles();
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            // open browse window to select file to upload
         /*   CloudStorage.FileUpload userUpload = new CloudStorage.FileUpload();
            userUpload.Owner = this;
            var ret = userUpload.ShowDialog();

            if (userUpload.DialogResult == DialogResult.OK)
            {
                // Update File List
   //             this.GetFiles();
            }*/
        }

        private void GetFiles()
        {
            string FS_requestUrl = string.Format("http://localhost:53003/files/GetFiles");
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
                        FileListGrid.Name = file.Name;
                    }
                    FileListGrid.DataSource = results;
                }
            }
    /*        JsonServiceClient client = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);

            List<string> FileList = new List<string>();
            FileList = client.Get<List<string>>("/filelist/karthik/abc");

            BindingList<string> FileNameList = new BindingList<string>(FileList);*/
            
        }
    }
}
