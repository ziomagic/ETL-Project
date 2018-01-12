using ETL_Project.Models;
using ETL_Project.Mvvm;
using ETL_Project.Pipeline;
using ETL_Project.Utils;
using System.Collections.ObjectModel;
using ServiceStack.OrmLite;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace ETL_Project.ViewModel
{
    public class PreviewWindowViewModel : BindableBase
    {
        #region Fields
        private const string ReviewFilePath = "reviews.csv";
        private const string ReviewsDirectory = "reviews/";
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
                        using (var reader = File.CreateText(ReviewFilePath))
                        {
                            using (var csv = new CsvHelper.CsvWriter(reader))
                            {
                                csv.WriteRecords<Review>(Reviews);
                                
                            }
                        }
                        MessageBox.Show("Reviews saved to " + ReviewFilePath);
                    });
                }

                return _saveToCsvCommand;
            }
        }

        private ICommand _saveToSeparateFileCommand;
        public ICommand SaveToFilesCommand
        {
            get
            {
                if (_saveToSeparateFileCommand == null)
                {
                    _saveToSeparateFileCommand = new RelayCommand(() =>
                    {
                        Directory.CreateDirectory(ReviewsDirectory);
                        var i = 0;
                        foreach (var review in Reviews)
                        {
                            File.WriteAllText($"{ReviewsDirectory}_{i++}_{review.Author}.txt", review.Comment);
                        }
                        MessageBox.Show("Reviews saved to directory " + ReviewsDirectory);
                    });
                }

                return _saveToSeparateFileCommand;
            }
        }
        #endregion

        #region Methods [private]
        /// <summary>
        /// Metoda wczytująca recenzję z bazy danych
        /// </summary>
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
