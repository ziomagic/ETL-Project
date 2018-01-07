using ETL_Project.Models;
using ETL_Project.Mvvm;
using ETL_Project.Pipeline;
using ETL_Project.Utils;
using System.Collections.ObjectModel;
using ServiceStack.OrmLite;
using System.Windows;
using System.Windows.Input;

namespace ETL_Project.ViewModel
{
    public class PreviewWindowViewModel : BindableBase
    {
        #region Fields
        #endregion

        public PreviewWindowViewModel() : base()
        {
            LoadReviews();
        }

        #region Properties
        private ObservableCollection<Review> _reviews = new ObservableCollection<Review>();
        public ObservableCollection<Review> Reviews
        {
            get { return _reviews; }
            set
            {
                SetProperty(ref _reviews, value);
            }
        }

        private ICommand _saveToCsvCommand;
        public ICommand SaveToCsvCommand
        {
            get
            {
                if (_saveToCsvCommand == null)
                {
                    _saveToCsvCommand = new RelayCommand(async () =>
                    {
                        // Save to csv
                    });
                }

                return _saveToCsvCommand;
            }
        }

        private ICommand _saveToSeparateFileCommand;
        public ICommand SaveToSeparateFileCommand
        {
            get
            {
                if (_saveToSeparateFileCommand == null)
                {
                    _saveToSeparateFileCommand = new RelayCommand(async () =>
                    {
                        // Save to csv
                    });
                }

                return _saveToSeparateFileCommand;
            }
        }
        #endregion

        #region Methods [private]
        private void LoadReviews()
        {
            var dbFactory = DbManager.GetDbFactory();
            using (var db = dbFactory.Open())
            {
                Reviews = new ObservableCollection<Review>(db.Select<Review>());
            }
        }
        #endregion
    }
}
