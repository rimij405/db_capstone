namespace PresentationLayer
{
    partial class CapstoneListFaculty
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
            this.pendingLabel = new System.Windows.Forms.TextBox();
            this.currentLabel = new System.Windows.Forms.TextBox();
            this.capstonePendingList = new System.Windows.Forms.ListBox();
            this.capstoneCurrentList = new System.Windows.Forms.ListBox();
            this.acceptCapstone = new System.Windows.Forms.Button();
            this.rejectCapstone = new System.Windows.Forms.Button();
            this.capstoneGradeList = new System.Windows.Forms.ListBox();
            this.userPageReturn = new System.Windows.Forms.Button();
            this.error = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pendingLabel
            // 
            this.pendingLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pendingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pendingLabel.Location = new System.Drawing.Point(13, 26);
            this.pendingLabel.Name = "pendingLabel";
            this.pendingLabel.ReadOnly = true;
            this.pendingLabel.Size = new System.Drawing.Size(600, 22);
            this.pendingLabel.TabIndex = 2;
            this.pendingLabel.Text = "Pending Capstones:";
            // 
            // currentLabel
            // 
            this.currentLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.currentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLabel.Location = new System.Drawing.Point(13, 274);
            this.currentLabel.Name = "currentLabel";
            this.currentLabel.ReadOnly = true;
            this.currentLabel.Size = new System.Drawing.Size(600, 22);
            this.currentLabel.TabIndex = 3;
            this.currentLabel.Text = "Current Capstones:";
            // 
            // capstonePendingList
            // 
            this.capstonePendingList.FormattingEnabled = true;
            this.capstonePendingList.Location = new System.Drawing.Point(13, 55);
            this.capstonePendingList.Name = "capstonePendingList";
            this.capstonePendingList.Size = new System.Drawing.Size(433, 199);
            this.capstonePendingList.TabIndex = 4;
            // 
            // capstoneCurrentList
            // 
            this.capstoneCurrentList.FormattingEnabled = true;
            this.capstoneCurrentList.Location = new System.Drawing.Point(13, 302);
            this.capstoneCurrentList.Name = "capstoneCurrentList";
            this.capstoneCurrentList.Size = new System.Drawing.Size(433, 251);
            this.capstoneCurrentList.TabIndex = 5;
            // 
            // acceptCapstone
            // 
            this.acceptCapstone.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.acceptCapstone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptCapstone.Location = new System.Drawing.Point(468, 126);
            this.acceptCapstone.Name = "acceptCapstone";
            this.acceptCapstone.Size = new System.Drawing.Size(144, 61);
            this.acceptCapstone.TabIndex = 6;
            this.acceptCapstone.Text = "Accept Capstone \r\nInvitation";
            this.acceptCapstone.UseVisualStyleBackColor = false;
            // 
            // rejectCapstone
            // 
            this.rejectCapstone.BackColor = System.Drawing.Color.DarkSalmon;
            this.rejectCapstone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rejectCapstone.Location = new System.Drawing.Point(468, 193);
            this.rejectCapstone.Name = "rejectCapstone";
            this.rejectCapstone.Size = new System.Drawing.Size(144, 61);
            this.rejectCapstone.TabIndex = 7;
            this.rejectCapstone.Text = "Decline Capstone\r\nInvitation\r\n";
            this.rejectCapstone.UseVisualStyleBackColor = false;
            // 
            // capstoneGradeList
            // 
            this.capstoneGradeList.FormattingEnabled = true;
            this.capstoneGradeList.Location = new System.Drawing.Point(442, 302);
            this.capstoneGradeList.Name = "capstoneGradeList";
            this.capstoneGradeList.Size = new System.Drawing.Size(171, 251);
            this.capstoneGradeList.TabIndex = 8;
            // 
            // userPageReturn
            // 
            this.userPageReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userPageReturn.Location = new System.Drawing.Point(469, 13);
            this.userPageReturn.Name = "userPageReturn";
            this.userPageReturn.Size = new System.Drawing.Size(143, 44);
            this.userPageReturn.TabIndex = 9;
            this.userPageReturn.Text = "Return to User Page";
            this.userPageReturn.UseVisualStyleBackColor = true;
            this.userPageReturn.Click += new System.EventHandler(this.userPageReturn_Click);
            // 
            // error
            // 
            this.error.BackColor = System.Drawing.Color.DarkSalmon;
            this.error.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.error.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.Location = new System.Drawing.Point(12, 565);
            this.error.Name = "error";
            this.error.ReadOnly = true;
            this.error.Size = new System.Drawing.Size(600, 24);
            this.error.TabIndex = 15;
            this.error.Text = "Error Goes Here";
            this.error.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.error.Visible = false;
            // 
            // CapstoneListFaculty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.error);
            this.Controls.Add(this.userPageReturn);
            this.Controls.Add(this.capstoneGradeList);
            this.Controls.Add(this.rejectCapstone);
            this.Controls.Add(this.acceptCapstone);
            this.Controls.Add(this.capstoneCurrentList);
            this.Controls.Add(this.capstonePendingList);
            this.Controls.Add(this.currentLabel);
            this.Controls.Add(this.pendingLabel);
            this.Name = "CapstoneListFaculty";
            this.Text = "CapstoneListFaculty";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox pendingLabel;
        private System.Windows.Forms.TextBox currentLabel;
        private System.Windows.Forms.ListBox capstonePendingList;
        private System.Windows.Forms.ListBox capstoneCurrentList;
        private System.Windows.Forms.Button acceptCapstone;
        private System.Windows.Forms.Button rejectCapstone;
        private System.Windows.Forms.ListBox capstoneGradeList;
        private System.Windows.Forms.Button userPageReturn;
        private System.Windows.Forms.TextBox error;
    }
}