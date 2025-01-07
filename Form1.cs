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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace databaseexpress
{
    public partial class Form1 : Form
    {
        private Logger logInstance;
        string connectionString = "Data Source=DESKTOP-0MVK7P6\\SQLEXPRESS;Initial Catalog=employee;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
            logInstance = new Logger();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validation.ValidateFields(txtFirstName.Text, txtLastName.Text, txtJobTitle.Text, txtDepartment.Text, txtPhoneNumber.Text, logInstance))
                    return;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "InsertEmployee";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@job_title", txtJobTitle.Text);
                        cmd.Parameters.AddWithValue("@department", txtDepartment.Text);
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        logInstance.Log($"Inserted record: {txtFirstName.Text} {txtLastName.Text}");
                        MessageBox.Show("Record Inserted Successfully!");
                    }
                }
            }
            catch (SqlException ex)
            {
                logInstance.Log($"Insert operation failed: {ex.Message}");
                MessageBox.Show("SQL Error occurred during insertion.");
            }
            catch (Exception ex)
            {
                logInstance.Log($"Insert operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during insertion.");
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validation.ValidateFields(txtFirstName.Text, txtLastName.Text, txtJobTitle.Text, txtDepartment.Text, txtPhoneNumber.Text, logInstance))
                    return;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UpdateEmployee";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", txtId.Text);
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@JobTitle", txtJobTitle.Text);
                        cmd.Parameters.AddWithValue("@Department", txtDepartment.Text);
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();

                        logInstance.Log($"Updated record ID: {txtId.Text} - {txtFirstName.Text} {txtLastName.Text}");
                        MessageBox.Show("Record updated successfully!");
                    }
                }

            }
            catch (Exception ex)
            {
                logInstance.Log($"Update operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during update.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
          try
            { 
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SearchEmployee";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtFirstName.Text = reader["first_name"].ToString();
                            txtLastName.Text = reader["last_name"].ToString();
                            txtJobTitle.Text = reader["job_title"].ToString();
                            txtDepartment.Text = reader["department"].ToString();
                            txtPhoneNumber.Text = reader["phone_number"].ToString();

                            logInstance.Log($"Searched record ID: {txtId.Text} - Found: {txtFirstName.Text} {txtLastName.Text}");
                        }
                        else
                        {
                            MessageBox.Show("Record not found!");
                            logInstance.Log($"Search failed: No record found for ID {txtId.Text}");
                        }
                    }
                }
            }
        }
            catch (Exception ex)
            {
                logInstance.Log($"Search operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during search.");
            }
        }

        private void btnAlter_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "AlterEmployeeTable";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        logInstance.Log("Table altered: Added phone_number column.");
                        MessageBox.Show("Table altered successfully!");
                    }
                }
            }

            catch (Exception ex)
            {
                logInstance.Log($"Alter operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during table alteration.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Please enter an employee ID to delete.");
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DeleteEmployee";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", txtId.Text);

                        try
                        {
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                logInstance.Log($"Deleted record with ID: {txtId.Text}");
                                MessageBox.Show("Record deleted successfully!");
                            }
                            else
                            {
                                MessageBox.Show("Record not found.");
                                logInstance.Log($"Delete failed: No record found for ID {txtId.Text}");
                            }
                        }
                        catch (Exception ex)
                        {
                            logInstance.Log($"Delete operation failed: {ex.Message}");
                            MessageBox.Show("Error occurred during deletion.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {   
                logInstance.Log($"Delete operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during deletion.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            { 
            txtFirstName.Clear();
            txtLastName.Clear();
            txtJobTitle.Clear();
            txtDepartment.Clear();
            txtPhoneNumber.Clear();
            txtId.Clear();

            logInstance.Log("Form cleared.");
        }
            catch (Exception ex)
            {
                logInstance.Log($"Clear operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during clearing.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch_Click(sender, e);
                    logInstance.Log($"Searched record Found");
                }
            }
            catch (Exception ex)
            {
                logInstance.Log($"Search operation failed: {ex.Message}");
                MessageBox.Show("Error occurred during search.");
            }
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void btnLogin_Click_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

        }
    }
}
