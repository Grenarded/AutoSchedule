
namespace AutoSchedule
{
    partial class EventEditForm
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblSelectEvent = new System.Windows.Forms.Label();
            this.cbMoreEvents = new System.Windows.Forms.ComboBox();
            this.gbEventSelection = new System.Windows.Forms.GroupBox();
            this.gbEventSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(89, 72);
            // 
            // lblEvent
            // 
            this.lblEvent.Location = new System.Drawing.Point(131, 140);
            // 
            // txtEvent
            // 
            this.txtEvent.Location = new System.Drawing.Point(131, 156);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(181, 203);
            // 
            // timePickerStart
            // 
            this.timePickerStart.Location = new System.Drawing.Point(238, 88);
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(90, 88);
            this.datePicker.Value = new System.DateTime(2023, 6, 5, 16, 0, 50, 467);
            // 
            // lblTimeStart
            // 
            this.lblTimeStart.Location = new System.Drawing.Point(238, 72);
            // 
            // lblTimeEnd
            // 
            this.lblTimeEnd.Location = new System.Drawing.Point(317, 72);
            // 
            // timePickerEnd
            // 
            this.timePickerEnd.Location = new System.Drawing.Point(317, 88);
            // 
            // lblEndTimeError
            // 
            this.lblEndTimeError.Location = new System.Drawing.Point(206, 114);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Red;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(194, 232);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(49, 21);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblSelectEvent
            // 
            this.lblSelectEvent.AutoSize = true;
            this.lblSelectEvent.Location = new System.Drawing.Point(24, 14);
            this.lblSelectEvent.Name = "lblSelectEvent";
            this.lblSelectEvent.Size = new System.Drawing.Size(71, 13);
            this.lblSelectEvent.TabIndex = 15;
            this.lblSelectEvent.Text = "Select Event:";
            // 
            // cbMoreEvents
            // 
            this.cbMoreEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMoreEvents.FormattingEnabled = true;
            this.cbMoreEvents.Location = new System.Drawing.Point(101, 11);
            this.cbMoreEvents.Name = "cbMoreEvents";
            this.cbMoreEvents.Size = new System.Drawing.Size(121, 21);
            this.cbMoreEvents.TabIndex = 14;
            this.cbMoreEvents.SelectedIndexChanged += new System.EventHandler(this.cbMoreEvents_SelectedIndexChanged);
            // 
            // gbEventSelection
            // 
            this.gbEventSelection.Controls.Add(this.lblSelectEvent);
            this.gbEventSelection.Controls.Add(this.cbMoreEvents);
            this.gbEventSelection.Location = new System.Drawing.Point(108, 3);
            this.gbEventSelection.Name = "gbEventSelection";
            this.gbEventSelection.Size = new System.Drawing.Size(250, 54);
            this.gbEventSelection.TabIndex = 16;
            this.gbEventSelection.TabStop = false;
            this.gbEventSelection.Visible = false;
            // 
            // EventEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(475, 318);
            this.Controls.Add(this.gbEventSelection);
            this.Controls.Add(this.btnDelete);
            this.Name = "EventEditForm";
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.lblDate, 0);
            this.Controls.SetChildIndex(this.lblEvent, 0);
            this.Controls.SetChildIndex(this.lblTimeStart, 0);
            this.Controls.SetChildIndex(this.lblTimeEnd, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.lblEndTimeError, 0);
            this.Controls.SetChildIndex(this.txtEvent, 0);
            this.Controls.SetChildIndex(this.timePickerStart, 0);
            this.Controls.SetChildIndex(this.datePicker, 0);
            this.Controls.SetChildIndex(this.timePickerEnd, 0);
            this.Controls.SetChildIndex(this.gbEventSelection, 0);
            this.gbEventSelection.ResumeLayout(false);
            this.gbEventSelection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblSelectEvent;
        private System.Windows.Forms.ComboBox cbMoreEvents;
        private System.Windows.Forms.GroupBox gbEventSelection;
    }
}
