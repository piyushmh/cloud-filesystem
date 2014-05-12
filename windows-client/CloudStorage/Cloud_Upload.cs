using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace CloudStorage
{
    public partial class Cloud_Upload : Form
    {
        public Cloud_Upload()
        {
            InitializeComponent();
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {            
            if(string.IsNullOrWhiteSpace(FileName_txt.Text))
            {
                MessageBox.Show("Specifiy a file to upload.", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                FileName_txt.Focus();            
            }
            else if (!File.Exists(FileName_txt.Text))
            {
                string message = string.Format("Unable to find '{0}'. Please check the file name and try again.", FileName_txt.Text);
                MessageBox.Show(message, "Upload",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                FileName_txt.Focus(); 
            }
            else 
            {
                try
                {
                    string CU_requestUrl = string.Format("http://localhost:53003/files/UploadFile/{0}/asd", System.IO.Path.GetFileName(this.FileName_txt.Text));
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(CU_requestUrl);
                    request.Method = "POST";
                    request.ContentType = "text/plain";

                    byte[] fileToSend = File.ReadAllBytes(FileName_txt.Text);
                    request.ContentLength = fileToSend.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        // Send the file as body request.
                        requestStream.Write(fileToSend, 0, fileToSend.Length);
                        requestStream.Close();
                    }

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        Result_txt.Text = "HTTP/" + response.ProtocolVersion + " " +(int)response.StatusCode+" " +response.StatusDescription;

                    MessageBox.Show("File sucessfully uploaded.", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during file upload: " + ex.Message, "Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }            
        }

        private void btn_Browse_Click(object sender, EventArgs e)
        {
            var fd = new System.Windows.Forms.OpenFileDialog();
            fd.Filter =   "All Files (*.*)|*.*";
            var ret = fd.ShowDialog();

            this.FileName_txt.Text = fd.FileName;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
