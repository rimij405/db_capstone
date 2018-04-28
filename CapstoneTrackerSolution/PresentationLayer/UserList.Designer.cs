namespace PresentationLayer
{
    partial class UserList
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
            this.userPageReturn = new System.Windows.Forms.Button();
            this.usersList = new System.Windows.Forms.ListBox();
            this.selectValues = new System.Windows.Forms.ComboBox();
            this.selectLabel = new System.Windows.Forms.TextBox();
            this.orderValues = new System.Windows.Forms.ComboBox();
            this.orderLabel = new System.Windows.Forms.TextBox();
            this.firstName = new System.Windows.Forms.TextBox();
            this.roles = new System.Windows.Forms.ComboBox();
            this.lastName = new System.Windows.Forms.TextBox();
            this.addUserButton = new System.Windows.Forms.Button();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.error = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // userPageReturn
            // 
            this.userPageReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userPageReturn.Location = new System.Drawing.Point(469, 12);
            this.userPageReturn.Name = "userPageReturn";
            this.userPageReturn.Size = new System.Drawing.Size(143, 44);
            this.userPageReturn.TabIndex = 11;
            this.userPageReturn.Text = "Return to User Page";
            this.userPageReturn.UseVisualStyleBackColor = true;
            this.userPageReturn.Click += new System.EventHandler(this.userPageReturn_Click);
            // 
            // usersList
            // 
            this.usersList.FormattingEnabled = true;
            this.usersList.Location = new System.Drawing.Point(12, 12);
            this.usersList.Name = "usersList";
            this.usersList.Size = new System.Drawing.Size(451, 498);
            this.usersList.TabIndex = 12;
            // 
            // selectValues
            // 
            this.selectValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectValues.FormattingEnabled = true;
            this.selectValues.Items.AddRange(new object[] {
            "Students",
            "Faculty",
            "Staff"});
            this.selectValues.Location = new System.Drawing.Point(470, 416);
            this.selectValues.Name = "selectValues";
            this.selectValues.Size = new System.Drawing.Size(142, 26);
            this.selectValues.TabIndex = 16;
            // 
            // selectLabel
            // 
            this.selectLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.selectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectLabel.Location = new System.Drawing.Point(470, 388);
            this.selectLabel.Name = "selectLabel";
            this.selectLabel.ReadOnly = true;
            this.selectLabel.Size = new System.Drawing.Size(142, 22);
            this.selectLabel.TabIndex = 15;
            this.selectLabel.Text = "Select:";
            this.selectLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // orderValues
            // 
            this.orderValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderValues.FormattingEnabled = true;
            this.orderValues.Items.AddRange(new object[] {
            "Name",
            "Role"});
            this.orderValues.Location = new System.Drawing.Point(470, 152);
            this.orderValues.Name = "orderValues";
            this.orderValues.Size = new System.Drawing.Size(142, 26);
            this.orderValues.TabIndex = 14;
            // 
            // orderLabel
            // 
            this.orderLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.orderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLabel.Location = new System.Drawing.Point(470, 124);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.ReadOnly = true;
            this.orderLabel.Size = new System.Drawing.Size(142, 22);
            this.orderLabel.TabIndex = 13;
            this.orderLabel.Text = "Order By:";
            this.orderLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // firstName
            // 
            this.firstName.Location = new System.Drawing.Point(75, 516);
            this.firstName.Name = "firstName";
            this.firstName.Size = new System.Drawing.Size(237, 20);
            this.firstName.TabIndex = 17;
            // 
            // roles
            // 
            this.roles.FormattingEnabled = true;
            this.roles.Items.AddRange(new object[] {
            "Student",
            "Faculty",
            "Staff"});
            this.roles.Location = new System.Drawing.Point(318, 517);
            this.roles.Name = "roles";
            this.roles.Size = new System.Drawing.Size(145, 21);
            this.roles.TabIndex = 18;
            // 
            // lastName
            // 
            this.lastName.Location = new System.Drawing.Point(75, 555);
            this.lastName.Name = "lastName";
            this.lastName.Size = new System.Drawing.Size(237, 20);
            this.lastName.TabIndex = 19;
            // 
            // addUserButton
            // 
            this.addUserButton.Location = new System.Drawing.Point(470, 517);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(142, 34);
            this.addUserButton.TabIndex = 20;
            this.addUserButton.Text = "Add New User";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(12, 517);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(57, 13);
            this.firstNameLabel.TabIndex = 21;
            this.firstNameLabel.Text = "First Name";
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(12, 562);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.lastNameLabel.TabIndex = 22;
            this.lastNameLabel.Text = "Last Name";
            // 
            // error
            // 
            this.error.BackColor = System.Drawing.Color.DarkSalmon;
            this.error.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.error.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.Location = new System.Drawing.Point(318, 557);
            this.error.Multiline = true;
            this.error.Name = "error";
            this.error.ReadOnly = true;
            this.error.Size = new System.Drawing.Size(294, 32);
            this.error.TabIndex = 23;
            this.error.Text = "Error Goes Here";
            // 
            // UserList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.error);
            this.Controls.Add(this.lastNameLabel);
            this.Controls.Add(this.firstNameLabel);
            this.Controls.Add(this.addUserButton);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.roles);
            this.Controls.Add(this.firstName);
            this.Controls.Add(this.selectValues);
            this.Controls.Add(this.selectLabel);
            this.Controls.Add(this.orderValues);
            this.Controls.Add(this.orderLabel);
            this.Controls.Add(this.usersList);
            this.Controls.Add(this.userPageReturn);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 640);
            this.MinimumSize = new System.Drawing.Size(640, 640);
            this.Name = "UserList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserList";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button userPageReturn;
        private System.Windows.Forms.ListBox usersList;
        private System.Windows.Forms.ComboBox selectValues;
        private System.Windows.Forms.TextBox selectLabel;
        private System.Windows.Forms.ComboBox orderValues;
        private System.Windows.Forms.TextBox orderLabel;
        private System.Windows.Forms.TextBox firstName;
        private System.Windows.Forms.ComboBox roles;
        private System.Windows.Forms.TextBox lastName;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox error;
    }
}