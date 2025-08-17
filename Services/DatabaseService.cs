using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
        static DatabaseService()
        {
            using (var connection = new SqliteConnection("Data Source=spending.db"))
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
                        Type INTEGER NOT NULL
                    )";
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
            using (var connection = new SqliteConnection("Data Source=spending.db"))
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
                            Type = (RecordType)reader.GetInt32(5)
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
            using (var connection = new SqliteConnection("Data Source=spending.db"))
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
        /**
         * <summary>
         * A method to retrieve records by type asynchronously.
         * </summary>
         * <param name="type">The type of records to filter by.</param>
         */
        public static async Task<List<Record>> GetRecordsByTypeAsync(RecordType type)
        {
            using (var connection = new SqliteConnection("Data Source=spending.db"))
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
        /**
         * <summary>
         * A method to edit an existing record in the database asynchronously.
         * </summary>
         * <param name="record">The record to edit.</param>
         */
        public static async Task EditRecordAsync(Record record)
        {
            using (var connection = new SqliteConnection("Data Source=spending.db"))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Records
                    SET Name = @Name, Amount = @Amount, Date = @Date, Category = @Category, Type = @Type
                    WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", record.Id);
                command.Parameters.AddWithValue("@Name", record.Name);
                command.Parameters.AddWithValue("@Amount", record.Amount);
                command.Parameters.AddWithValue("@Date", record.Date.ToString("o"));
                command.Parameters.AddWithValue("@Category", (int)record.Category);
                command.Parameters.AddWithValue("@Type", (int)record.Type);
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
            using (var connection = new SqliteConnection("Data Source=spending.db"))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Records (Name, Amount, Date, Category, Type)
                    VALUES (@Name, @Amount, @Date, @Category, @Type)";
                command.Parameters.AddWithValue("@Name", record.Name);
                command.Parameters.AddWithValue("@Amount", record.Amount);
                command.Parameters.AddWithValue("@Date", record.Date.ToString("o"));
                command.Parameters.AddWithValue("@Category", (int)record.Category);
                command.Parameters.AddWithValue("@Type", (int)record.Type);
                await command.ExecuteNonQueryAsync();
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
            using (var connection = new SqliteConnection("Data Source=spending.db"))
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
            using (var connection = new SqliteConnection("Data Source=spending.db"))
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
            var records = await GetRecordsAsync();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            return records
                .Where(r => r.Date.Month == currentMonth && r.Date.Year == currentYear)
                .Sum(r => r.Type == RecordType.Income ? r.Amount : -r.Amount);
        }
        /**
         * <summary>
         * A method to get the current month's income asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetCurrentMonthIncome()
        {
            var records = await GetRecordsAsync();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            return records
                .Where(r => r.Date.Month == currentMonth && r.Date.Year == currentYear && r.Type == RecordType.Income)
                .Sum(r => r.Amount);
        }
        /**
         * <summary>
         * A method to get the current month's expenses asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetCurrentMonthExpense()
        {
            var records = await GetRecordsAsync();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            return records
                .Where(r => r.Date.Month == currentMonth && r.Date.Year == currentYear && r.Type == RecordType.Expense)
                .Sum(r => r.Amount);
        }
        /**
         * <summary>
         * A method to get the total balance of all records asynchronously.
         * </summary>
         */
        public static async Task<decimal> GetTotalBalance()
        {
            var records = await GetRecordsAsync();
            return records.Sum(r => r.Type == RecordType.Income ? r.Amount : -r.Amount);
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
                    ("Type", "INTEGER")
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
                    MessageBox.Show("No tables found in the database.");
                    return Task.CompletedTask;
                }
                var message = new StringBuilder("Import Results:\n");
                foreach (var result in results)
                {
                    message.AppendLine($"{result.Name}: {(result.Matches ? "Matches" : "Does not match")}");
                }
                message.AppendLine("\nDo you want to import the data?");
                bool res = NotificationService.AskConfirmation("Import Results", message.ToString());
                if (res)
                {
                    var ImportTables = results.Where(r => r.Matches).Select(r => r.Name).ToList();
                    foreach (string importTableName in ImportTables)
                    {
                        using (var importCommand = connection.CreateCommand())
                        {
                            importCommand.CommandText = $"SELECT (Name,Amount,Date,Category,Type) FROM {importTableName}";
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
                    NotificationService.ShowNotification("Import Successful", "The database has been successfully imported.");
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
            File.Copy("spending.db", filePath, true);
            return Task.CompletedTask;
        }
        public static Task Clear()
        {
            using (var connection = new SqliteConnection("Data Source=spending.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Records";
                command.ExecuteNonQuery();
            }
            NotificationService.ShowNotification("Database Cleared", "All records have been cleared from the database.");
            return Task.CompletedTask;
        }
    }
}
