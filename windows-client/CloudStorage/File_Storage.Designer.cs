namespace CloudStorage
{
    partial class File_Storage
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
            this.UserOperations = new System.Windows.Forms.GroupBox();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnUpload = new System.Windows.Forms.Button();
            this.BtnGetFile = new System.Windows.Forms.Button();
            this.dgFiles = new System.Windows.Forms.DataGridView();
            this.UserOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // UserOperations
            // 
            this.UserOperations.Controls.Add(this.BtnDelete);
            this.UserOperations.Controls.Add(this.BtnUpload);
            this.UserOperations.Controls.Add(this.BtnGetFile);
            this.UserOperations.Location = new System.Drawing.Point(563, 154);
            this.UserOperations.Name = "UserOperations";
            this.UserOperations.Size = new System.Drawing.Size(144, 230);
            this.UserOperations.TabIndex = 0;
            this.UserOperations.TabStop = false;
            this.UserOperations.Text = "File Manager";
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(22, 134);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(108, 25);
            this.BtnDelete.TabIndex = 2;
            this.BtnDelete.Text = "Delete File";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnUpload
            // 
            this.BtnUpload.Location = new System.Drawing.Point(22, 83);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(108, 25);
            this.BtnUpload.TabIndex = 1;
            this.BtnUpload.Text = "Upload File";
            this.BtnUpload.UseVisualStyleBackColor = true;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // BtnGetFile
            // 
            this.BtnGetFile.Location = new System.Drawing.Point(22, 35);
            this.BtnGetFile.Name = "BtnGetFile";
            this.BtnGetFile.Size = new System.Drawing.Size(108, 25);
            this.BtnGetFile.TabIndex = 0;
            this.BtnGetFile.Text = "Download File";
            this.BtnGetFile.UseVisualStyleBackColor = true;
            this.BtnGetFile.Click += new System.EventHandler(this.BtnGetFile_Click);
            // 
            // dgFiles
            // 
            this.dgFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFiles.Location = new System.Drawing.Point(39, 12);
            this.dgFiles.Name = "dgFiles";
            this.dgFiles.Size = new System.Drawing.Size(502, 351);
            this.dgFiles.TabIndex = 1;
            this.dgFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgFiles_CellContentClick);
            // 
            // File_Storage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 396);
            this.Controls.Add(this.dgFiles);
            this.Controls.Add(this.UserOperations);
            this.Name = "File_Storage";
            this.Text = "File_Storage";
            this.UserOperations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox UserOperations;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button BtnUpload;
        private System.Windows.Forms.Button BtnGetFile;
        private System.Windows.Forms.DataGridView dgFiles;
    }
}