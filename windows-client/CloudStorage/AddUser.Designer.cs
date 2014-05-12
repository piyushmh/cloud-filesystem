namespace CloudStorage
{
    partial class NewUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AddUserTile = new MetroFramework.Controls.MetroTile();
            this.BtnCreate = new MetroFramework.Controls.MetroButton();
            this.NewPassTextBox = new MetroFramework.Controls.MetroTextBox();
            this.NewUserIdTextBox = new MetroFramework.Controls.MetroTextBox();
            this.AddUserTextBox = new System.Windows.Forms.TextBox();
            this.AddUserTile.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddUserTile
            // 
            this.AddUserTile.ActiveControl = null;
            this.AddUserTile.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.AddUserTile.Controls.Add(this.BtnCreate);
            this.AddUserTile.Controls.Add(this.NewPassTextBox);
            this.AddUserTile.Controls.Add(this.NewUserIdTextBox);
            this.AddUserTile.Controls.Add(this.AddUserTextBox);
            this.AddUserTile.Location = new System.Drawing.Point(534, 64);
            this.AddUserTile.Margin = new System.Windows.Forms.Padding(10);
            this.AddUserTile.Name = "AddUserTile";
            this.AddUserTile.Size = new System.Drawing.Size(381, 439);
            this.AddUserTile.Style = MetroFramework.MetroColorStyle.Black;
            this.AddUserTile.TabIndex = 0;
            this.AddUserTile.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.AddUserTile.Theme = MetroFramework.MetroThemeStyle.Light;
            this.AddUserTile.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.AddUserTile.UseSelectable = true;
            // 
            // BtnCreate
            // 
            this.BtnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BtnCreate.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.BtnCreate.ForeColor = System.Drawing.Color.Black;
            this.BtnCreate.Location = new System.Drawing.Point(33, 302);
            this.BtnCreate.Name = "BtnCreate";
            this.BtnCreate.Size = new System.Drawing.Size(340, 47);
            this.BtnCreate.TabIndex = 3;
            this.BtnCreate.Text = "CREATE";
            this.BtnCreate.UseCustomBackColor = true;
            this.BtnCreate.UseCustomForeColor = true;
            this.BtnCreate.UseSelectable = true;
            this.BtnCreate.UseStyleColors = true;
            this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // NewPassTextBox
            // 
            this.NewPassTextBox.BackColor = System.Drawing.Color.Black;
            this.NewPassTextBox.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.NewPassTextBox.Lines = new string[] {
        "Password"};
            this.NewPassTextBox.Location = new System.Drawing.Point(33, 221);
            this.NewPassTextBox.MaxLength = 32767;
            this.NewPassTextBox.Name = "NewPassTextBox";
            this.NewPassTextBox.PasswordChar = '\0';
            this.NewPassTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NewPassTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.NewPassTextBox.SelectedText = "";
            this.NewPassTextBox.Size = new System.Drawing.Size(340, 37);
            this.NewPassTextBox.Style = MetroFramework.MetroColorStyle.Silver;
            this.NewPassTextBox.TabIndex = 2;
            this.NewPassTextBox.Text = "Password";
            this.NewPassTextBox.UseCustomBackColor = true;
            this.NewPassTextBox.UseCustomForeColor = true;
            this.NewPassTextBox.UseSelectable = true;
            this.NewPassTextBox.UseStyleColors = true;
            this.NewPassTextBox.Click += new System.EventHandler(this.NewPassTextBox_Click);
            this.NewPassTextBox.Enter += new System.EventHandler(this.NewPassTextBox_Tab);
            // 
            // NewUserIdTextBox
            // 
            this.NewUserIdTextBox.BackColor = System.Drawing.Color.Black;
            this.NewUserIdTextBox.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.NewUserIdTextBox.Lines = new string[] {
        "New User"};
            this.NewUserIdTextBox.Location = new System.Drawing.Point(33, 149);
            this.NewUserIdTextBox.MaxLength = 32767;
            this.NewUserIdTextBox.Name = "NewUserIdTextBox";
            this.NewUserIdTextBox.PasswordChar = '\0';
            this.NewUserIdTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NewUserIdTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.NewUserIdTextBox.SelectedText = "";
            this.NewUserIdTextBox.Size = new System.Drawing.Size(340, 37);
            this.NewUserIdTextBox.Style = MetroFramework.MetroColorStyle.Silver;
            this.NewUserIdTextBox.TabIndex = 1;
            this.NewUserIdTextBox.Text = "New User";
            this.NewUserIdTextBox.Theme = MetroFramework.MetroThemeStyle.Light;
            this.NewUserIdTextBox.UseCustomBackColor = true;
            this.NewUserIdTextBox.UseCustomForeColor = true;
            this.NewUserIdTextBox.UseSelectable = true;
            this.NewUserIdTextBox.UseStyleColors = true;
            this.NewUserIdTextBox.Click += new System.EventHandler(this.NewUserIdTextBox_Click);
            this.NewUserIdTextBox.Enter += new System.EventHandler(this.NewUserIdTextBox_Tab);
            // 
            // AddUserTextBox
            // 
            this.AddUserTextBox.BackColor = System.Drawing.Color.Black;
            this.AddUserTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AddUserTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.AddUserTextBox.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddUserTextBox.ForeColor = System.Drawing.Color.DarkGray;
            this.AddUserTextBox.Location = new System.Drawing.Point(29, 33);
            this.AddUserTextBox.Name = "AddUserTextBox";
            this.AddUserTextBox.Size = new System.Drawing.Size(230, 59);
            this.AddUserTextBox.TabIndex = 0;
            this.AddUserTextBox.Text = "Add User";
            this.AddUserTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NewUser
            // 
            this.ApplyImageInvert = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CloudStorage.Properties.Resources.blurry_vision_by_loucrow_d3aj9pb;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackImage = global::CloudStorage.Properties.Resources.blurry_vision_by_loucrow_d3aj9pb;
            this.BackImagePadding = new System.Windows.Forms.Padding(-1);
            this.BackLocation = MetroFramework.Forms.BackLocation.TopRight;
            this.BackMaxSize = 1000;
            this.ClientSize = new System.Drawing.Size(960, 540);
            this.Controls.Add(this.AddUserTile);
            this.ForeColor = System.Drawing.Color.DarkGray;
            this.Name = "NewUser";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "SONIC CLOUD";
            this.AddUserTile.ResumeLayout(false);
            this.AddUserTile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile AddUserTile;
        private System.Windows.Forms.TextBox AddUserTextBox;
        private MetroFramework.Controls.MetroTextBox NewUserIdTextBox;
        private MetroFramework.Controls.MetroTextBox NewPassTextBox;
        private MetroFramework.Controls.MetroButton BtnCreate;
    }
}