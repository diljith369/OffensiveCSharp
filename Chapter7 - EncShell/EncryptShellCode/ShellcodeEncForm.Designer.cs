namespace EncryptShellCode
{
    partial class ShellcodeEncForm
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
            this.txtShellcode = new System.Windows.Forms.TextBox();
            this.txtEncShellcode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEncryptShellCode = new System.Windows.Forms.Button();
            this.txtIV = new System.Windows.Forms.TextBox();
            this.txtkey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnKeyGen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtShellcode
            // 
            this.txtShellcode.Location = new System.Drawing.Point(17, 23);
            this.txtShellcode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtShellcode.Multiline = true;
            this.txtShellcode.Name = "txtShellcode";
            this.txtShellcode.Size = new System.Drawing.Size(426, 316);
            this.txtShellcode.TabIndex = 0;
            // 
            // txtEncShellcode
            // 
            this.txtEncShellcode.Location = new System.Drawing.Point(450, 23);
            this.txtEncShellcode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtEncShellcode.Multiline = true;
            this.txtEncShellcode.Name = "txtEncShellcode";
            this.txtEncShellcode.Size = new System.Drawing.Size(398, 434);
            this.txtEncShellcode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Shell Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(448, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ecrypted Shell Code";
            // 
            // btnEncryptShellCode
            // 
            this.btnEncryptShellCode.Location = new System.Drawing.Point(304, 422);
            this.btnEncryptShellCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEncryptShellCode.Name = "btnEncryptShellCode";
            this.btnEncryptShellCode.Size = new System.Drawing.Size(139, 37);
            this.btnEncryptShellCode.TabIndex = 4;
            this.btnEncryptShellCode.Text = "Encrypt It";
            this.btnEncryptShellCode.UseVisualStyleBackColor = true;
            this.btnEncryptShellCode.Click += new System.EventHandler(this.btnEncryptShellCode_Click);
            // 
            // txtIV
            // 
            this.txtIV.Location = new System.Drawing.Point(17, 397);
            this.txtIV.Name = "txtIV";
            this.txtIV.Size = new System.Drawing.Size(426, 20);
            this.txtIV.TabIndex = 5;
            // 
            // txtkey
            // 
            this.txtkey.Location = new System.Drawing.Point(17, 357);
            this.txtkey.Name = "txtkey";
            this.txtkey.Size = new System.Drawing.Size(426, 20);
            this.txtkey.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 341);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Key";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 381);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "IV";
            // 
            // btnKeyGen
            // 
            this.btnKeyGen.Location = new System.Drawing.Point(17, 422);
            this.btnKeyGen.Margin = new System.Windows.Forms.Padding(2);
            this.btnKeyGen.Name = "btnKeyGen";
            this.btnKeyGen.Size = new System.Drawing.Size(148, 37);
            this.btnKeyGen.TabIndex = 9;
            this.btnKeyGen.Text = "Get Key and  IV ";
            this.btnKeyGen.UseVisualStyleBackColor = true;
            this.btnKeyGen.Click += new System.EventHandler(this.btnKeyGen_Click);
            // 
            // ShellcodeEncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 468);
            this.Controls.Add(this.btnKeyGen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtkey);
            this.Controls.Add(this.txtIV);
            this.Controls.Add(this.btnEncryptShellCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEncShellcode);
            this.Controls.Add(this.txtShellcode);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ShellcodeEncForm";
            this.Text = "Shellcode Ecryptor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtShellcode;
        private System.Windows.Forms.TextBox txtEncShellcode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEncryptShellCode;
        private System.Windows.Forms.TextBox txtIV;
        private System.Windows.Forms.TextBox txtkey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnKeyGen;
    }
}

