using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spending.Services
{
    public class LocalizationService : INotifyPropertyChanged
    {
        private static readonly ResourceManager _resourceManager = new ResourceManager("Spending.Resources.Resources", typeof(LocalizationService).Assembly);
        public static LocalizationService Instance { get; } = new LocalizationService();
        public event PropertyChangedEventHandler PropertyChanged;
        public string this[string key] => _resourceManager.GetString(key,Thread.CurrentThread.CurrentUICulture);
        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
