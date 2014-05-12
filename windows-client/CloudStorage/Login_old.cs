using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using MetroFramework.Forms;
namespace CloudStorage
{
    public partial class Login : MetroForm
    {
        public static string responseText;
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if ((this.ClientName.Text != "") && (this.ClientPassword.Text != ""))
            {
                // Send message to load balancer - http://Load-balancer_IP:Port/users/ClientName                // Create the REST request.
                string url = "http://localhost:53003/";
                //string url = ConfigurationManager.AppSettings["http://Load-Balancer_IP:Port/"];
                string requestUrl = string.Format("{0}users/{1}", url, this.ClientName.Text);
                //string requestUrl = string.Format("{0}/users", url);
                
                WebRequest request = WebRequest.Create(requestUrl);

                // receive response from load balancer - http://FileServer_IP/
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // parse File-System IP and pass to next window
                    //string responseStr = response.ToString();
                    // Stream str = response.GetResponseStream();

                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        responseText = reader.ReadToEnd();
                        // Display welcome message                    
                        this.greetings_output.Text = "Welcome " + this.ClientName.Text;// +". Go to -" + responseText;
                    }
                }
                // Link to File in storage Page                
                this.Hide();
                CloudStorage.File_Storage f = new CloudStorage.File_Storage();
                f.Show();
            }
            else
            {
                if ((this.ClientName.Text == "") && (this.ClientPassword.Text == ""))
                {
                    MessageBox.Show("Please enter UserName and Password");
                }
                else if (this.ClientName.Text == "")
                {
                    MessageBox.Show("Please enter UserName");
                }
                else
                {
                    MessageBox.Show("Please enter Password");
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }          
    }
}
