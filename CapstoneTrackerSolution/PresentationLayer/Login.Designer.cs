namespace PresentationLayer
{
    partial class Login
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
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.usernameTag = new System.Windows.Forms.TextBox();
            this.passwordTag = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.TextBox();
            this.error = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.Location = new System.Drawing.Point(155, 353);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(393, 24);
            this.username.TabIndex = 0;
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(155, 440);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(393, 24);
            this.password.TabIndex = 1;
            // 
            // usernameTag
            // 
            this.usernameTag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernameTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTag.Location = new System.Drawing.Point(63, 353);
            this.usernameTag.Name = "usernameTag";
            this.usernameTag.ReadOnly = true;
            this.usernameTag.Size = new System.Drawing.Size(86, 17);
            this.usernameTag.TabIndex = 2;
            this.usernameTag.Text = "Username:";
            // 
            // passwordTag
            // 
            this.passwordTag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTag.Location = new System.Drawing.Point(63, 440);
            this.passwordTag.Name = "passwordTag";
            this.passwordTag.ReadOnly = true;
            this.passwordTag.Size = new System.Drawing.Size(86, 17);
            this.passwordTag.TabIndex = 3;
            this.passwordTag.Text = "Password:";
            // 
            // title
            // 
            this.title.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(63, 79);
            this.title.Multiline = true;
            this.title.Name = "title";
            this.title.ReadOnly = true;
            this.title.Size = new System.Drawing.Size(485, 141);
            this.title.TabIndex = 4;
            this.title.Text = "IST Graduate\r\nCapstone Tracker\r\n";
            this.title.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // error
            // 
            this.error.BackColor = System.Drawing.Color.DarkSalmon;
            this.error.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.error.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.Location = new System.Drawing.Point(63, 276);
            this.error.Name = "error";
            this.error.ReadOnly = true;
            this.error.Size = new System.Drawing.Size(485, 26);
            this.error.TabIndex = 5;
            this.error.Text = "Invalid login attempt. Please try again.";
            this.error.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.error.Visible = false;
            // 
            // loginButton
            // 
            this.loginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginButton.Location = new System.Drawing.Point(217, 508);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(174, 53);
            this.loginButton.TabIndex = 6;
            this.loginButton.Text = "Log In";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // Login
            // 
            this.AcceptButton = this.loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.error);
            this.Controls.Add(this.title);
            this.Controls.Add(this.passwordTag);
            this.Controls.Add(this.usernameTag);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 640);
            this.MinimumSize = new System.Drawing.Size(640, 640);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox usernameTag;
        private System.Windows.Forms.TextBox passwordTag;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.TextBox error;
        private System.Windows.Forms.Button loginButton;
    }
}