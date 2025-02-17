using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WPFDBTool.connectionfiles;
using WPFDBTool.LogFiles;

namespace WPFDBTool
{
    public partial class MainWindow : Window
    {
        private InterDBServices _dbServices;

        public MainWindow()
        {
            InitializeComponent();
            _dbServices = null;

            DatabaseSelector.Items.Clear();

            DatabaseSelector.Items.Add("MS SQL");
            DatabaseSelector.Items.Add("MySQL");
            DatabaseSelector.SelectedIndex = -1;

            LogActions.Log("Application started.");
        }

        private void DatabaseSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatabaseSelector.SelectedItem != null)
            {
                string selectedDb = DatabaseSelector.SelectedItem.ToString();
                try
                {
                    switch (selectedDb)
                    {
                        case "MS SQL":
                            _dbServices = new MSSQL();
                            LogActions.Log("Database selected: MS SQL.");
                            MessageBox.Show("MSSQL database selected.", "Database Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;

                        case "MySQL":
                            _dbServices = new MySQL();
                            LogActions.Log("Database selected: MySQL.");
                            MessageBox.Show("MySQL database selected.", "Database Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;

                        default:
                            _dbServices = null;
                            LogActions.Log("Unknown database type selected.");
                            MessageBox.Show("Unknown database type selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error initializing database service: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dbServices == null)
                {
                    LogActions.Log("GoButton clicked without database selection.");
                    MessageBox.Show("Please select a database type (MSSQL or MySQL) first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string connectionString = ConnectionStringInputBox.Text;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    LogActions.Log("Connection attempt with empty connection string.");
                    MessageBox.Show("Connection string cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_dbServices.Connect(connectionString))
                {
                    LogActions.Log("Connection successful.");
                    MessageBox.Show("Connection Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    LogActions.Log("Connection failed.");
                    MessageBox.Show("Connection Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LogActions.Log($"Error on connection attempt: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            string query = QueryTextBox.Text;

            if (string.IsNullOrWhiteSpace(query))
            {
                LogActions.Log("Attempted to execute an empty query.");
                MessageBox.Show("Query cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = _dbServices.Execute(query);

            if (result.ErrorCode == "0")
            {
                DataGrid.ItemsSource = result.DataTable.DefaultView;
                LogActions.Log("Query executed successfully.");
                MessageBox.Show("Query executed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                LogActions.Log($"Query execution failed. Error: {result.Error}");
                MessageBox.Show($"Error: {result.Error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            QueryTextBox.Clear();
            ConnectionStringInputBox.Clear();
            DataGrid.ItemsSource = null;
            DatabaseSelector.SelectedIndex = -1;
            _dbServices = null;
            LogActions.Log("Application cleared all fields.");
        }

        private void ExportPDFButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF File|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                MessageBoxResult result = MessageBox.Show("Do you want the report to be horizontal?", "Layout Option", MessageBoxButton.YesNo, MessageBoxImage.Question);

                bool isHorizontal = result == MessageBoxResult.Yes;

                // Pass only the filePath, DataGrid, and isHorizontal; remove the query text.
                ExportFilesAs.ExportAsPDF.ExportToPDF(filePath, DataGrid, isHorizontal);

                LogActions.Log($"Exported DataGrid to PDF at {filePath} with {(isHorizontal ? "horizontal" : "vertical")} layout.");
                MessageBox.Show("Data exported to PDF successfully.", "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void UserControl_loaded(object sender, RoutedEventArgs e)
        {
            // Add any initialization logic you need here.
            LogActions.Log("Window loaded.");
        }

        private void ExportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV File|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                ExportDataGridToCSV(filePath);
                LogActions.Log($"Exported DataGrid to CSV at {filePath}.");
                MessageBox.Show("Data exported to CSV successfully.", "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportDataGridToCSV(string filePath)
        {
            StringBuilder csvContent = new StringBuilder();

            foreach (DataGridColumn column in DataGrid.Columns)
            {
                csvContent.Append("\"" + column.Header.ToString().Replace("\"", "\"\"") + "\","); // Escape quotes
            }
            csvContent.AppendLine();

            foreach (var item in DataGrid.Items)
            {
                if (item is DataRowView dataRow)
                {
                    foreach (var cell in dataRow.Row.ItemArray)
                    {
                        csvContent.Append("\"" + cell?.ToString().Replace("\"", "\"\"") + "\","); // Escape quotes
                    }
                    csvContent.AppendLine();
                }
            }

            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
