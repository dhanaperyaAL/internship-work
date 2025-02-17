using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using WPFDBTool.connectionfiles;
using WPFDBTool.LogFiles;
using WPFDBTool;
using System;


namespace WPFDBTool
{
    public partial class RFIDDashboard : Page
    {
        private readonly string connectionString = "Data Source=DESKTOP-0MVK7P6\\SQLEXPRESS; Initial Catalog=kcw_reports; Integrated Security=True";

        public RFIDDashboard()
        {
            InitializeComponent();
            LoadEarlyEntryChart();
            LoadLateEntryExitChart();
            LoadGatepassComplianceChart();
        }

        private void LoadEarlyEntryChart()
        {
            var entries = new ChartValues<int>();
            var labels = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT gp.entry_time AS 'Entry Time Slot', COUNT(r.tracking_id) AS 'Early Entry Count'
                    FROM kcw_reports.dbo.rfid_reader_tracking r
                    JOIN kcw_reports.dbo.gatepass gp ON gp.id = r.gatepass_id
                    WHERE gp.entry_time BETWEEN '06:00:00' AND '07:00:00'
                    GROUP BY gp.entry_time
                    ORDER BY gp.entry_time";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    labels.Add(reader["Entry Time Slot"].ToString());
                    entries.Add((int)reader["Early Entry Count"]);
                }
            }

            EarlyEntryChart.Series.Add(new ColumnSeries { Values = entries, Title = "Early Entry Count" });
            EarlyEntryChart.AxisX.Add(new Axis { Labels = labels });
        }

        private void LoadLateEntryExitChart()
        {
            var entryValues = new ChartValues<int>();
            var exitValues = new ChartValues<int>();
            var labels = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT ISNULL(gp.exit_time, gp.entry_time) AS 'Time Slot',
                    CASE WHEN r.antenna_number = 1 THEN 'IN' WHEN r.antenna_number = 2 THEN 'OUT' ELSE 'UNKNOWN' END AS 'Entry/Exit Type',
                    COUNT(r.tracking_id) AS 'Count'
                    FROM kcw_reports.dbo.rfid_reader_tracking r
                    LEFT JOIN kcw_reports.dbo.gatepass gp ON gp.id = r.gatepass_id
                    WHERE (gp.exit_time > '18:00:00' OR gp.entry_time > '18:00:00')
                    GROUP BY ISNULL(gp.exit_time , gp.entry_time),
                    CASE WHEN r.antenna_number = 1 THEN 'IN' WHEN r.antenna_number = 2 THEN 'OUT' ELSE 'UNKNOWN' END
                    ORDER BY 'Time Slot'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string timeSlot = reader["Time Slot"].ToString();
                    if (!labels.Contains(timeSlot)) labels.Add(timeSlot);

                    if (reader["Entry/Exit Type"].ToString() == "IN")
                        entryValues.Add((int)reader["Count"]);
                    else
                        exitValues.Add((int)reader["Count"]);
                }
            }

            LateEntryExitChart.Series.Add(new StackedColumnSeries { Title = "IN", Values = entryValues });
            LateEntryExitChart.Series.Add(new StackedColumnSeries { Title = "OUT", Values = exitValues });
            LateEntryExitChart.AxisX.Add(new Axis { Labels = labels });
        }

        private void LoadGatepassComplianceChart()
        {
            var values = new ChartValues<int>();
            var labels = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CASE WHEN r.status = 1 THEN 'Authorized'
                                WHEN r.status = 0 THEN 'UnAuthorized'
                                ELSE 'UNKNOWN' END AS 'Gatepass Status',
                    COUNT(r.tracking_id) AS 'Count'
                    FROM kcw_reports.dbo.rfid_reader_tracking r
                    GROUP BY r.status
                    ORDER BY 'Count' DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    labels.Add(reader["Gatepass Status"].ToString());
                    values.Add((int)reader["Count"]);
                }
            }

            GatepassComplianceChart.Series.Add(new ColumnSeries { Values = values, Title = "Gatepass Compliance" });
            GatepassComplianceChart.AxisX.Add(new Axis { Labels = labels });
        }
    }
}
