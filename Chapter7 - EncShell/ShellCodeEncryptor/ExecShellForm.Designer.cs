namespace ShellCodeEncryptor
{
    partial class ExecShellForm
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
            this.btnExec = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExec
            // 
            this.btnExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExec.Location = new System.Drawing.Point(12, 12);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(255, 161);
            this.btnExec.TabIndex = 0;
            this.btnExec.Text = "Don\'t Click";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // ExecShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 186);
            this.Controls.Add(this.btnExec);
            this.Name = "ExecShellForm";
            this.Text = "Encrypted Shell";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExec;
    }
}

