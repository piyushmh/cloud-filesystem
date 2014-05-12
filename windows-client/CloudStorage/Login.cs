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
namespace CloudStorage
{
    public partial class Login : MetroFramework.Forms.MetroForm
    {
        public static string CLOUD_SERVICE_ENDPOINT = "http://128.84.216.57:8080";
 //       public ClientInfo ConnectedUser;
        public Login()
        {
            InitializeComponent();                      
        }

        private void Btnsignin_Click(object sender, EventArgs e)
        {
            
           if ((UserIdTextBox.Text != "User Name") && (UserIdTextBox.Text != "") && (PasswordTextBox.Text != "Password") && (PasswordTextBox.Text != ""))
            {
                // save user namen and password in client info   
             //   ConnectedUser = new ClientInfo(this.UserIdTextBox.Text, this.PasswordTextBox.Text);

                // send clientId and Password for authentication.

                // Get response and connect to File Server.

                // Launch User Window
                this.Hide();
                CloudStorage.FunctionScreen UserScreen = new CloudStorage.FunctionScreen(this.UserIdTextBox.Text, this.PasswordTextBox.Text);
                UserScreen.Show();
            }
            else
            {
                if((UserIdTextBox.Text == "User Name") && (UserIdTextBox.Text == ""))
                    MessageBox.Show("Please enter UserName");

                else if ((PasswordTextBox.Text == "Password") && (PasswordTextBox.Text == ""))                
                    MessageBox.Show("Please enter Password");

                else
                    MessageBox.Show("Please enter UserName and Password");
            }
            /* to test
           this.Hide();
           CloudStorage.FunctionScreen UserScreen = new CloudStorage.FunctionScreen("karthik", "abc");
           UserScreen.Show();    
 */                          
        }

        private void PasswordTextBox_Click(object sender, EventArgs e)
        {
            PasswordTextBox.Text = "";
            PasswordTextBox.PasswordChar = '*';
        }

        private void UserIdTextBox_Click(object sender, EventArgs e)
        {
            UserIdTextBox.Text = "";
        }

        private void PasswordTextBox_Tab(object sender, EventArgs e)
        {
            PasswordTextBox.Text = "";
            PasswordTextBox.PasswordChar = '*';
        }

        private void UserIdTextBox_Tab(object sender, EventArgs e)
        {
            UserIdTextBox.Text = "";
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            CloudStorage.NewUser NewUserForm = new NewUser();
            NewUserForm.Show();
            this.Hide();
        }               


    }
}
