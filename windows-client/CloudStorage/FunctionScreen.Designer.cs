using System.Windows.Forms;
namespace CloudStorage
{
    partial class FunctionScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Tabs = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.BtnRefresh = new MetroFramework.Controls.MetroButton();
            this.UsageTextBox = new System.Windows.Forms.TextBox();
            this.FileList_dataGridView = new System.Windows.Forms.DataGridView();
            this.DirectoryTextBox = new MetroFramework.Controls.MetroTextBox();
            this.UsageProgressBar = new MetroFramework.Controls.MetroProgressBar();
            this.BtnUpload = new MetroFramework.Controls.MetroButton();
            this.BtnRecent = new MetroFramework.Controls.MetroButton();
            this.BtnBack = new MetroFramework.Controls.MetroButton();
            this.BtnHome = new MetroFramework.Controls.MetroButton();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.SharedByDataList = new System.Windows.Forms.DataGridView();
            this.SharedWithDataList = new System.Windows.Forms.DataGridView();
            this.BtnWith = new MetroFramework.Controls.MetroButton();
            this.BtnBy = new MetroFramework.Controls.MetroButton();
            this.metroTabPage4 = new MetroFramework.Controls.MetroTabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PicturetextBox = new System.Windows.Forms.TextBox();
            this.BtnCancel = new MetroFramework.Controls.MetroButton();
            this.BtnUpdate = new MetroFramework.Controls.MetroButton();
            this.EMailInp_textBox = new System.Windows.Forms.TextBox();
            this.ConfPasswordInp_textBox = new System.Windows.Forms.TextBox();
            this.NewPassInp_textBox = new System.Windows.Forms.TextBox();
            this.CurrPasswordInp_textBox = new System.Windows.Forms.TextBox();
            this.UserNameDisp_textBox = new System.Windows.Forms.TextBox();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.ConfPasswordtextBox = new System.Windows.Forms.TextBox();
            this.NewPasswordBox = new System.Windows.Forms.TextBox();
            this.CurPassTextBox = new System.Windows.Forms.TextBox();
            this.UsertextBox = new System.Windows.Forms.TextBox();
            this.SignOutLink = new MetroFramework.Controls.MetroLink();
            this.UserIdTextBox = new System.Windows.Forms.TextBox();
            this.UserpictureBox = new System.Windows.Forms.PictureBox();
            this.Tabs.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileList_dataGridView)).BeginInit();
            this.metroTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SharedByDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharedWithDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserpictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.metroTabPage1);
            this.Tabs.Controls.Add(this.metroTabPage2);
            this.Tabs.Controls.Add(this.metroTabPage4);
            this.Tabs.FontSize = MetroFramework.MetroTabControlSize.Tall;
            this.Tabs.ItemSize = new System.Drawing.Size(500, 34);
            this.Tabs.Location = new System.Drawing.Point(38, 85);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(889, 402);
            this.Tabs.TabIndex = 0;
            this.Tabs.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Tabs.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.BackColor = System.Drawing.Color.Transparent;
            this.metroTabPage1.Controls.Add(this.BtnRefresh);
            this.metroTabPage1.Controls.Add(this.UsageTextBox);
            this.metroTabPage1.Controls.Add(this.FileList_dataGridView);
            this.metroTabPage1.Controls.Add(this.DirectoryTextBox);
            this.metroTabPage1.Controls.Add(this.UsageProgressBar);
            this.metroTabPage1.Controls.Add(this.BtnUpload);
            this.metroTabPage1.Controls.Add(this.BtnRecent);
            this.metroTabPage1.Controls.Add(this.BtnBack);
            this.metroTabPage1.Controls.Add(this.BtnHome);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = true;
            this.metroTabPage1.HorizontalScrollbarSize = 100;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(881, 360);
            this.metroTabPage1.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Files        ";
            this.metroTabPage1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.BtnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRefresh.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.BtnRefresh.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnRefresh.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnRefresh.Location = new System.Drawing.Point(729, 15);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(68, 25);
            this.BtnRefresh.Style = MetroFramework.MetroColorStyle.Black;
            this.BtnRefresh.TabIndex = 19;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnRefresh.UseCustomBackColor = true;
            this.BtnRefresh.UseCustomForeColor = true;
            this.BtnRefresh.UseSelectable = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // UsageTextBox
            // 
            this.UsageTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.UsageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UsageTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsageTextBox.ForeColor = System.Drawing.Color.DarkGray;
            this.UsageTextBox.Location = new System.Drawing.Point(4, 193);
            this.UsageTextBox.Name = "UsageTextBox";
            this.UsageTextBox.Size = new System.Drawing.Size(72, 16);
            this.UsageTextBox.TabIndex = 18;
            this.UsageTextBox.Text = "10% of 10 GB";
            // 
            // FileList_dataGridView
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White;
            this.FileList_dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.FileList_dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.FileList_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileList_dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.FileList_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.FileList_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.FileList_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FileList_dataGridView.ColumnHeadersVisible = false;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.FileList_dataGridView.DefaultCellStyle = dataGridViewCellStyle17;
            this.FileList_dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.FileList_dataGridView.Location = new System.Drawing.Point(123, 56);
            this.FileList_dataGridView.Name = "FileList_dataGridView";
            this.FileList_dataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White;
            this.FileList_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.FileList_dataGridView.Size = new System.Drawing.Size(600, 264);
            this.FileList_dataGridView.TabIndex = 10;
            this.FileList_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.FileList_dataGridView_CellContentClick);
            this.FileList_dataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.FileList_dataGridView_CellDoubleClick);
            // 
            // DirectoryTextBox
            // 
            this.DirectoryTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.DirectoryTextBox.ForeColor = System.Drawing.Color.DarkGray;
            this.DirectoryTextBox.Lines = new string[0];
            this.DirectoryTextBox.Location = new System.Drawing.Point(123, 10);
            this.DirectoryTextBox.MaxLength = 32767;
            this.DirectoryTextBox.Name = "DirectoryTextBox";
            this.DirectoryTextBox.PasswordChar = '\0';
            this.DirectoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.DirectoryTextBox.SelectedText = "";
            this.DirectoryTextBox.Size = new System.Drawing.Size(600, 35);
            this.DirectoryTextBox.Style = MetroFramework.MetroColorStyle.Black;
            this.DirectoryTextBox.TabIndex = 9;
            this.DirectoryTextBox.UseCustomBackColor = true;
            this.DirectoryTextBox.UseCustomForeColor = true;
            this.DirectoryTextBox.UseSelectable = true;
            this.DirectoryTextBox.UseStyleColors = true;
            // 
            // UsageProgressBar
            // 
            this.UsageProgressBar.Location = new System.Drawing.Point(7, 211);
            this.UsageProgressBar.Name = "UsageProgressBar";
            this.UsageProgressBar.Size = new System.Drawing.Size(65, 10);
            this.UsageProgressBar.TabIndex = 6;
            this.UsageProgressBar.Value = 50;
            // 
            // BtnUpload
            // 
            this.BtnUpload.BackColor = System.Drawing.Color.Transparent;
            this.BtnUpload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUpload.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.BtnUpload.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnUpload.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnUpload.Location = new System.Drawing.Point(3, 136);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(68, 25);
            this.BtnUpload.Style = MetroFramework.MetroColorStyle.Black;
            this.BtnUpload.TabIndex = 5;
            this.BtnUpload.Text = "Upload";
            this.BtnUpload.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnUpload.UseCustomBackColor = true;
            this.BtnUpload.UseCustomForeColor = true;
            this.BtnUpload.UseSelectable = true;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // BtnRecent
            // 
            this.BtnRecent.BackColor = System.Drawing.Color.Transparent;
            this.BtnRecent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnRecent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRecent.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.BtnRecent.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnRecent.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnRecent.Location = new System.Drawing.Point(3, 96);
            this.BtnRecent.Name = "BtnRecent";
            this.BtnRecent.Size = new System.Drawing.Size(68, 25);
            this.BtnRecent.Style = MetroFramework.MetroColorStyle.Black;
            this.BtnRecent.TabIndex = 4;
            this.BtnRecent.Text = "Recent";
            this.BtnRecent.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnRecent.UseCustomBackColor = true;
            this.BtnRecent.UseCustomForeColor = true;
            this.BtnRecent.UseSelectable = true;
            this.BtnRecent.Click += new System.EventHandler(this.BtnRecent_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.BackColor = System.Drawing.Color.Transparent;
            this.BtnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBack.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.BtnBack.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnBack.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnBack.Location = new System.Drawing.Point(3, 56);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(68, 25);
            this.BtnBack.Style = MetroFramework.MetroColorStyle.Black;
            this.BtnBack.TabIndex = 3;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "Back";
            this.BtnBack.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnBack.UseCustomBackColor = true;
            this.BtnBack.UseCustomForeColor = true;
            this.BtnBack.UseSelectable = true;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnHome
            // 
            this.BtnHome.BackColor = System.Drawing.Color.Transparent;
            this.BtnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnHome.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.BtnHome.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnHome.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnHome.Location = new System.Drawing.Point(3, 15);
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.Size = new System.Drawing.Size(68, 25);
            this.BtnHome.Style = MetroFramework.MetroColorStyle.Black;
            this.BtnHome.TabIndex = 2;
            this.BtnHome.Text = "Home";
            this.BtnHome.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnHome.UseCustomBackColor = true;
            this.BtnHome.UseCustomForeColor = true;
            this.BtnHome.UseSelectable = true;
            this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.Controls.Add(this.SharedByDataList);
            this.metroTabPage2.Controls.Add(this.SharedWithDataList);
            this.metroTabPage2.Controls.Add(this.BtnWith);
            this.metroTabPage2.Controls.Add(this.BtnBy);
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 100;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(881, 360);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "Share        ";
            this.metroTabPage2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            // 
            // SharedByDataList
            // 
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.DimGray;
            this.SharedByDataList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
            this.SharedByDataList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.SharedByDataList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.SharedByDataList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SharedByDataList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SharedByDataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.SharedByDataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SharedByDataList.ColumnHeadersVisible = false;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.SharedByDataList.DefaultCellStyle = dataGridViewCellStyle21;
            this.SharedByDataList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SharedByDataList.Location = new System.Drawing.Point(140, 192);
            this.SharedByDataList.Name = "SharedByDataList";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SharedByDataList.RowHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.SharedByDataList.RowHeadersVisible = false;
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle23.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.Color.DimGray;
            this.SharedByDataList.RowsDefaultCellStyle = dataGridViewCellStyle23;
            this.SharedByDataList.Size = new System.Drawing.Size(600, 168);
            this.SharedByDataList.TabIndex = 5;
            this.SharedByDataList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SharedByDataList_CellContentClick);
            // 
            // SharedWithDataList
            // 
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle24.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.Color.DimGray;
            this.SharedWithDataList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle24;
            this.SharedWithDataList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.SharedWithDataList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.SharedWithDataList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SharedWithDataList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SharedWithDataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.SharedWithDataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SharedWithDataList.ColumnHeadersVisible = false;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.SharedWithDataList.DefaultCellStyle = dataGridViewCellStyle26;
            this.SharedWithDataList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SharedWithDataList.Location = new System.Drawing.Point(140, 10);
            this.SharedWithDataList.Name = "SharedWithDataList";
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SharedWithDataList.RowHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.SharedWithDataList.RowHeadersVisible = false;
            dataGridViewCellStyle28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle28.ForeColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.Color.DimGray;
            this.SharedWithDataList.RowsDefaultCellStyle = dataGridViewCellStyle28;
            this.SharedWithDataList.Size = new System.Drawing.Size(600, 168);
            this.SharedWithDataList.TabIndex = 4;
            this.SharedWithDataList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SharedWithDataList_CellContentClick);
            // 
            // BtnWith
            // 
            this.BtnWith.BackColor = System.Drawing.Color.Transparent;
            this.BtnWith.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnWith.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.BtnWith.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnWith.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnWith.Location = new System.Drawing.Point(5, 10);
            this.BtnWith.Name = "BtnWith";
            this.BtnWith.Size = new System.Drawing.Size(105, 31);
            this.BtnWith.Style = MetroFramework.MetroColorStyle.White;
            this.BtnWith.TabIndex = 3;
            this.BtnWith.Text = "Shared with you";
            this.BtnWith.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnWith.UseCustomBackColor = true;
            this.BtnWith.UseCustomForeColor = true;
            this.BtnWith.UseSelectable = true;
            this.BtnWith.UseStyleColors = true;
            this.BtnWith.Click += new System.EventHandler(this.BtnWith_Click);
            // 
            // BtnBy
            // 
            this.BtnBy.BackColor = System.Drawing.Color.Transparent;
            this.BtnBy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnBy.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.BtnBy.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.BtnBy.ForeColor = System.Drawing.Color.DarkGray;
            this.BtnBy.Location = new System.Drawing.Point(5, 192);
            this.BtnBy.Name = "BtnBy";
            this.BtnBy.Size = new System.Drawing.Size(105, 31);
            this.BtnBy.TabIndex = 2;
            this.BtnBy.Text = "Shared by you";
            this.BtnBy.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BtnBy.UseCustomBackColor = true;
            this.BtnBy.UseCustomForeColor = true;
            this.BtnBy.UseSelectable = true;
            this.BtnBy.UseStyleColors = true;
            this.BtnBy.Click += new System.EventHandler(this.BtnBy_Click);
            // 
            // metroTabPage4
            // 
            this.metroTabPage4.HorizontalScrollbarBarColor = true;
            this.metroTabPage4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage4.HorizontalScrollbarSize = 100;
            this.metroTabPage4.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage4.Name = "metroTabPage4";
            this.metroTabPage4.Size = new System.Drawing.Size(881, 360);
            this.metroTabPage4.TabIndex = 3;
            this.metroTabPage4.Text = "About Us         ";
            this.metroTabPage4.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabPage4.VerticalScrollbarBarColor = true;
            this.metroTabPage4.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage4.VerticalScrollbarSize = 10;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // PicturetextBox
            // 
            this.PicturetextBox.Location = new System.Drawing.Point(0, 0);
            this.PicturetextBox.Name = "PicturetextBox";
            this.PicturetextBox.Size = new System.Drawing.Size(100, 20);
            this.PicturetextBox.TabIndex = 0;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(0, 0);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 0;
            this.BtnCancel.UseSelectable = true;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(0, 0);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(75, 23);
            this.BtnUpdate.TabIndex = 0;
            this.BtnUpdate.UseSelectable = true;
            // 
            // EMailInp_textBox
            // 
            this.EMailInp_textBox.Location = new System.Drawing.Point(0, 0);
            this.EMailInp_textBox.Name = "EMailInp_textBox";
            this.EMailInp_textBox.Size = new System.Drawing.Size(100, 20);
            this.EMailInp_textBox.TabIndex = 0;
            // 
            // ConfPasswordInp_textBox
            // 
            this.ConfPasswordInp_textBox.Location = new System.Drawing.Point(0, 0);
            this.ConfPasswordInp_textBox.Name = "ConfPasswordInp_textBox";
            this.ConfPasswordInp_textBox.Size = new System.Drawing.Size(100, 20);
            this.ConfPasswordInp_textBox.TabIndex = 0;
            // 
            // NewPassInp_textBox
            // 
            this.NewPassInp_textBox.Location = new System.Drawing.Point(0, 0);
            this.NewPassInp_textBox.Name = "NewPassInp_textBox";
            this.NewPassInp_textBox.Size = new System.Drawing.Size(100, 20);
            this.NewPassInp_textBox.TabIndex = 0;
            // 
            // CurrPasswordInp_textBox
            // 
            this.CurrPasswordInp_textBox.Location = new System.Drawing.Point(0, 0);
            this.CurrPasswordInp_textBox.Name = "CurrPasswordInp_textBox";
            this.CurrPasswordInp_textBox.Size = new System.Drawing.Size(100, 20);
            this.CurrPasswordInp_textBox.TabIndex = 0;
            // 
            // UserNameDisp_textBox
            // 
            this.UserNameDisp_textBox.Location = new System.Drawing.Point(0, 0);
            this.UserNameDisp_textBox.Name = "UserNameDisp_textBox";
            this.UserNameDisp_textBox.Size = new System.Drawing.Size(100, 20);
            this.UserNameDisp_textBox.TabIndex = 0;
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Location = new System.Drawing.Point(0, 0);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(100, 20);
            this.EmailTextBox.TabIndex = 0;
            // 
            // ConfPasswordtextBox
            // 
            this.ConfPasswordtextBox.Location = new System.Drawing.Point(0, 0);
            this.ConfPasswordtextBox.Name = "ConfPasswordtextBox";
            this.ConfPasswordtextBox.Size = new System.Drawing.Size(100, 20);
            this.ConfPasswordtextBox.TabIndex = 0;
            // 
            // NewPasswordBox
            // 
            this.NewPasswordBox.Location = new System.Drawing.Point(0, 0);
            this.NewPasswordBox.Name = "NewPasswordBox";
            this.NewPasswordBox.Size = new System.Drawing.Size(100, 20);
            this.NewPasswordBox.TabIndex = 0;
            // 
            // CurPassTextBox
            // 
            this.CurPassTextBox.Location = new System.Drawing.Point(0, 0);
            this.CurPassTextBox.Name = "CurPassTextBox";
            this.CurPassTextBox.Size = new System.Drawing.Size(100, 20);
            this.CurPassTextBox.TabIndex = 0;
            // 
            // UsertextBox
            // 
            this.UsertextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.UsertextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UsertextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsertextBox.ForeColor = System.Drawing.Color.DarkGray;
            this.UsertextBox.Location = new System.Drawing.Point(106, 57);
            this.UsertextBox.Name = "UsertextBox";
            this.UsertextBox.Size = new System.Drawing.Size(131, 26);
            this.UsertextBox.TabIndex = 2;
            this.UsertextBox.Text = "User Name";
            this.UsertextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SignOutLink
            // 
            this.SignOutLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SignOutLink.ForeColor = System.Drawing.Color.DimGray;
            this.SignOutLink.Location = new System.Drawing.Point(852, 27);
            this.SignOutLink.Name = "SignOutLink";
            this.SignOutLink.Size = new System.Drawing.Size(75, 16);
            this.SignOutLink.TabIndex = 2;
            this.SignOutLink.Text = "SIGN OUT";
            this.SignOutLink.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.SignOutLink.UseCustomForeColor = true;
            this.SignOutLink.UseSelectable = true;
            this.SignOutLink.Click += new System.EventHandler(this.SignOutLink_Click);
            // 
            // UserIdTextBox
            // 
            this.UserIdTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.UserIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserIdTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserIdTextBox.ForeColor = System.Drawing.Color.DarkGray;
            this.UserIdTextBox.Location = new System.Drawing.Point(703, 63);
            this.UserIdTextBox.Name = "UserIdTextBox";
            this.UserIdTextBox.Size = new System.Drawing.Size(163, 26);
            this.UserIdTextBox.TabIndex = 17;
            // 
            // UserpictureBox
            // 
            this.UserpictureBox.BackColor = System.Drawing.Color.White;
            this.UserpictureBox.BackgroundImage = global::CloudStorage.Properties.Resources.DarthVader64x64;
            this.UserpictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.UserpictureBox.Location = new System.Drawing.Point(619, 27);
            this.UserpictureBox.Name = "UserpictureBox";
            this.UserpictureBox.Size = new System.Drawing.Size(64, 64);
            this.UserpictureBox.TabIndex = 3;
            this.UserpictureBox.TabStop = false;
            // 
            // FunctionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 540);
            this.Controls.Add(this.UserIdTextBox);
            this.Controls.Add(this.UserpictureBox);
            this.Controls.Add(this.SignOutLink);
            this.Controls.Add(this.Tabs);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FunctionScreen";
            this.Text = "SONIC CLOUD";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Tabs.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.metroTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileList_dataGridView)).EndInit();
            this.metroTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SharedByDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharedWithDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserpictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTabPage metroTabPage2;
//        private MetroFramework.Controls.MetroTabPage metroTabPage3;
        private MetroFramework.Controls.MetroTabPage metroTabPage4;
//        private MetroFramework.Controls.MetroTabPage metroTabPage5;
        private MetroFramework.Controls.MetroTabControl Tabs;
        private MetroFramework.Controls.MetroLink SignOutLink;
        private System.Windows.Forms.PictureBox UserpictureBox;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTextBox DirectoryTextBox;
        private MetroFramework.Controls.MetroProgressBar UsageProgressBar;
        private MetroFramework.Controls.MetroButton BtnUpload;
        private MetroFramework.Controls.MetroButton BtnRecent;
        private MetroFramework.Controls.MetroButton BtnBack;
        private MetroFramework.Controls.MetroButton BtnHome;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.TextBox ConfPasswordtextBox;
        private System.Windows.Forms.TextBox NewPasswordBox;
        private System.Windows.Forms.TextBox CurPassTextBox;
        private System.Windows.Forms.TextBox UsertextBox;
        private System.Windows.Forms.TextBox EMailInp_textBox;
        private System.Windows.Forms.TextBox ConfPasswordInp_textBox;
        private System.Windows.Forms.TextBox NewPassInp_textBox;
        private System.Windows.Forms.TextBox CurrPasswordInp_textBox;
        private System.Windows.Forms.TextBox UserNameDisp_textBox;
        private MetroFramework.Controls.MetroButton BtnCancel;
        private MetroFramework.Controls.MetroButton BtnUpdate;
        private MetroFramework.Controls.MetroButton BtnWith;
        private MetroFramework.Controls.MetroButton BtnBy;
        private System.Windows.Forms.DataGridView SharedWithDataList;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox PicturetextBox;
        private DataGridView FileList_dataGridView;
        private TextBox UserIdTextBox;
        private TextBox UsageTextBox;
        private MetroFramework.Controls.MetroButton BtnRefresh;
        private DataGridView SharedByDataList;
    }
}