namespace PresentationLayer
{
    partial class CapstoneListFaculty
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
            this.pendingCapstones = new System.Windows.Forms.DataGridView();
            this.currentCapstones = new System.Windows.Forms.DataGridView();
            this.pendingLabel = new System.Windows.Forms.TextBox();
            this.currentLabel = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Accept = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Reject = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Title2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Chair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pendingCapstones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentCapstones)).BeginInit();
            this.SuspendLayout();
            // 
            // pendingCapstones
            // 
            this.pendingCapstones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pendingCapstones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.View,
            this.Accept,
            this.Reject});
            this.pendingCapstones.Location = new System.Drawing.Point(13, 54);
            this.pendingCapstones.Name = "pendingCapstones";
            this.pendingCapstones.Size = new System.Drawing.Size(600, 240);
            this.pendingCapstones.TabIndex = 0;
            this.pendingCapstones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.pendingCapstones_CellContentClick);
            // 
            // currentCapstones
            // 
            this.currentCapstones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.currentCapstones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title2,
            this.View2,
            this.Chair});
            this.currentCapstones.Location = new System.Drawing.Point(14, 349);
            this.currentCapstones.Name = "currentCapstones";
            this.currentCapstones.Size = new System.Drawing.Size(599, 240);
            this.currentCapstones.TabIndex = 1;
            // 
            // pendingLabel
            // 
            this.pendingLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pendingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pendingLabel.Location = new System.Drawing.Point(13, 26);
            this.pendingLabel.Name = "pendingLabel";
            this.pendingLabel.ReadOnly = true;
            this.pendingLabel.Size = new System.Drawing.Size(600, 22);
            this.pendingLabel.TabIndex = 2;
            this.pendingLabel.Text = "Pending Capstones:";
            // 
            // currentLabel
            // 
            this.currentLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.currentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLabel.Location = new System.Drawing.Point(13, 321);
            this.currentLabel.Name = "currentLabel";
            this.currentLabel.ReadOnly = true;
            this.currentLabel.Size = new System.Drawing.Size(600, 22);
            this.currentLabel.TabIndex = 3;
            this.currentLabel.Text = "Current Capstones:";
            // 
            // Title
            // 
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 255;
            // 
            // View
            // 
            this.View.HeaderText = "View";
            this.View.Name = "View";
            this.View.Text = "View";
            // 
            // Accept
            // 
            this.Accept.HeaderText = "Accept";
            this.Accept.Name = "Accept";
            this.Accept.Text = "Accept";
            // 
            // Reject
            // 
            this.Reject.HeaderText = "Reject";
            this.Reject.Name = "Reject";
            this.Reject.Text = "Reject";
            // 
            // Title2
            // 
            this.Title2.HeaderText = "Title";
            this.Title2.Name = "Title2";
            this.Title2.ReadOnly = true;
            this.Title2.Width = 255;
            // 
            // View2
            // 
            this.View2.HeaderText = "View";
            this.View2.Name = "View2";
            this.View2.Text = "View";
            // 
            // Chair
            // 
            this.Chair.HeaderText = "Chair";
            this.Chair.Name = "Chair";
            this.Chair.ReadOnly = true;
            this.Chair.Width = 200;
            // 
            // CapstoneListFaculty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.currentLabel);
            this.Controls.Add(this.pendingLabel);
            this.Controls.Add(this.currentCapstones);
            this.Controls.Add(this.pendingCapstones);
            this.Name = "CapstoneListFaculty";
            this.Text = "CapstoneListFaculty";
            ((System.ComponentModel.ISupportInitialize)(this.pendingCapstones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentCapstones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView pendingCapstones;
        private System.Windows.Forms.DataGridView currentCapstones;
        private System.Windows.Forms.TextBox pendingLabel;
        private System.Windows.Forms.TextBox currentLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewButtonColumn Accept;
        private System.Windows.Forms.DataGridViewButtonColumn Reject;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title2;
        private System.Windows.Forms.DataGridViewButtonColumn View2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Chair;
    }
}