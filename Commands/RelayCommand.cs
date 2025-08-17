using System;
using System.Windows.Input;

namespace JuanNotTheHuman.Spending.Commands
{
    /**
     * <summary>
     * A simple implementation of the ICommand interface that allows you to define commands in a more straightforward way.
     * </summary>
    */
    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public RelayCommand() { }
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        /**
         * <summary
         * Determines whether the command can execute in its current state.
         * </summary>
        */
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }
        /**
         * <summary>
         * Executes the command.
         * </summary>
        */
        public void Execute(object parameter)
        {
            _execute();
        }
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
    /**
     * <summary>
     * A generic version of the RelayCommand that allows you to pass a parameter
     * </summary>
    */
    internal class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        /**
         * <summary>
         * Determines whether the command can execute in its current state.
         * </summary>
         */
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }
        /**
         * <summary>
         * Executes the command.
         * </summary>
         */
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
