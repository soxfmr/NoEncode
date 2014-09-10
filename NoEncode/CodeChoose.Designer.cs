namespace NoEncode
{
    partial class CodeChoose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeChoose));
            this.label1 = new System.Windows.Forms.Label();
            this.commBoxEncoding = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sources Encoding";
            // 
            // commBoxEncoding
            // 
            this.commBoxEncoding.FormattingEnabled = true;
            this.commBoxEncoding.Items.AddRange(new object[] {
            "Shift_JIS",
            "UTF-8",
            "Unicode"});
            this.commBoxEncoding.Location = new System.Drawing.Point(25, 56);
            this.commBoxEncoding.Name = "commBoxEncoding";
            this.commBoxEncoding.Size = new System.Drawing.Size(219, 20);
            this.commBoxEncoding.TabIndex = 1;
            this.commBoxEncoding.Text = "Shift_JIS";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(76, 104);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(118, 34);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // CodeChoose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 151);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.commBoxEncoding);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CodeChoose";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CodeChoose";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox commBoxEncoding;
        private System.Windows.Forms.Button btnConfirm;
    }
}