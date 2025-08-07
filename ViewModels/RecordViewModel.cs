using Spending.Commands;
using Spending.Enumerables;
using Spending.Models;
using Spending.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spending.ViewModels
{
    internal class RecordViewModel : ViewModelBase
    {
        private int _id;
        private string _name;
        private decimal _amount;
        private DateTime _date;
        private Category _category;
        private RecordType _type;
        public int Id
        {
            get => _id;
        }
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }
        public Category Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }
        public RecordType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }
        public Record GetRecord()
        {
            return new Record(_id,_name, _amount, _date, _category,_type);
        }
        public RecordViewModel(Record record)
        {
            _id = record.Id;
            _name = record.Name;
            _amount = record.Amount;
            _date = record.Date;
            _category = record.Category;
            _type = record.Type;
        }
        public RecordViewModel(string name, decimal amount, DateTime date, Category category, RecordType type)
        {
            _name = name;
            _amount = amount;
            _date = date;
            _category = category;
            _type = type;
        }
        public RecordViewModel()
        {
            _name = string.Empty;
            _amount = 0;
            _date = DateTime.Now;
            _category = Category.Other;
            _type = RecordType.Expense;
        }
        public ICommand EditCommand => new RelayCommand(() =>
        {

        });
    }
}
