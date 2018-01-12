using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ETL_Project.Mvvm
{
    /// <summary>
    /// Klasa będąca składnikiem implementacji wzorca MVVM
    /// </summary>
    public class RelayCommand : ICommand
    {
        public Action _onExecute;
        public RelayCommand(Action action)
        {
            _onExecute = action;
        }

        public event EventHandler CanExecuteChanged;
        
        /// <summary>
        /// Czy można wykonać
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        
        /// <summary>
        /// Wykonaj komende
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _onExecute?.Invoke();
        }
    }
}
