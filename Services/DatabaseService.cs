using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JuanNotTheHuman.Spending.Services
{
    /**
     * <summary>
     * A service for managing database operations related to spending records.
     * </summary>
     */
    internal static class DatabaseService
    {
        private readonly static string _connectionString = "Data Source=spending.db";
        static DatabaseService()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Records (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Amount REAL NOT NULL,
                        Date TEXT NOT NULL,
                        Category INTEGER NOT NULL,
                        Type INTEGER NOT NULL,
                        Image BLOB
                    )";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE VIEW IF NOT EXISTS CurrentMonthNet AS SELECT SUM(CASE WHEN Type = 0 THEN Amount ELSE -Amount END) AS Net FROM Records WHERE strftime('%Y-%m',Date) = strftime('%Y-%m','now')";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE VIEW IF NOT EXISTS CurrentMonthIncome AS SELECT SUM(Amount) AS Income FROM Records WHERE strftime('%Y-%m',Date) = strftime('%Y-%m','now') AND Type = 0";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE VIEW IF NOT EXISTS CurrentMonthExpense AS SELECT SUM(Amount) AS Expense FROM Records WHERE strftime('%Y-%m',Date) = strftime('%Y-%m','now') AND Type = 1";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE VIEW IF NOT EXISTS TotalBalance AS SELECT SUM(CASE WHEN Type = 0 THEN Amount ELSE -Amount END) AS Balance FROM Records"; 
                command.ExecuteNonQuery();

            }
        }
        /**
         * <summary>
         * A method to retrieve all records from the database asynchronously.
         * </summary>
         */
        public static async Task<List<Record>> GetRecordsAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Records";
                var records = new List<Record>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var record = new Record
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            Date = DateTime.Parse(reader.GetString(3)),
                            Category = (Category)reader.GetInt32(4),
                            Type = (RecordType)reader.GetInt32(5),
                            Image = reader.IsDBNull(6) ? null : (byte[])reader["Image"]
                        };
                        records.Add(record);
                    }
                }
                return records;
            }
        }
        /**
         * <summary>
         * A method to retrieve records by category asynchronously.
         * </summary>
         * <param name="category">The category to filter records by.</param>
         */
        public static async Task<List<Record>> GetRecordsByCategoryAsync(Category category)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Records WHERE Category = @Category";
                command.Parameters.AddWithValue("@Category", (int)category);
                var records = new List<Record>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var record = new Record
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            Date = DateTime.Parse(reader.GetString(3)),
                            Category = (Category)reader.GetInt32(4),
                            Type = (RecordType)reader.GetInt32(5)
                        };
                        records.Add(record);
                    }
                }
                return records;
            }
        }
        // Poprawka dla błędu CS0030: prawidłowe pobieranie danych BLOB z czytnika
        public static async Task<List<Record>> GetRecordsByTypeAsync(RecordType type)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Records WHERE Type = @Type";
                command.Parameters.AddWithValue("@Type", (int)type);
                var records = new List<Record>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        byte[] image = null;
                        if (!reader.IsDBNull(6))
                        {
                            long length = reader.GetBytes(6, 0, null, 0, 0);
                            image = new byte[length];
                            reader.GetBytes(6, 0, image, 0, (int)length);
                        }
                        var record = new Record
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            Date = DateTime.Parse(reader.GetString(3)),
                            Category = (Category)reader.GetInt32(4),
                            Type = (RecordType)reader.GetInt32(5),
                            Image = image
                        };
                        records.Add(record);
                    }
                }
                return records;
            }
        }
        /**
         * <summary>
         * A method to edit an existing record in the database asynchronously.
         * </summary>
         * <param name="record">The record to edit.</param>
         */
        public static async Task EditRecordAsync(Record record)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
            UPDATE Records
            SET Name = @Name, Amount = @Amount, Date = @Date,
                Category = @Category, Type = @Type, Image = @Image
            WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", record.Id);
                command.Parameters.AddWithValue("@Name", record.Name);
                command.Parameters.AddWithValue("@Amount", record.Amount);
                command.Parameters.AddWithValue("@Date", record.Date.ToString("o"));
                command.Parameters.AddWithValue("@Category", (int)record.Category);
                command.Parameters.AddWithValue("@Type", (int)record.Type);
                var imageParam = command.CreateParameter();
                imageParam.ParameterName = "@Image";
                imageParam.SqliteType = SqliteType.Blob;
                imageParam.Value = (object)record.Image ?? DBNull.Value;
                command.Parameters.Add(imageParam);

                await command.ExecuteNonQueryAsync();
            }
        }
        /**
         * <summary>
         * A method to add a new record to the database asynchronously.
         * </summary>
         * <param name="record">The record to add.</param>
         */
        public static async Task AddRecordAsync(Record record)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                INSERT INTO Records (Name, Amount, Date, Category, Type, Image)
                VALUES (@Name, @Amount, @Date, @Category, @Type, @Image)";

                    command.Parameters.AddWithValue("@Name", record.Name);
                    command.Parameters.AddWithValue("@Amount", record.Amount);
                    command.Parameters.AddWithValue("@Date", record.Date.ToString("o"));
                    command.Parameters.AddWithValue("@Category", (int)record.Category);
                    command.Parameters.AddWithValue("@Type", (int)record.Type);

                    var imageParam = command.CreateParameter();
                    imageParam.ParameterName = "@Image";
                    imageParam.SqliteType = SqliteType.Blob;
                    imageParam.Value = (object)record.Image ?? DBNull.Value;
                    command.Parameters.Add(imageParam);

                    Debug.WriteLine(record.Image == null
                        ? "No image"
                        : $"Image size: {record.Image.Length} bytes");

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /**
         * <summary>
         * A method to delete a record from the database asynchronously.
         * </summary>
         * <param name="record">The record to delete.</param>
         */
        public static async Task DeleteRecordAsync(Record record)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Records WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", record.Id);
                await command.ExecuteNonQueryAsync();
            }
        }
        /**
         * <summary>
         * A method to delete a record by its ID asynchronously.
         * </summary>
         * <param name="id">The ID of the record to delete.</param>
         */
        public static async Task DeleteRecordAsync(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Records WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
        }
        /**
         * <summary>
         * A method to get the current month's net amount (income - expenses) asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetCurrentMonthNet()
        {
            using(var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Net FROM CurrentMonthNet";
                var result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToDecimal(result);
                }
                return 0m;
            }
        }
        /**
         * <summary>
         * A method to get the current month's income asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetCurrentMonthIncome()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Income FROM CurrentMonthIncome";
                var result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToDecimal(result);
                }
                return 0m;
            }
        }
        /**
         * <summary>
         * A method to get the current month's expenses asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetCurrentMonthExpense()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Expense FROM CurrentMonthExpense";
                var result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToDecimal(result);
                }
                return 0m;
            }
        }
        /**
         * <summary>
         * A method to get the total balance of all records asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetTotalBalance()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Balance FROM TotalBalance";
                var result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToDecimal(result);
                }
                return 0m;
            }
        }
        /**
         * <summary>
         * A class representing the result of an import operation.
         * </summary>
         */
        public class ImportResult
        {
            public string Name { get; set; }
            public bool Matches { get; set; }
        }
        private static bool SchemasMatch(List<(string, string)> expected, List<(string, string)> actual)
        {
            foreach (var item in expected)
            {
                if (!actual.Any(a => a.Item1.Equals(item.Item1, StringComparison.OrdinalIgnoreCase) &&
                                     a.Item2.Equals(item.Item2, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }
            return true;
        }
        /**
         * <summary>
         * A method to import a database from a file asynchronously.
         * </summary>
         * <param name="filePath">The path to the database file.</param>
         */
        public static Task ImportDatabase(string filePath)
        {
            using (var connection = new SqliteConnection($"Data Source={filePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
                var tables = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
                var Schema = new List<(string, string)>()
                {
                    ("Name", "TEXT"),
                    ("Amount", "REAL"),
                    ("Date", "TEXT"),
                    ("Category", "INTEGER"),
                    ("Type", "INTEGER"),
                    ("Image","BLOB")
                }.OrderBy(x => x.Item1).ToList();
                var results = new List<ImportResult>();
                foreach (string tableName in tables)
                {
                    using (var GetSchemaCommand = connection.CreateCommand())
                    {
                        GetSchemaCommand.CommandText = $"PRAGMA table_info({tableName})";
                        using (var schemaReader = GetSchemaCommand.ExecuteReader())
                        {
                            var columns = new List<(string, string)>();
                            while (schemaReader.Read())
                            {
                                columns.Add((schemaReader.GetString(1), schemaReader.GetString(2)));
                            }
                            columns = columns.OrderBy(x => x.Item1).ToList();
                            if (SchemasMatch(Schema, columns))
                            {
                                results.Add(new ImportResult
                                {
                                    Name = tableName,
                                    Matches = true
                                });
                            }
                            else
                            {
                                results.Add(new ImportResult
                                {
                                    Name = tableName,
                                    Matches = false
                                });
                            }
                        }
                    }
                }
                if (results.Count == 0)
                {
                    MessageBox.Show(LocalizationService.Instance["NoTablesMessage"]);
                    return Task.CompletedTask;
                }
                var message = new StringBuilder($"{LocalizationService.Instance["DatabaseImportResultsTitle"]}:\n");
                foreach (var result in results)
                {
                    message.AppendLine($"{result.Name}: {(result.Matches ? LocalizationService.Instance["Matches"] : LocalizationService.Instance["NotMatches"])}");
                }
                message.AppendLine($"\n{LocalizationService.Instance["ImportDataQuestion"]}");
                bool res = NotificationService.AskConfirmation(LocalizationService.Instance["DatabaseImportResultsTitle"], message.ToString());
                if (res)
                {
                    var ImportTables = results.Where(r => r.Matches).Select(r => r.Name).ToList();
                    foreach (string importTableName in ImportTables)
                    {
                        using (var importCommand = connection.CreateCommand())
                        {
                            importCommand.CommandText = $"SELECT (Name,Amount,Date,Category,Type,Image) FROM {importTableName}";
                            using (var reader = importCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var record = new Record
                                    {
                                        Name = reader.GetString(0),
                                        Amount = reader.GetDecimal(1),
                                        Date = DateTime.Parse(reader.GetString(2)),
                                        Category = (Category)reader.GetInt32(3),
                                        Type = (RecordType)reader.GetInt32(4)
                                    };
                                    AddRecordAsync(record).Wait();
                                }
                            }
                        }
                    }
                    NotificationService.ShowNotification(LocalizationService.Instance["DatabaseImportSuccessTitle"], LocalizationService.Instance["DatabaseImportSuccessText"]);
                }
            }
            return Task.CompletedTask;
        }
        /**
         * <summary>
         * A method to export the database to a specified file path asynchronously.
         * </summary>
         * <param name="filePath">The path to export the database to.</param>
         */
        public static Task ExportDatabase(string filePath)
        {
            File.Copy(_connectionString, filePath, true);
            return Task.CompletedTask;
        }
        public static Task Clear()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Records";
                command.ExecuteNonQuery();
            }
            NotificationService.ShowNotification(LocalizationService.Instance["DatabaseClearedTitle"], LocalizationService.Instance["DatabaseClearedText"]);
            return Task.CompletedTask;
        }
    }
}
