namespace PresentationLayer
{
    partial class CapstonePageView
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
            this.titleLabel = new System.Windows.Forms.TextBox();
            this.abstractLabel = new System.Windows.Forms.TextBox();
            this.facultyLabel = new System.Windows.Forms.TextBox();
            this.defenseDateLabel = new System.Windows.Forms.TextBox();
            this.titleValue = new System.Windows.Forms.TextBox();
            this.abstractValue = new System.Windows.Forms.TextBox();
            this.defenseDateValue = new System.Windows.Forms.TextBox();
            this.facultyValue = new System.Windows.Forms.ListBox();
            this.statusLabel = new System.Windows.Forms.TextBox();
            this.statusValue = new System.Windows.Forms.TextBox();
            this.edit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(13, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.ReadOnly = true;
            this.titleLabel.Size = new System.Drawing.Size(134, 17);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title:";
            this.titleLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // abstractLabel
            // 
            this.abstractLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.abstractLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abstractLabel.Location = new System.Drawing.Point(13, 100);
            this.abstractLabel.Name = "abstractLabel";
            this.abstractLabel.ReadOnly = true;
            this.abstractLabel.Size = new System.Drawing.Size(134, 17);
            this.abstractLabel.TabIndex = 1;
            this.abstractLabel.Text = "Abstract:";
            this.abstractLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // facultyLabel
            // 
            this.facultyLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.facultyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facultyLabel.Location = new System.Drawing.Point(12, 351);
            this.facultyLabel.Name = "facultyLabel";
            this.facultyLabel.ReadOnly = true;
            this.facultyLabel.Size = new System.Drawing.Size(134, 17);
            this.facultyLabel.TabIndex = 2;
            this.facultyLabel.Text = "Faculty:";
            this.facultyLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // defenseDateLabel
            // 
            this.defenseDateLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.defenseDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defenseDateLabel.Location = new System.Drawing.Point(12, 536);
            this.defenseDateLabel.Name = "defenseDateLabel";
            this.defenseDateLabel.ReadOnly = true;
            this.defenseDateLabel.Size = new System.Drawing.Size(134, 17);
            this.defenseDateLabel.TabIndex = 3;
            this.defenseDateLabel.Text = "Defense Date:";
            this.defenseDateLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // titleValue
            // 
            this.titleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleValue.Location = new System.Drawing.Point(154, 13);
            this.titleValue.Name = "titleValue";
            this.titleValue.ReadOnly = true;
            this.titleValue.Size = new System.Drawing.Size(289, 24);
            this.titleValue.TabIndex = 4;
            // 
            // abstractValue
            // 
            this.abstractValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abstractValue.Location = new System.Drawing.Point(154, 101);
            this.abstractValue.Multiline = true;
            this.abstractValue.Name = "abstractValue";
            this.abstractValue.ReadOnly = true;
            this.abstractValue.Size = new System.Drawing.Size(289, 191);
            this.abstractValue.TabIndex = 5;
            // 
            // defenseDateValue
            // 
            this.defenseDateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defenseDateValue.Location = new System.Drawing.Point(154, 537);
            this.defenseDateValue.Name = "defenseDateValue";
            this.defenseDateValue.ReadOnly = true;
            this.defenseDateValue.Size = new System.Drawing.Size(289, 24);
            this.defenseDateValue.TabIndex = 6;
            // 
            // facultyValue
            // 
            this.facultyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facultyValue.FormattingEnabled = true;
            this.facultyValue.ItemHeight = 18;
            this.facultyValue.Items.AddRange(new object[] {
            "Faculty List"});
            this.facultyValue.Location = new System.Drawing.Point(154, 351);
            this.facultyValue.Name = "facultyValue";
            this.facultyValue.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.facultyValue.Size = new System.Drawing.Size(289, 130);
            this.facultyValue.TabIndex = 7;
            // 
            // statusLabel
            // 
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(478, 54);
            this.statusLabel.Multiline = true;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.ReadOnly = true;
            this.statusLabel.Size = new System.Drawing.Size(134, 41);
            this.statusLabel.TabIndex = 8;
            this.statusLabel.Text = "Project\r\nStatus:";
            this.statusLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusValue
            // 
            this.statusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusValue.Location = new System.Drawing.Point(478, 101);
            this.statusValue.Multiline = true;
            this.statusValue.Name = "statusValue";
            this.statusValue.ReadOnly = true;
            this.statusValue.Size = new System.Drawing.Size(134, 380);
            this.statusValue.TabIndex = 9;
            // 
            // edit
            // 
            this.edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edit.Location = new System.Drawing.Point(478, 536);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(134, 53);
            this.edit.TabIndex = 10;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            // 
            // CapstonePageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.statusValue);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.facultyValue);
            this.Controls.Add(this.defenseDateValue);
            this.Controls.Add(this.abstractValue);
            this.Controls.Add(this.titleValue);
            this.Controls.Add(this.defenseDateLabel);
            this.Controls.Add(this.facultyLabel);
            this.Controls.Add(this.abstractLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "CapstonePageView";
            this.Text = "CapstonePageView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox titleLabel;
        private System.Windows.Forms.TextBox abstractLabel;
        private System.Windows.Forms.TextBox facultyLabel;
        private System.Windows.Forms.TextBox defenseDateLabel;
        private System.Windows.Forms.TextBox titleValue;
        private System.Windows.Forms.TextBox abstractValue;
        private System.Windows.Forms.TextBox defenseDateValue;
        private System.Windows.Forms.ListBox facultyValue;
        private System.Windows.Forms.TextBox statusLabel;
        private System.Windows.Forms.TextBox statusValue;
        private System.Windows.Forms.Button edit;
    }
}