﻿namespace PresentationLayer
{
    partial class CapstonePageEdit
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
            this.statusLabel = new System.Windows.Forms.TextBox();
            this.facultyValue = new System.Windows.Forms.ListBox();
            this.abstractValue = new System.Windows.Forms.TextBox();
            this.titleValue = new System.Windows.Forms.TextBox();
            this.defenseDateLabel = new System.Windows.Forms.TextBox();
            this.facultyLabel = new System.Windows.Forms.TextBox();
            this.abstractLabel = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.TextBox();
            this.facultyRequest = new System.Windows.Forms.TextBox();
            this.facultyRequestConfirm = new System.Windows.Forms.Button();
            this.defenseDateValue = new System.Windows.Forms.DateTimePicker();
            this.feedbackText = new System.Windows.Forms.TextBox();
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.deleteFaculty = new System.Windows.Forms.Button();
            this.gradeValue = new System.Windows.Forms.TextBox();
            this.gradeLabel = new System.Windows.Forms.TextBox();
            this.plagarismScoreValue = new System.Windows.Forms.TextBox();
            this.plagarismScoreLabel = new System.Windows.Forms.TextBox();
            this.statuses = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(477, 53);
            this.statusLabel.Multiline = true;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.ReadOnly = true;
            this.statusLabel.Size = new System.Drawing.Size(134, 41);
            this.statusLabel.TabIndex = 18;
            this.statusLabel.Text = "Project\r\nStatus:";
            this.statusLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // facultyValue
            // 
            this.facultyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facultyValue.FormattingEnabled = true;
            this.facultyValue.ItemHeight = 18;
            this.facultyValue.Location = new System.Drawing.Point(153, 350);
            this.facultyValue.Name = "facultyValue";
            this.facultyValue.Size = new System.Drawing.Size(289, 130);
            this.facultyValue.TabIndex = 17;
            // 
            // abstractValue
            // 
            this.abstractValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abstractValue.Location = new System.Drawing.Point(153, 100);
            this.abstractValue.Multiline = true;
            this.abstractValue.Name = "abstractValue";
            this.abstractValue.Size = new System.Drawing.Size(289, 191);
            this.abstractValue.TabIndex = 15;
            // 
            // titleValue
            // 
            this.titleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleValue.Location = new System.Drawing.Point(153, 12);
            this.titleValue.Name = "titleValue";
            this.titleValue.Size = new System.Drawing.Size(289, 24);
            this.titleValue.TabIndex = 14;
            // 
            // defenseDateLabel
            // 
            this.defenseDateLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.defenseDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defenseDateLabel.Location = new System.Drawing.Point(11, 535);
            this.defenseDateLabel.Name = "defenseDateLabel";
            this.defenseDateLabel.ReadOnly = true;
            this.defenseDateLabel.Size = new System.Drawing.Size(134, 17);
            this.defenseDateLabel.TabIndex = 13;
            this.defenseDateLabel.Text = "Defense Date:";
            this.defenseDateLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // facultyLabel
            // 
            this.facultyLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.facultyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facultyLabel.Location = new System.Drawing.Point(11, 350);
            this.facultyLabel.Name = "facultyLabel";
            this.facultyLabel.ReadOnly = true;
            this.facultyLabel.Size = new System.Drawing.Size(134, 17);
            this.facultyLabel.TabIndex = 12;
            this.facultyLabel.Text = "Faculty:";
            this.facultyLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // abstractLabel
            // 
            this.abstractLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.abstractLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abstractLabel.Location = new System.Drawing.Point(12, 99);
            this.abstractLabel.Name = "abstractLabel";
            this.abstractLabel.ReadOnly = true;
            this.abstractLabel.Size = new System.Drawing.Size(134, 17);
            this.abstractLabel.TabIndex = 11;
            this.abstractLabel.Text = "Abstract:";
            this.abstractLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // titleLabel
            // 
            this.titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(12, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.ReadOnly = true;
            this.titleLabel.Size = new System.Drawing.Size(134, 17);
            this.titleLabel.TabIndex = 10;
            this.titleLabel.Text = "Title:";
            this.titleLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // facultyRequest
            // 
            this.facultyRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facultyRequest.Location = new System.Drawing.Point(153, 486);
            this.facultyRequest.Name = "facultyRequest";
            this.facultyRequest.Size = new System.Drawing.Size(202, 24);
            this.facultyRequest.TabIndex = 20;
            this.facultyRequest.Text = "Add Faculty Here";
            // 
            // facultyRequestConfirm
            // 
            this.facultyRequestConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facultyRequestConfirm.Location = new System.Drawing.Point(362, 486);
            this.facultyRequestConfirm.Name = "facultyRequestConfirm";
            this.facultyRequestConfirm.Size = new System.Drawing.Size(80, 44);
            this.facultyRequestConfirm.TabIndex = 21;
            this.facultyRequestConfirm.Text = "Request\r\nFaculty\r\n";
            this.facultyRequestConfirm.UseVisualStyleBackColor = true;
            this.facultyRequestConfirm.Click += new System.EventHandler(this.facultyRequestConfirm_Click);
            // 
            // defenseDateValue
            // 
            this.defenseDateValue.Location = new System.Drawing.Point(151, 535);
            this.defenseDateValue.Name = "defenseDateValue";
            this.defenseDateValue.Size = new System.Drawing.Size(291, 20);
            this.defenseDateValue.TabIndex = 22;
            // 
            // feedbackText
            // 
            this.feedbackText.BackColor = System.Drawing.Color.DarkSalmon;
            this.feedbackText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.feedbackText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.feedbackText.Location = new System.Drawing.Point(11, 572);
            this.feedbackText.Name = "feedbackText";
            this.feedbackText.ReadOnly = true;
            this.feedbackText.Size = new System.Drawing.Size(431, 24);
            this.feedbackText.TabIndex = 23;
            this.feedbackText.Text = "Program feedback goes here";
            this.feedbackText.Visible = false;
            // 
            // save
            // 
            this.save.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.save.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save.Location = new System.Drawing.Point(461, 553);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 42);
            this.save.TabIndex = 24;
            this.save.Text = "Save changes";
            this.save.UseVisualStyleBackColor = false;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // cancel
            // 
            this.cancel.BackColor = System.Drawing.Color.DarkSalmon;
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(542, 553);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 42);
            this.cancel.TabIndex = 25;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = false;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // deleteFaculty
            // 
            this.deleteFaculty.Location = new System.Drawing.Point(72, 432);
            this.deleteFaculty.Name = "deleteFaculty";
            this.deleteFaculty.Size = new System.Drawing.Size(75, 48);
            this.deleteFaculty.TabIndex = 27;
            this.deleteFaculty.Text = "Delete Selected Faculty";
            this.deleteFaculty.UseVisualStyleBackColor = true;
            this.deleteFaculty.Click += new System.EventHandler(this.deleteFaculty_Click);
            // 
            // gradeValue
            // 
            this.gradeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradeValue.Location = new System.Drawing.Point(477, 382);
            this.gradeValue.Name = "gradeValue";
            this.gradeValue.ReadOnly = true;
            this.gradeValue.Size = new System.Drawing.Size(134, 24);
            this.gradeValue.TabIndex = 29;
            // 
            // gradeLabel
            // 
            this.gradeLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradeLabel.Location = new System.Drawing.Point(478, 356);
            this.gradeLabel.Multiline = true;
            this.gradeLabel.Name = "gradeLabel";
            this.gradeLabel.ReadOnly = true;
            this.gradeLabel.Size = new System.Drawing.Size(134, 20);
            this.gradeLabel.TabIndex = 28;
            this.gradeLabel.Text = "Grade:";
            this.gradeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // plagarismScoreValue
            // 
            this.plagarismScoreValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plagarismScoreValue.Location = new System.Drawing.Point(478, 456);
            this.plagarismScoreValue.Name = "plagarismScoreValue";
            this.plagarismScoreValue.ReadOnly = true;
            this.plagarismScoreValue.Size = new System.Drawing.Size(134, 24);
            this.plagarismScoreValue.TabIndex = 31;
            // 
            // plagarismScoreLabel
            // 
            this.plagarismScoreLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.plagarismScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plagarismScoreLabel.Location = new System.Drawing.Point(479, 412);
            this.plagarismScoreLabel.Multiline = true;
            this.plagarismScoreLabel.Name = "plagarismScoreLabel";
            this.plagarismScoreLabel.ReadOnly = true;
            this.plagarismScoreLabel.Size = new System.Drawing.Size(134, 38);
            this.plagarismScoreLabel.TabIndex = 30;
            this.plagarismScoreLabel.Text = "Plagarism \r\nScore:";
            this.plagarismScoreLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statuses
            // 
            this.statuses.FormattingEnabled = true;
            this.statuses.Location = new System.Drawing.Point(477, 100);
            this.statuses.Name = "statuses";
            this.statuses.Size = new System.Drawing.Size(134, 21);
            this.statuses.TabIndex = 32;
            this.statuses.Visible = false;
            // 
            // CapstonePageEdit
            // 
            this.AcceptButton = this.facultyRequestConfirm;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.statuses);
            this.Controls.Add(this.plagarismScoreValue);
            this.Controls.Add(this.plagarismScoreLabel);
            this.Controls.Add(this.gradeValue);
            this.Controls.Add(this.gradeLabel);
            this.Controls.Add(this.deleteFaculty);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.Controls.Add(this.feedbackText);
            this.Controls.Add(this.defenseDateValue);
            this.Controls.Add(this.facultyRequestConfirm);
            this.Controls.Add(this.facultyRequest);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.facultyValue);
            this.Controls.Add(this.abstractValue);
            this.Controls.Add(this.titleValue);
            this.Controls.Add(this.defenseDateLabel);
            this.Controls.Add(this.facultyLabel);
            this.Controls.Add(this.abstractLabel);
            this.Controls.Add(this.titleLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 640);
            this.MinimumSize = new System.Drawing.Size(640, 640);
            this.Name = "CapstonePageEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CapstonePageEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox statusLabel;
        private System.Windows.Forms.ListBox facultyValue;
        private System.Windows.Forms.TextBox abstractValue;
        private System.Windows.Forms.TextBox titleValue;
        private System.Windows.Forms.TextBox defenseDateLabel;
        private System.Windows.Forms.TextBox facultyLabel;
        private System.Windows.Forms.TextBox abstractLabel;
        private System.Windows.Forms.TextBox titleLabel;
        private System.Windows.Forms.TextBox facultyRequest;
        private System.Windows.Forms.Button facultyRequestConfirm;
        private System.Windows.Forms.DateTimePicker defenseDateValue;
        private System.Windows.Forms.TextBox feedbackText;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button deleteFaculty;
        private System.Windows.Forms.TextBox gradeValue;
        private System.Windows.Forms.TextBox gradeLabel;
        private System.Windows.Forms.TextBox plagarismScoreValue;
        private System.Windows.Forms.TextBox plagarismScoreLabel;
        private System.Windows.Forms.ComboBox statuses;
    }
}