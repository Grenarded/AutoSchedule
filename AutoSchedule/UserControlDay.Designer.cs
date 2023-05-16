
namespace AutoSchedule
{
    partial class UserControlDay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDayNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDayNum
            // 
            this.lblDayNum.AutoSize = true;
            this.lblDayNum.Location = new System.Drawing.Point(4, 4);
            this.lblDayNum.Name = "lblDayNum";
            this.lblDayNum.Size = new System.Drawing.Size(19, 13);
            this.lblDayNum.TabIndex = 0;
            this.lblDayNum.Text = "00";
            // 
            // UserControlDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblDayNum);
            this.Name = "UserControlDay";
            this.Size = new System.Drawing.Size(145, 120);
            this.Load += new System.EventHandler(this.UserControlDay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDayNum;
    }
}
