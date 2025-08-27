using JuanNotTheHuman.Spending.Enumerables;
using JuanNotTheHuman.Spending.Models;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JuanNotTheHuman.Spending.ViewModels
{
    /**
     * <summary>
     * A view model for a record in the application.
     * </summary>
     */
    internal class RecordViewModel : ViewModelBase
    {
        private int _id;
        private string _name;
        private decimal _amount;
        private DateTime _date;
        private Category _category;
        private RecordType _type;
        private byte[] _image;
        /**
         * <summary>
         * Gets or sets the ID of the record.
         * </summary>
         */
        public int Id
        {
            get => _id;
        }
        /**
         * <summary>
         * Gets or sets the name of the record.
         * </summary>
         */
        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }
        /**
         * <summary>
         * Gets or sets the amount of the record.
         * </summary>
         */
        public decimal Amount
        {
            get => _amount;
            set => Set(ref _amount, value, nameof(Amount));
        }
        /**
         * <summary>
         * Gets or sets the date of the record.
         * </summary>
         */
        public DateTime Date
        {
            get => _date;
            set => Set(ref _date, value, nameof(Date));
        }
        /**
         * <summary>
         * Gets or sets the category of the record.
         * </summary>
         */
        public Category Category
        {
            get => _category;
            set => Set(ref _category, value, nameof(Category));
        }
        /**
         * <summary>
         * Gets or sets the type of the record (Expense or Income).
         * </summary>
         */
        public RecordType Type
        {
            get => _type;
            set => Set(ref _type, value, nameof(Type));
        }
        /**
         * <summary>
         * Gets or sets the image associated with the record.
         * </summary>
         */
        public byte[] Image
        {
            get => _image;
            set=> Set(ref _image, value, nameof(Image));
        }
        /**
         * <summary>
         * Gets the command to delete the record.
         * </summary>
         */
        public Record GetRecord()
        {
            return new Record(_id, _name, _amount, _date, _category, _type,_image);
        }
        /**
         * <summary>
         * Initializes a new instance of the RecordViewModel class.
         * </summary>
         * <param name="record">The record to initialize the view model with.</param>
         */
        public RecordViewModel(Record record)
        {
            _id = record.Id;
            _name = record.Name;
            _amount = record.Amount;
            _date = record.Date;
            _category = record.Category;
            _type = record.Type;
            _image = record.Image;
        }
        /**
         * <summary>
         * Initializes a new instance of the RecordViewModel class with specified parameters.
         * </summary>
         * <param name="name">The name of the record.</param>
         * <param name="amount">The amount of the record.</param>
         * <param name="date">The date of the record.</param>
         * <param name="category">The category of the record.</param>
         * <param name="type">The type of the record (Expense or Income).</param>
         */
        public RecordViewModel(string name, decimal amount, DateTime date, Category category, RecordType type, byte[] image)
        {
            _name = name;
            _amount = amount;
            _date = date;
            _category = category;
            _type = type;
            _image = image;
        }
        public RecordViewModel()
        {
            _name = string.Empty;
            _amount = 0;
            _date = DateTime.Now;
            _category = Category.Other;
            _type = RecordType.Expense;
            _image = null;
        }
    }
}
