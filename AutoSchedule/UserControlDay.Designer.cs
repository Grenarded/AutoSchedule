
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
            this.components = new System.ComponentModel.Container();
            this.lblDayNum = new System.Windows.Forms.Label();
            this.flpEvents = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMaxEvents = new System.Windows.Forms.Label();
            this.ttMoreEvents = new System.Windows.Forms.ToolTip(this.components);
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
            // flpEvents
            // 
            this.flpEvents.Location = new System.Drawing.Point(0, 20);
            this.flpEvents.Name = "flpEvents";
            this.flpEvents.Size = new System.Drawing.Size(145, 100);
            this.flpEvents.TabIndex = 1;
            this.flpEvents.Click += new System.EventHandler(this.flpEvents_Click);
            // 
            // lblMaxEvents
            // 
            this.lblMaxEvents.BackColor = System.Drawing.Color.Salmon;
            this.lblMaxEvents.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMaxEvents.Location = new System.Drawing.Point(0, 100);
            this.lblMaxEvents.Name = "lblMaxEvents";
            this.lblMaxEvents.Size = new System.Drawing.Size(145, 20);
            this.lblMaxEvents.TabIndex = 2;
            this.lblMaxEvents.Text = "+ More Events";
            this.lblMaxEvents.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMaxEvents.Visible = false;
            this.lblMaxEvents.Click += new System.EventHandler(this.lblMaxEvents_Click);
            this.lblMaxEvents.MouseLeave += new System.EventHandler(this.lblMaxEvents_MouseLeave);
            this.lblMaxEvents.MouseHover += new System.EventHandler(this.lblMaxEvents_MouseHover);
            // 
            // ttMoreEvents
            // 
            this.ttMoreEvents.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttMoreEvents.ToolTipTitle = "More Events";
            // 
            // UserControlDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblMaxEvents);
            this.Controls.Add(this.flpEvents);
            this.Controls.Add(this.lblDayNum);
            this.Name = "UserControlDay";
            this.Size = new System.Drawing.Size(145, 120);
            this.Load += new System.EventHandler(this.UserControlDay_Load);
            this.Click += new System.EventHandler(this.UserControlDay_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblDayNum;
        protected System.Windows.Forms.FlowLayoutPanel flpEvents;
        protected System.Windows.Forms.Label lblMaxEvents;
        protected System.Windows.Forms.ToolTip ttMoreEvents;
    }
}
