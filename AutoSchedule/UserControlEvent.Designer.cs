
namespace AutoSchedule
{
    partial class UserControlEvent
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
            this.lblEventName = new System.Windows.Forms.Label();
            this.ttEventInfo = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblEventName
            // 
            this.lblEventName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEventName.Location = new System.Drawing.Point(0, 0);
            this.lblEventName.Name = "lblEventName";
            this.lblEventName.Size = new System.Drawing.Size(139, 20);
            this.lblEventName.TabIndex = 0;
            this.lblEventName.Text = "label1";
            this.lblEventName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEventName.Click += new System.EventHandler(this.lblEventName_Click);
            this.lblEventName.MouseHover += new System.EventHandler(this.lblEventName_MouseHover);
            // 
            // ttEventInfo
            // 
            this.ttEventInfo.Tag = "";
            this.ttEventInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttEventInfo.ToolTipTitle = "Event Info";
            // 
            // UserControlEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.lblEventName);
            this.Name = "UserControlEvent";
            this.Size = new System.Drawing.Size(139, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEventName;
        private System.Windows.Forms.ToolTip ttEventInfo;
    }
}
