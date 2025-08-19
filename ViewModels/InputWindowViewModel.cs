using JuanNotTheHuman.Spending.Commands;
using JuanNotTheHuman.Spending.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JuanNotTheHuman.Spending.ViewModels
{
    public class InputWindowViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private string _input;
        public string Title
        {
            get => _title;
            set
            {
                if(value != _title)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if(value != _description)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public string Input
        {
            get => _input;
            set
            {
                if(value != _input)
                {
                    _input = value;
                    OnPropertyChanged(nameof(Input));
                }
            }
        }
        public event Action<bool> CloseRequested;
        public InputWindowViewModel(string title, string description)
        {
            Title = title;
            Description = description;
            Input = string.Empty;
        }
        public InputWindowViewModel() { }
        public ICommand SubmitCommand => new RelayCommand(() =>
        {
            CloseRequested?.Invoke(true);
        });
        public ICommand CancelCommand => new RelayCommand(() =>
        {
            CloseRequested?.Invoke(false);
        });
    }
}
