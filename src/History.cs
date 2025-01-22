using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace src
{
    public class History : Form
    {
        private DataGridView dataGridView;

        public History()
        {
            this.Text = "Score History";
            this.Size = new Size(600, 400);
            InitializeComponents();
            LoadScores();
        }

        private void InitializeComponents()
        {
            dataGridView = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            this.Controls.Add(dataGridView);
        }

        private void LoadScores()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=etu003247"))
                {
                    connection.Open();
                    string query = "SELECT id_score, id_joueur, valeur, date_score FROM tennis_score ORDER BY date_score ASC";
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading scores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
