namespace CloudStorage
{
    partial class Cloud_Upload
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
            this.FileName_disp = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.FileName_txt = new System.Windows.Forms.TextBox();
            this.btn_Browse = new System.Windows.Forms.Button();
            this.btn_Upload = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.Result_txt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FileName_disp
            // 
            this.FileName_disp.BackColor = System.Drawing.SystemColors.Control;
            this.FileName_disp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileName_disp.Location = new System.Drawing.Point(36, 86);
            this.FileName_disp.Name = "FileName_disp";
            this.FileName_disp.ReadOnly = true;
            this.FileName_disp.Size = new System.Drawing.Size(60, 13);
            this.FileName_disp.TabIndex = 100;
            this.FileName_disp.Text = "File name: ";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.HideSelection = false;
            this.textBox2.Location = new System.Drawing.Point(9, 9);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(307, 31);
            this.textBox2.TabIndex = 101;
            this.textBox2.Text = "Upload a File";
            // 
            // FileName_txt
            // 
            this.FileName_txt.Location = new System.Drawing.Point(102, 81);
            this.FileName_txt.Name = "FileName_txt";
            this.FileName_txt.Size = new System.Drawing.Size(315, 20);
            this.FileName_txt.TabIndex = 0;
            // 
            // btn_Browse
            // 
            this.btn_Browse.Location = new System.Drawing.Point(438, 74);
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.Size = new System.Drawing.Size(98, 25);
            this.btn_Browse.TabIndex = 1;
            this.btn_Browse.Text = "Browse";
            this.btn_Browse.UseVisualStyleBackColor = true;
            this.btn_Browse.Click += new System.EventHandler(this.btn_Browse_Click);
            // 
            // btn_Upload
            // 
            this.btn_Upload.Location = new System.Drawing.Point(331, 177);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(100, 29);
            this.btn_Upload.TabIndex = 2;
            this.btn_Upload.Text = "Upload";
            this.btn_Upload.UseVisualStyleBackColor = true;
            this.btn_Upload.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(439, 178);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(97, 28);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // Result_txt
            // 
            this.Result_txt.BackColor = System.Drawing.SystemColors.Control;
            this.Result_txt.Location = new System.Drawing.Point(66, 120);
            this.Result_txt.Name = "Result_txt";
            this.Result_txt.Size = new System.Drawing.Size(400, 20);
            this.Result_txt.TabIndex = 102;
            // 
            // Cloud_Upload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 206);
            this.Controls.Add(this.Result_txt);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Upload);
            this.Controls.Add(this.btn_Browse);
            this.Controls.Add(this.FileName_txt);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.FileName_disp);
            this.Name = "Cloud_Upload";
            this.Text = "Cloud_Upload";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FileName_disp;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox FileName_txt;
        private System.Windows.Forms.Button btn_Browse;
        private System.Windows.Forms.Button btn_Upload;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.TextBox Result_txt;
    }
}