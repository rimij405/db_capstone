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
            // 
            // UserList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.userPageReturn);
            this.Name = "UserList";
            this.Text = "UserList";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button userPageReturn;
    }
}