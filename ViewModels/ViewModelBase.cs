using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JuanNotTheHuman.Spending.ViewModels
{
    /**
     * <summary>
     * A base class for view models that implements INotifyPropertyChanged.
     * </summary>
     */
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
