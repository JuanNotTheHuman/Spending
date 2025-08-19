using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private static readonly string ConnectionString = UserSecretsService.GetSecret("Turso:ConnectionString");
        static DatabaseService()
        {
            UserSecretsService.SetSecret("Turso:ConnectionString", "https://spending-juanthehuman.aws-eu-west-1.turso.io/v2/pipeline");
            if (string.IsNullOrEmpty(ConnectionString))
            {
                bool set = NotificationService.AskConfirmation("Question", "The connection string for the database is not set. Do you want to set it now?");
                if (set)
                {
                    string connectionString = NotificationService.AskInput(ConnectionString, "Set Connection String", "Please enter the connection string for the database:");
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        UserSecretsService.SetSecret("Turso:ConnectionString", connectionString);
                        ConnectionString = connectionString;
                    }
                    else
                    {
                        throw new Exception("Connection string cannot be empty.");
                    }
                }
                else
                {
                    throw new Exception("Connection string for the database is not set.");
                }
            }
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "CREATE TABLE IF NOT EXISTS Records (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Amount REAL, Date TEXT, Category INTEGER, Type INTEGER)"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            if(string.IsNullOrEmpty(UserSecretsService.GetSecret("Turso:Token")))
            {
                bool set = NotificationService.AskConfirmation("Question", "The token for the database is not set. Do you want to set it now?");
                if (set)
                {
                    string token = NotificationService.AskInput("Set Token", "Please enter the token for the database:");
                    if (!string.IsNullOrEmpty(token))
                    {
                        UserSecretsService.SetSecret("Turso:Token", token);
                    }
                    else
                    {
                        throw new Exception("Token cannot be empty.");
                    }
                }
                else
                {
                    throw new Exception("Token for the database is not set.");
                }
            }
            _ = ApiService.Post(ConnectionString, body, UserSecretsService.GetSecret("Turso:Token"));
        }
        /**
         * <summary>
         * A method to retrieve all records from the database asynchronously.
         * </summary>
         */
        public static async Task<List<Record>> GetRecordsAsync()
        {
            try
            {
                var body = new RootRequest
                {
                    requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "SELECT * FROM records"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
                };
                Record[] recordsArr = await ApiService.Post<Record[]>(ConnectionString, new FetchOptions { Authorization = UserSecretsService.GetSecret("Turso:Token"), Method = "POST", Body = body });
                if (recordsArr == null)
                {
                    return new List<Record>();
                }
                return recordsArr.ToList();
            }
            catch (Exception ex)
            {
                NotificationService.ShowNotification("Error",$"Failed to retrieve records from database:\n{ex.Message}");
                return new List<Record>();
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
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "SELECT * FROM Records WHERE Category=@Category"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            body.requests[0].stmt.sql = body.requests[0].stmt.sql.Replace("@Category", ((int)category).ToString());
            try
            {
                Record[] recordsArr = await ApiService.Post<Record[]>(ConnectionString, new FetchOptions { Authorization = UserSecretsService.GetSecret("Turso:Token"), Method = "POST", Body = body });
                if (recordsArr == null)
                {
                    return new List<Record>();
                }
                return recordsArr.ToList();
            }
            catch(Exception ex)
            {
                NotificationService.ShowNotification("Error", $"Failed to load records from database:\n{ex.Message}");
                return new List<Record>();
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
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "SELECT * FROM Records WHERE Type=@Type"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            body.requests[0].stmt.sql = body.requests[0].stmt.sql.Replace("@Type", ((int)type).ToString());
            try
            {

                Record[] recordsArr = await ApiService.Post<Record[]>(ConnectionString, new FetchOptions { Authorization = UserSecretsService.GetSecret("Turso:Token"), Method = "GET", Body = body });
                if (recordsArr == null)
                {
                    return new List<Record>();
                }
                return recordsArr.Where(r => r.Type == type).ToList();
            }
            catch(Exception ex)
            {
                NotificationService.ShowNotification("Error", $"Failed to load records from database:\n{ex.Message}");
                return new List<Record>();
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
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "UPDATE Records SET Name = @Name, Amount = @Amount, Date = @Date, Category = @Category, Type = @Type WHERE Id = @Id"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            body.requests[0].stmt.sql = body.requests[0].stmt.sql
                .Replace("@Name", record.Name)
                .Replace("@Amount", record.Amount.ToString())
                .Replace("@Date", record.Date.ToString("o"))
                .Replace("@Category", ((int)record.Category).ToString())
                .Replace("@Type", ((int)record.Type).ToString())
                .Replace("@Id", record.Id.ToString());
            bool success = await ApiService.Post(ConnectionString, body,UserSecretsService.GetSecret("Turso:Token"));
            if (!success)
            {
                throw new Exception("Failed to edit record in the database.");
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
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "INSERT INTO Records (Name, Amount, Date, Category, Type) VALUES (@Name, @Amount, @Date, @Category, @Type)"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            body.requests[0].stmt.sql = body.requests[0].stmt.sql
                .Replace("@Name", record.Name)
                .Replace("@Amount", record.Amount.ToString())
                .Replace("@Date", record.Date.ToString("o"))
                .Replace("@Category", ((int)record.Category).ToString())
                .Replace("@Type", ((int)record.Type).ToString());
            bool success = await ApiService.Post(ConnectionString, body,UserSecretsService.GetSecret("Turso:Token"));
            if (!success)
            {
                throw new Exception("Failed to add record to the database.");
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
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "DELETE FROM Records WHERE Id = @Id"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            body.requests[0].stmt.sql = body.requests[0].stmt.sql.Replace("@Id", record.Id.ToString());
            bool success = await ApiService.Post(ConnectionString, body,UserSecretsService.GetSecret("Turso:Token"));
            if (!success)
            {
                throw new Exception("Failed to delete record from the database.");
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
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "DELETE FROM Records WHERE Id = @Id"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            body.requests[0].stmt.sql = body.requests[0].stmt.sql.Replace("@Id", id.ToString());
            bool success = await ApiService.Post(ConnectionString, body,UserSecretsService.GetSecret("Turso:Token"));
            if (!success)
            {
                throw new Exception("Failed to delete record from the database.");
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
            var income = records
                .Where(r => r.Date.Month == currentMonth && r.Date.Year == currentYear && r.Type == RecordType.Income)
                .Sum(r => r.Amount);
            var expenses = records
                .Where(r => r.Date.Month == currentMonth && r.Date.Year == currentYear && r.Type == RecordType.Expense)
                .Sum(r => r.Amount);
            return income - expenses;
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
            var records = GetRecordsAsync().Result;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var connection = new SqliteConnection($"Data Source={filePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Records (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Amount REAL, Date TEXT, Category INTEGER, Type INTEGER)";
                command.ExecuteNonQuery();
                var insertCommand = connection.CreateCommand();
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("INSERT INTO Records (Name, Amount, Date, Category, Type) VALUES ");
                foreach (var record in records)
                {
                    stringBuilder.AppendLine($"('{record.Name}', {record.Amount}, '{record.Date:yyyy-MM-dd HH:mm:ss}', {(int)record.Category}, {(int)record.Type}),");
                }
                stringBuilder.Length -= 3;
                insertCommand.CommandText = stringBuilder.ToString();
                insertCommand.ExecuteNonQuery();
                NotificationService.ShowNotification("Export Successful", "The database has been successfully exported.");
            }
            return Task.CompletedTask;
        }
        public static Task Clear()
        {
            var body = new RootRequest
            {
                requests = new List<SqlRequest>
                {
                    new SqlRequest
                    {
                        type = "execute",
                        stmt = new SqlStatement
                        {
                            sql = "DELETE FROM Records"
                        }
                    },
                    new SqlRequest
                    {
                        type = "close"
                    }
                }
            };
            return ApiService.Post(ConnectionString, body, UserSecretsService.GetSecret("Torso:Token"));
        }
    }
}
