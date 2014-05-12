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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
namespace CloudStorage
{

    public partial class NewUser : MetroFramework.Forms.MetroForm
    {
        //public static string CLOUD_SERVICE_ENDPOINT = "https://128.84.216.68";
        public static string CLOUD_SERVICE_ENDPOINT = File.ReadAllText("ConfigFile.txt");
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            //Do something to check the certificate is valid. 
            return true;
        }
        public NewUser()
        {
            InitializeComponent();
        }
        public class AddUser
        {

        }

        private void NewUserIdTextBox_Click(object sender, EventArgs e)
        {
            NewUserIdTextBox.Text = "";
        }

        private void NewPassTextBox_Click(object sender, EventArgs e)
        {
            NewPassTextBox.Text = "";
            NewPassTextBox.PasswordChar = '*';
        }
        private void NewPassTextBox_Tab(object sender, EventArgs e)
        {
            NewPassTextBox.Text = "";
            NewPassTextBox.PasswordChar = '*';
        }

        private void NewUserIdTextBox_Tab(object sender, EventArgs e)
        {
            NewUserIdTextBox.Text = "";
        }
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if ((NewUserIdTextBox.Text != "User Name") && (NewUserIdTextBox.Text != "") && (NewPassTextBox.Text != "Password") && (NewPassTextBox.Text != ""))
            {                          
                // Add clientId and Password for authentication.
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                JsonServiceClient Newclient = new JsonServiceClient(CLOUD_SERVICE_ENDPOINT);
                string NewUserUrl = string.Format("/adduser/{0}/{1}", this.NewUserIdTextBox.Text, this.NewPassTextBox.Text);
                try
                {
                    Newclient.Post<object>(NewUserUrl, new AddUser());
                    // Get response and connect to File Server.

                    // Launch User Window
                    this.Hide();
                    CloudStorage.FunctionScreen UserScreen = new CloudStorage.FunctionScreen(this.NewUserIdTextBox.Text, this.NewPassTextBox.Text);
                    UserScreen.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            else
            {
                if((NewUserIdTextBox.Text == "User Name") && (NewUserIdTextBox.Text == ""))
                    MessageBox.Show("Please enter UserName");

                else if ((NewPassTextBox.Text == "Password") && (NewPassTextBox.Text == ""))                
                    MessageBox.Show("Please enter Password");

                else
                    MessageBox.Show("Please enter UserName and Password");
            }
           // to test
           this.Hide();
        }
    }
}
