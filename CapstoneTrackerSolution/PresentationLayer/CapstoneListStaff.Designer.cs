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
            this.capstoneGrid = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderLabel = new System.Windows.Forms.TextBox();
            this.orderValues = new System.Windows.Forms.ComboBox();
            this.selectValues = new System.Windows.Forms.ComboBox();
            this.selectLabel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.capstoneGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // capstoneGrid
            // 
            this.capstoneGrid.AllowUserToAddRows = false;
            this.capstoneGrid.AllowUserToDeleteRows = false;
            this.capstoneGrid.AllowUserToResizeColumns = false;
            this.capstoneGrid.AllowUserToResizeRows = false;
            this.capstoneGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.capstoneGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.View,
            this.Status});
            this.capstoneGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.capstoneGrid.Location = new System.Drawing.Point(12, 12);
            this.capstoneGrid.Name = "capstoneGrid";
            this.capstoneGrid.Size = new System.Drawing.Size(452, 577);
            this.capstoneGrid.TabIndex = 0;
            // 
            // Title
            // 
            this.Title.HeaderText = "Title";
            this.Title.MaxInputLength = 100;
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Title.Width = 200;
            // 
            // View
            // 
            this.View.HeaderText = "View";
            this.View.Name = "View";
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MaxInputLength = 45;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // orderLabel
            // 
            this.orderLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.orderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLabel.Location = new System.Drawing.Point(470, 13);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.ReadOnly = true;
            this.orderLabel.Size = new System.Drawing.Size(142, 22);
            this.orderLabel.TabIndex = 1;
            this.orderLabel.Text = "Order By:";
            this.orderLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // orderValues
            // 
            this.orderValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderValues.FormattingEnabled = true;
            this.orderValues.Items.AddRange(new object[] {
            "Name",
            "Date",
            "Status"});
            this.orderValues.Location = new System.Drawing.Point(470, 41);
            this.orderValues.Name = "orderValues";
            this.orderValues.Size = new System.Drawing.Size(142, 26);
            this.orderValues.TabIndex = 2;
            // 
            // selectValues
            // 
            this.selectValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectValues.FormattingEnabled = true;
            this.selectValues.Items.AddRange(new object[] {
            "Pending",
            "Ongoing",
            "Completed"});
            this.selectValues.Location = new System.Drawing.Point(470, 305);
            this.selectValues.Name = "selectValues";
            this.selectValues.Size = new System.Drawing.Size(142, 26);
            this.selectValues.TabIndex = 4;
            // 
            // selectLabel
            // 
            this.selectLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.selectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectLabel.Location = new System.Drawing.Point(470, 277);
            this.selectLabel.Name = "selectLabel";
            this.selectLabel.ReadOnly = true;
            this.selectLabel.Size = new System.Drawing.Size(142, 22);
            this.selectLabel.TabIndex = 3;
            this.selectLabel.Text = "Select:";
            this.selectLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CapstoneListStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.selectValues);
            this.Controls.Add(this.selectLabel);
            this.Controls.Add(this.orderValues);
            this.Controls.Add(this.orderLabel);
            this.Controls.Add(this.capstoneGrid);
            this.Name = "CapstoneListStaff";
            this.Text = "CapstoneListStaff";
            ((System.ComponentModel.ISupportInitialize)(this.capstoneGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView capstoneGrid;
        private System.Windows.Forms.TextBox orderLabel;
        private System.Windows.Forms.ComboBox orderValues;
        private System.Windows.Forms.ComboBox selectValues;
        private System.Windows.Forms.TextBox selectLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}