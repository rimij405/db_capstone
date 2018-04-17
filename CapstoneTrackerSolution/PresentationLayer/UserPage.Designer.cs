namespace PresentationLayer
{
    partial class UserPage
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
            this.firstName = new System.Windows.Forms.TextBox();
            this.lastName = new System.Windows.Forms.TextBox();
            this.userRole = new System.Windows.Forms.TextBox();
            this.contantInfo = new System.Windows.Forms.TextBox();
            this.emails = new System.Windows.Forms.TextBox();
            this.phones = new System.Windows.Forms.TextBox();
            this.viewUsers = new System.Windows.Forms.Button();
            this.viewCapstones = new System.Windows.Forms.Button();
            this.editProfile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // firstName
            // 
            this.firstName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.firstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstName.Location = new System.Drawing.Point(13, 13);
            this.firstName.Name = "firstName";
            this.firstName.ReadOnly = true;
            this.firstName.Size = new System.Drawing.Size(200, 33);
            this.firstName.TabIndex = 0;
            this.firstName.Text = "fName";
            // 
            // lastName
            // 
            this.lastName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastName.Location = new System.Drawing.Point(219, 13);
            this.lastName.Name = "lastName";
            this.lastName.ReadOnly = true;
            this.lastName.Size = new System.Drawing.Size(230, 33);
            this.lastName.TabIndex = 1;
            this.lastName.Text = "lName";
            // 
            // userRole
            // 
            this.userRole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userRole.Location = new System.Drawing.Point(13, 72);
            this.userRole.Name = "userRole";
            this.userRole.ReadOnly = true;
            this.userRole.Size = new System.Drawing.Size(95, 22);
            this.userRole.TabIndex = 2;
            this.userRole.Text = "User Role";
            // 
            // contantInfo
            // 
            this.contantInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contantInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contantInfo.Location = new System.Drawing.Point(13, 168);
            this.contantInfo.Name = "contantInfo";
            this.contantInfo.ReadOnly = true;
            this.contantInfo.Size = new System.Drawing.Size(380, 31);
            this.contantInfo.TabIndex = 3;
            this.contantInfo.Text = "Contact Information";
            // 
            // emails
            // 
            this.emails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.emails.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emails.Location = new System.Drawing.Point(13, 231);
            this.emails.Multiline = true;
            this.emails.Name = "emails";
            this.emails.ReadOnly = true;
            this.emails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.emails.Size = new System.Drawing.Size(380, 176);
            this.emails.TabIndex = 4;
            this.emails.Text = "Emails go here";
            // 
            // phones
            // 
            this.phones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.phones.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phones.Location = new System.Drawing.Point(13, 410);
            this.phones.Multiline = true;
            this.phones.Name = "phones";
            this.phones.ReadOnly = true;
            this.phones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.phones.Size = new System.Drawing.Size(380, 176);
            this.phones.TabIndex = 5;
            this.phones.Text = "Phone numbers go here";
            // 
            // viewUsers
            // 
            this.viewUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.viewUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewUsers.Location = new System.Drawing.Point(454, 168);
            this.viewUsers.Name = "viewUsers";
            this.viewUsers.Size = new System.Drawing.Size(134, 46);
            this.viewUsers.TabIndex = 6;
            this.viewUsers.Text = "View Users";
            this.viewUsers.UseVisualStyleBackColor = true;
            this.viewUsers.Visible = false;
            this.viewUsers.Click += new System.EventHandler(this.viewUsers_Click);
            // 
            // viewCapstones
            // 
            this.viewCapstones.Cursor = System.Windows.Forms.Cursors.Hand;
            this.viewCapstones.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewCapstones.Location = new System.Drawing.Point(454, 275);
            this.viewCapstones.Name = "viewCapstones";
            this.viewCapstones.Size = new System.Drawing.Size(134, 46);
            this.viewCapstones.TabIndex = 7;
            this.viewCapstones.Text = "View Capstone";
            this.viewCapstones.UseVisualStyleBackColor = true;
            this.viewCapstones.Click += new System.EventHandler(this.viewCapstones_Click);
            // 
            // editProfile
            // 
            this.editProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editProfile.Location = new System.Drawing.Point(454, 525);
            this.editProfile.Name = "editProfile";
            this.editProfile.Size = new System.Drawing.Size(134, 46);
            this.editProfile.TabIndex = 8;
            this.editProfile.Text = "Edit Profile";
            this.editProfile.UseVisualStyleBackColor = true;
            this.editProfile.Click += new System.EventHandler(this.editProfile_Click);
            // 
            // UserPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.editProfile);
            this.Controls.Add(this.viewCapstones);
            this.Controls.Add(this.viewUsers);
            this.Controls.Add(this.phones);
            this.Controls.Add(this.emails);
            this.Controls.Add(this.contantInfo);
            this.Controls.Add(this.userRole);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.firstName);
            this.Name = "UserPage";
            this.Text = "UserPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstName;
        private System.Windows.Forms.TextBox lastName;
        private System.Windows.Forms.TextBox userRole;
        private System.Windows.Forms.TextBox contantInfo;
        private System.Windows.Forms.TextBox emails;
        private System.Windows.Forms.TextBox phones;
        private System.Windows.Forms.Button viewUsers;
        private System.Windows.Forms.Button viewCapstones;
        private System.Windows.Forms.Button editProfile;
    }
}