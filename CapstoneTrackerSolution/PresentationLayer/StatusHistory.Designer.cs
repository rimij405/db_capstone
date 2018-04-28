namespace PresentationLayer
{
    partial class StatusHistory
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
            this.statusValues = new System.Windows.Forms.ListBox();
            this.statusDescriptions = new System.Windows.Forms.TextBox();
            this.statusDateValues = new System.Windows.Forms.ListBox();
            this.returnButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // statusValues
            // 
            this.statusValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusValues.FormattingEnabled = true;
            this.statusValues.ItemHeight = 16;
            this.statusValues.Location = new System.Drawing.Point(13, 61);
            this.statusValues.Name = "statusValues";
            this.statusValues.Size = new System.Drawing.Size(404, 404);
            this.statusValues.TabIndex = 0;
            // 
            // statusDescriptions
            // 
            this.statusDescriptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusDescriptions.Location = new System.Drawing.Point(13, 471);
            this.statusDescriptions.Multiline = true;
            this.statusDescriptions.Name = "statusDescriptions";
            this.statusDescriptions.ReadOnly = true;
            this.statusDescriptions.Size = new System.Drawing.Size(599, 118);
            this.statusDescriptions.TabIndex = 2;
            this.statusDescriptions.Text = "Status Description";
            // 
            // statusDateValues
            // 
            this.statusDateValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusDateValues.FormattingEnabled = true;
            this.statusDateValues.ItemHeight = 16;
            this.statusDateValues.Location = new System.Drawing.Point(417, 61);
            this.statusDateValues.Name = "statusDateValues";
            this.statusDateValues.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.statusDateValues.Size = new System.Drawing.Size(195, 404);
            this.statusDateValues.TabIndex = 3;
            // 
            // returnButton
            // 
            this.returnButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returnButton.Location = new System.Drawing.Point(417, 12);
            this.returnButton.Name = "returnButton";
            this.returnButton.Size = new System.Drawing.Size(195, 43);
            this.returnButton.TabIndex = 4;
            this.returnButton.Text = "Back to Capstone";
            this.returnButton.UseVisualStyleBackColor = true;
            this.returnButton.Click += new System.EventHandler(this.returnButton_Click);
            // 
            // StatusHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.returnButton);
            this.Controls.Add(this.statusDateValues);
            this.Controls.Add(this.statusDescriptions);
            this.Controls.Add(this.statusValues);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 640);
            this.MinimumSize = new System.Drawing.Size(640, 640);
            this.Name = "StatusHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StatusHistory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox statusValues;
        private System.Windows.Forms.TextBox statusDescriptions;
        private System.Windows.Forms.ListBox statusDateValues;
        private System.Windows.Forms.Button returnButton;
    }
}