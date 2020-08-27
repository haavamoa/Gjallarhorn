using System;
using System.Windows.Input;

namespace Gjallarhorn.Server.Resources.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> m_canExecute;
        private readonly Action<object> m_execute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<object> execute,
            Predicate<object> canExecute)
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }



        public bool CanExecute(object parameter)
        {
            if (m_canExecute == null)
            {
                return true;
            }

            return m_canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            m_execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}