namespace _94625_Astroidinator
{
    partial class fmLogging
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
            this.rtbSerialMonitor = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbxLineNumbering = new System.Windows.Forms.CheckBox();
            this.cbxAutoScroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rtbSerialMonitor
            // 
            this.rtbSerialMonitor.BackColor = System.Drawing.Color.Black;
            this.rtbSerialMonitor.ForeColor = System.Drawing.Color.White;
            this.rtbSerialMonitor.Location = new System.Drawing.Point(16, 15);
            this.rtbSerialMonitor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbSerialMonitor.Name = "rtbSerialMonitor";
            this.rtbSerialMonitor.Size = new System.Drawing.Size(693, 255);
            this.rtbSerialMonitor.TabIndex = 0;
            this.rtbSerialMonitor.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(611, 278);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cbxLineNumbering
            // 
            this.cbxLineNumbering.AutoSize = true;
            this.cbxLineNumbering.Checked = true;
            this.cbxLineNumbering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxLineNumbering.Location = new System.Drawing.Point(112, 283);
            this.cbxLineNumbering.Name = "cbxLineNumbering";
            this.cbxLineNumbering.Size = new System.Drawing.Size(116, 20);
            this.cbxLineNumbering.TabIndex = 5;
            this.cbxLineNumbering.Text = "line numbering";
            this.cbxLineNumbering.UseVisualStyleBackColor = true;
            // 
            // cbxAutoScroll
            // 
            this.cbxAutoScroll.AutoSize = true;
            this.cbxAutoScroll.Checked = true;
            this.cbxAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxAutoScroll.Location = new System.Drawing.Point(16, 283);
            this.cbxAutoScroll.Name = "cbxAutoScroll";
            this.cbxAutoScroll.Size = new System.Drawing.Size(90, 20);
            this.cbxAutoScroll.TabIndex = 4;
            this.cbxAutoScroll.Text = "auto scroll";
            this.cbxAutoScroll.UseVisualStyleBackColor = true;
            // 
            // fmLogging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 315);
            this.Controls.Add(this.cbxLineNumbering);
            this.Controls.Add(this.cbxAutoScroll);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rtbSerialMonitor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "fmLogging";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbSerialMonitor;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox cbxLineNumbering;
        private System.Windows.Forms.CheckBox cbxAutoScroll;
    }
}