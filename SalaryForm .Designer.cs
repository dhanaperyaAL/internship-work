namespace databaseexpress
{
    partial class SalaryForm
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
            this.salaryDataGridView = new System.Windows.Forms.DataGridView();
            this.sqlDataAdapter1 = new Microsoft.Data.SqlClient.SqlDataAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.salaryDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // salaryDataGridView
            // 
            this.salaryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.salaryDataGridView.Location = new System.Drawing.Point(81, 65);
            this.salaryDataGridView.Name = "salaryDataGridView";
            this.salaryDataGridView.RowHeadersWidth = 62;
            this.salaryDataGridView.RowTemplate.Height = 28;
            this.salaryDataGridView.Size = new System.Drawing.Size(1098, 334);
            this.salaryDataGridView.TabIndex = 0;
            // 
            // SalaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 450);
            this.Controls.Add(this.salaryDataGridView);
            this.Name = "SalaryForm";
            this.Text = "SalaryForm";
            this.Load += new System.EventHandler(this.SalaryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.salaryDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView salaryDataGridView;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter1;
    }
}