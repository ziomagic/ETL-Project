using ETL_Project.Mvvm;
using ETL_Project.Pipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ETL_Project.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields
        private IPipelineOperation _extractOperation;
        private IPipelineOperation _transformOperation;
        private IPipelineOperation _loadOperation;

        private List<IPipelineOperation> _etlOperations;
        #endregion

        public MainWindowViewModel() : base()
        {
            _extractOperation = new ExtractOperation();
            _transformOperation = new TransformOperation();
            _loadOperation = new LoadOperation();

            _etlOperations = new List<IPipelineOperation>
            {
                _extractOperation,
                _transformOperation,
                _loadOperation
            };
        }

        #region Properties
        private ICommand _etlCommand;
        public ICommand EtlCommand
        {
            get
            {
                if (_etlCommand == null)
                {
                    _etlCommand = new RelayCommand(() =>
                    {
                        ExecuteEtl(Url);
                    });
                }

                return _etlCommand;
            }
        }

        private ICommand _clearDbCommand;
        public ICommand ClearDbCommand
        {
            get
            {
                if (_clearDbCommand == null)
                {
                    _clearDbCommand = new RelayCommand(() =>
                    {
                        ClearDb();
                    });
                }

                return _clearDbCommand;
            }
        }

        private string _url = "https://www.ceneo.pl/30688215#tab=reviews";
        public string Url
        {
            get { return _url; }
            set
            {
                SetProperty(ref _url, value);
            }
        }
        #endregion

        #region Methods [private/protected]
        private void ExecuteEtl(object input)
        {
            foreach(var operation in _etlOperations)
            {
                input = operation.HandleData(input);
            }
        }

        private void ClearDb()
        {
            var path = LoadOperation.GetDbPath();
            if(File.Exists(path))
            {
                File.Delete(path);
            }

            MessageBox.Show("Database cleared.");
        }
        #endregion
    }
}
