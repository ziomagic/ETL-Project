using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ETL_Project.Mvvm
{
    public class RelayCommand : ICommand
    {
        public Action _onExecute;
        public RelayCommand(Action action)
        {
            _onExecute = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _onExecute?.Invoke();
        }
    }
}
