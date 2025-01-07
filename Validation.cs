using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace databaseexpress
{
    public static class Validation
    {
        // Adjusted to take logger parameter for proper logging
        public static bool ValidateFields(string firstName, string lastName, string jobTitle, string department, string phoneNumber, Logger logger)
        {
            try
            {
                // Check if any required field is empty or whitespace
                if (string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(jobTitle) ||
                    string.IsNullOrWhiteSpace(department) ||
                    string.IsNullOrWhiteSpace(phoneNumber))
                {
                    logger.Log("Validation failed: One or more required fields are empty.");
                    MessageBox.Show("Please fill in all fields.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Corrected logger call
                logger.Log($"Validation failed: {ex.Message}");
                MessageBox.Show("Error occurred during validation.");
                return false;
            }

            try
            {
                // Validate phone number to ensure it matches the 10-digit format
                if (!Regex.IsMatch(phoneNumber, @"^\d{10}$"))
                {
                    logger.Log("Validation failed: Invalid phone number format.");
                    MessageBox.Show("Please enter a valid phone number (10 digits).");
                    return false;
                }

                return true;  // Validation passed
            }
            catch (Exception ex)
            {
                // Corrected logger call
                logger.Log($"Validation failed: {ex.Message}");
                MessageBox.Show("Error occurred during validation.");
                return false;
            }
        }
    }
}
