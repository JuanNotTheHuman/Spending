using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spending.Enumerables;
namespace Spending.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Category Category { get; set; }
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
