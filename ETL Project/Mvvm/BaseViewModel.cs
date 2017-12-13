using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Project.Mvvm
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty(ref string field, string value, 
            [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            field = value;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty(ref int field, int value,
            [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            field = value;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
