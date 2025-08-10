using System;
using JuanNotTheHuman.Spending.Enumerables;
namespace JuanNotTheHuman.Spending.Models
{
    /**
     * <summary>
     * A record representing a financial transaction.
     * </summary>
     */
    internal class Record
    {
        /**
         * <summary>
         * Unique identifier for the record.
         * </summary>
         */
        public int Id { get; set; }
        /**
         * <summary>
         * Name of the record.
         * </summary>
         */
        public string Name { get; set; }
        /**
         * <summary>
         * Amount of the transaction.
         * </summary>
         */
        public decimal Amount { get; set; }
        /**
         * <summary>
         * Date of the transaction.
         * </summary>
         */
        public DateTime Date { get; set; }
        /**
         * <summary>
         * Category of the transaction.
         * </summary>
         */
        public Category Category { get; set; }
        /**
         * <summary>
         * Type of the transaction (income or expense).
         * </summary>
         */
        public RecordType Type { get; set; }

        public Record(string name, decimal amount, DateTime date, Category category, RecordType type)
        {
            Name = name;
            Amount = amount;
            Date = date;
            Category = category;
            Type = type;
        }
        public Record(int id, string name, decimal amount, DateTime date, Category category, RecordType type)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Date = date;
            Category = category;
            Type = type;
        }

        public Record() { }
    }
}
