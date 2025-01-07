using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace databaseexpress
{
    public partial class LoginForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-0MVK7P6\\SQLEXPRESS;Initial Catalog=employee;Integrated Security=True;";

        public LoginForm()
        {
            InitializeComponent();
            this.KeyPreview = true;  // Ensure the form can capture key events
            this.KeyDown += new KeyEventHandler(btnLogin_KeyDown); // Attach the KeyDown event
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(1) FROM tbl_admin WHERE username = @username AND password = @password";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        con.Open();
                        int result = Convert.ToInt32(cmd.ExecuteScalar());

                        if (result == 1)
                        {
                            MessageBox.Show("Login successful! Welcome Admin.");
                            this.Hide();

                            
                            // Pass the admin ID to the SalaryForm constructor
                            SalaryForm salaryForm = new SalaryForm();
                            salaryForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during login: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the login process when Enter is pressed
                btnLogin_Click(sender, e);

            }
        }
    }
}
