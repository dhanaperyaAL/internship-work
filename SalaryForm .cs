using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace databaseexpress
{
    public partial class SalaryForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-0MVK7P6\\SQLEXPRESS;Initial Catalog=employee;Integrated Security=True;";
       
        public SalaryForm()
        {
            InitializeComponent();
        }


       

        private void SalaryForm_Load(object sender, EventArgs e)
        {
            LoadSalaryData();
        }

        private void LoadSalaryData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                   
                    string query = "SELECT e.id, e.first_name, e.job_title, e.department, s.salary, s.pay_date from tbl_employee e inner join tbl_salary s on e.id =s.employee_id"; // Change 'SalaryData' to your actual salary table name
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            
                            salaryDataGridView.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("No salary records found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading salary data: " + ex.Message);
            }
        }
        

       
        private void btnShowSalaryData_Click(object sender, EventArgs e)
        {
            LoadSalaryData();
        }
    }
}

