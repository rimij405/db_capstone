namespace PresentationLayer
{
    partial class UserPageEdit
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
            this.contantInfo = new System.Windows.Forms.TextBox();
            this.userRole = new System.Windows.Forms.TextBox();
            this.lastName = new System.Windows.Forms.TextBox();
            this.firstName = new System.Windows.Forms.TextBox();
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.error = new System.Windows.Forms.TextBox();
            this.emails = new System.Windows.Forms.ListBox();
            this.phones = new System.Windows.Forms.ListBox();
            this.emailTypes = new System.Windows.Forms.ComboBox();
            this.phoneTypes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // contantInfo
            // 
            this.contantInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contantInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contantInfo.Location = new System.Drawing.Point(12, 167);
            this.contantInfo.Name = "contantInfo";
            this.contantInfo.ReadOnly = true;
            this.contantInfo.Size = new System.Drawing.Size(380, 31);
            this.contantInfo.TabIndex = 7;
            this.contantInfo.Text = "Contact Information";
            // 
            // userRole
            // 
            this.userRole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userRole.Location = new System.Drawing.Point(12, 71);
            this.userRole.Name = "userRole";
            this.userRole.ReadOnly = true;
            this.userRole.Size = new System.Drawing.Size(95, 22);
            this.userRole.TabIndex = 6;
            this.userRole.Text = "User Role";
            // 
            // lastName
            // 
            this.lastName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastName.Location = new System.Drawing.Point(218, 12);
            this.lastName.Name = "lastName";
            this.lastName.ReadOnly = true;
            this.lastName.Size = new System.Drawing.Size(230, 33);
            this.lastName.TabIndex = 5;
            this.lastName.Text = "lName";
            // 
            // firstName
            // 
            this.firstName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.firstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstName.Location = new System.Drawing.Point(12, 12);
            this.firstName.Name = "firstName";
            this.firstName.ReadOnly = true;
            this.firstName.Size = new System.Drawing.Size(200, 33);
            this.firstName.TabIndex = 4;
            this.firstName.Text = "fName";
            // 
            // save
            // 
            this.save.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.save.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save.Location = new System.Drawing.Point(417, 71);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(134, 46);
            this.save.TabIndex = 12;
            this.save.Text = "Save Changes";
            this.save.UseVisualStyleBackColor = false;
            // 
            // cancel
            // 
            this.cancel.BackColor = System.Drawing.Color.DarkSalmon;
            this.cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(417, 152);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(134, 46);
            this.cancel.TabIndex = 13;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = false;
            // 
            // error
            // 
            this.error.BackColor = System.Drawing.Color.DarkSalmon;
            this.error.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.error.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.Location = new System.Drawing.Point(12, 577);
            this.error.Name = "error";
            this.error.ReadOnly = true;
            this.error.Size = new System.Drawing.Size(600, 24);
            this.error.TabIndex = 14;
            this.error.Text = "Error Goes Here";
            this.error.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.error.Visible = false;
            // 
            // emails
            // 
            this.emails.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emails.FormattingEnabled = true;
            this.emails.ItemHeight = 18;
            this.emails.Items.AddRange(new object[] {
            "emails",
            "go",
            "here"});
            this.emails.Location = new System.Drawing.Point(13, 223);
            this.emails.Name = "emails";
            this.emails.Size = new System.Drawing.Size(379, 166);
            this.emails.TabIndex = 15;
            // 
            // phones
            // 
            this.phones.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phones.FormattingEnabled = true;
            this.phones.ItemHeight = 18;
            this.phones.Items.AddRange(new object[] {
            "phones",
            "go",
            "here"});
            this.phones.Location = new System.Drawing.Point(13, 398);
            this.phones.Name = "phones";
            this.phones.Size = new System.Drawing.Size(379, 166);
            this.phones.TabIndex = 16;
            // 
            // emailTypes
            // 
            this.emailTypes.FormattingEnabled = true;
            this.emailTypes.Items.AddRange(new object[] {
            "",
            "Primary",
            "School",
            "Work",
            "Personal"});
            this.emailTypes.Location = new System.Drawing.Point(398, 223);
            this.emailTypes.Name = "emailTypes";
            this.emailTypes.Size = new System.Drawing.Size(195, 21);
            this.emailTypes.TabIndex = 17;
            // 
            // phoneTypes
            // 
            this.phoneTypes.FormattingEnabled = true;
            this.phoneTypes.Items.AddRange(new object[] {
            "",
            "Primary",
            "Home",
            "Mobile",
            "Work"});
            this.phoneTypes.Location = new System.Drawing.Point(398, 398);
            this.phoneTypes.Name = "phoneTypes";
            this.phoneTypes.Size = new System.Drawing.Size(195, 21);
            this.phoneTypes.TabIndex = 18;
            // 
            // UserPageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.phoneTypes);
            this.Controls.Add(this.emailTypes);
            this.Controls.Add(this.phones);
            this.Controls.Add(this.emails);
            this.Controls.Add(this.error);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.Controls.Add(this.contantInfo);
            this.Controls.Add(this.userRole);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.firstName);
            this.Name = "UserPageEdit";
            this.Text = "UserPageEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox contantInfo;
        private System.Windows.Forms.TextBox userRole;
        private System.Windows.Forms.TextBox lastName;
        private System.Windows.Forms.TextBox firstName;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox error;
        private System.Windows.Forms.ListBox emails;
        private System.Windows.Forms.ListBox phones;
        private System.Windows.Forms.ComboBox emailTypes;
        private System.Windows.Forms.ComboBox phoneTypes;
    }
}