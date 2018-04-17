namespace PresentationLayer
{
    partial class CapstoneListStaff
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
            this.orderLabel = new System.Windows.Forms.TextBox();
            this.orderValues = new System.Windows.Forms.ComboBox();
            this.selectValues = new System.Windows.Forms.ComboBox();
            this.selectLabel = new System.Windows.Forms.TextBox();
            this.capstoneList = new System.Windows.Forms.ListBox();
            this.statusList = new System.Windows.Forms.ListBox();
            this.userPageReturn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // orderLabel
            // 
            this.orderLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.orderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLabel.Location = new System.Drawing.Point(471, 107);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.ReadOnly = true;
            this.orderLabel.Size = new System.Drawing.Size(142, 22);
            this.orderLabel.TabIndex = 1;
            this.orderLabel.Text = "Order By:";
            this.orderLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // orderValues
            // 
            this.orderValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderValues.FormattingEnabled = true;
            this.orderValues.Items.AddRange(new object[] {
            "Name",
            "Date",
            "Status"});
            this.orderValues.Location = new System.Drawing.Point(471, 135);
            this.orderValues.Name = "orderValues";
            this.orderValues.Size = new System.Drawing.Size(142, 26);
            this.orderValues.TabIndex = 2;
            // 
            // selectValues
            // 
            this.selectValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectValues.FormattingEnabled = true;
            this.selectValues.Items.AddRange(new object[] {
            "Pending",
            "Ongoing",
            "Completed"});
            this.selectValues.Location = new System.Drawing.Point(471, 399);
            this.selectValues.Name = "selectValues";
            this.selectValues.Size = new System.Drawing.Size(142, 26);
            this.selectValues.TabIndex = 4;
            // 
            // selectLabel
            // 
            this.selectLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.selectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectLabel.Location = new System.Drawing.Point(471, 371);
            this.selectLabel.Name = "selectLabel";
            this.selectLabel.ReadOnly = true;
            this.selectLabel.Size = new System.Drawing.Size(142, 22);
            this.selectLabel.TabIndex = 3;
            this.selectLabel.Text = "Select:";
            this.selectLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // capstoneList
            // 
            this.capstoneList.FormattingEnabled = true;
            this.capstoneList.Location = new System.Drawing.Point(13, 41);
            this.capstoneList.Name = "capstoneList";
            this.capstoneList.Size = new System.Drawing.Size(293, 550);
            this.capstoneList.TabIndex = 5;
            // 
            // statusList
            // 
            this.statusList.FormattingEnabled = true;
            this.statusList.Location = new System.Drawing.Point(301, 41);
            this.statusList.Name = "statusList";
            this.statusList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.statusList.Size = new System.Drawing.Size(163, 550);
            this.statusList.TabIndex = 6;
            // 
            // userPageReturn
            // 
            this.userPageReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userPageReturn.Location = new System.Drawing.Point(470, 12);
            this.userPageReturn.Name = "userPageReturn";
            this.userPageReturn.Size = new System.Drawing.Size(143, 44);
            this.userPageReturn.TabIndex = 10;
            this.userPageReturn.Text = "Return to User Page";
            this.userPageReturn.UseVisualStyleBackColor = true;
            this.userPageReturn.Click += new System.EventHandler(this.userPageReturn_Click);
            // 
            // CapstoneListStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.userPageReturn);
            this.Controls.Add(this.statusList);
            this.Controls.Add(this.capstoneList);
            this.Controls.Add(this.selectValues);
            this.Controls.Add(this.selectLabel);
            this.Controls.Add(this.orderValues);
            this.Controls.Add(this.orderLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 640);
            this.MinimumSize = new System.Drawing.Size(640, 640);
            this.Name = "CapstoneListStaff";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CapstoneListStaff";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox orderLabel;
        private System.Windows.Forms.ComboBox orderValues;
        private System.Windows.Forms.ComboBox selectValues;
        private System.Windows.Forms.TextBox selectLabel;
        private System.Windows.Forms.ListBox capstoneList;
        private System.Windows.Forms.ListBox statusList;
        private System.Windows.Forms.Button userPageReturn;
    }
}