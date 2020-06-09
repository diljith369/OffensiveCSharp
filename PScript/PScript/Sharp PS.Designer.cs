namespace PScript
{
    partial class Sharp_PS
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
            this.dontclick = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dontclick
            // 
            this.dontclick.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dontclick.Location = new System.Drawing.Point(11, 12);
            this.dontclick.Name = "dontclick";
            this.dontclick.Size = new System.Drawing.Size(190, 99);
            this.dontclick.TabIndex = 0;
            this.dontclick.Text = "Don\'t Click";
            this.dontclick.UseVisualStyleBackColor = true;
            this.dontclick.Click += new System.EventHandler(this.dontclick_Click);
            // 
            // Sharp_PS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 123);
            this.Controls.Add(this.dontclick);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Sharp_PS";
            this.Text = "Sharp_PS";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button dontclick;
    }
}