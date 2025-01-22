using Npgsql;
using System;
using System.Data;

namespace src
{
    public class Connexion
    {
        private string connectionString;

        public Connexion(string host, string database, string username, string password)
        {
            connectionString = $"Host={host};Database={database};Username={username};Password={password}";
        }

        public DataTable AfficherTousLesElements(string tableName)
        {
            DataTable dataTable = new DataTable();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT * FROM {tableName}";
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, connection);
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'affichage des éléments : {ex.Message}");
                }
            }

            return dataTable;
        }

        public bool InsererNouvelleLigne(string tableName, string[] columns, object[] values)
        {
            if (columns.Length != values.Length)
            {
                Console.WriteLine("Le nombre de colonnes ne correspond pas au nombre de valeurs.");
                return false;
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string columnNames = string.Join(", ", columns);
                    string parameterPlaceholders = string.Join(", ", values.Select((_, i) => $"@val{i}"));

                    string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterPlaceholders})";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            command.Parameters.AddWithValue($"@val{i}", values[i]);
                        }

                        int affectedRows = command.ExecuteNonQuery();
                        return affectedRows > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'insertion : {ex.Message}");
                    return false;
                }
            }
        }
    }
}
