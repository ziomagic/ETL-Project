using ETL_Project.Mvvm;
using ETL_Project.Pipeline;
using ETL_Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ETL_Project.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields
        private IPipelineOperation _extractOperation;
        private IPipelineOperation _transformOperation;
        private IPipelineOperation _loadOperation;

        private List<IPipelineOperation> _etlOperations;
        private object _cachedOperationResult;
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

            OutputList.Add("Starting..");
            Logger.NewLogAppeared += (sender, msg) => Application.Current.Dispatcher.Invoke(() =>
            {
                OutputList.Add(msg);
                OnPropertyChanged("TabIndex");
            });
        }

        #region Properties
        private bool _canExtract = true;
        public bool CanExtract
        {
            get { return _canExtract; }
            set { SetProperty(ref _canExtract, value); }
        }

        private bool _canTransform;
        public bool CanTransform
        {
            get { return _canTransform; }
            set { SetProperty(ref _canTransform, value); }
        }

        private bool _canLoad;
        public bool CanLoad
        {
            get { return _canLoad; }
            set { SetProperty(ref _canLoad, value); }
        }

        private ICommand _extractCommand;
        public ICommand ExtractCommand
        {
            get
            {
                if (_extractCommand == null)
                {
                    _extractCommand = new RelayCommand(async () =>
                    {
                        if (!ValidateProductNumber(ProductNumber))
                        {
                            MessageBox.Show("Please provide product number.");
                            return;
                        }

                        CanExtract = false;
                        var dispatcher = Application.Current.Dispatcher;
                        await Task.Run(() =>
                        {
                            _cachedOperationResult = _extractOperation.HandleData(ProductNumber);
                            dispatcher.Invoke(() =>
                            {
                                CanExtract = true;
                                CanTransform = true;
                            });
                        });
                    });
                }

                return _extractCommand;
            }
        }

        private ICommand _transformCommand;
        public ICommand TransformCommand
        {
            get
            {
                if (_transformCommand == null)
                {
                    _transformCommand = new RelayCommand(async () =>
                    {
                        CanTransform = false;
                        CanExtract = false;
                        CanTransform = false;
                        var dispatcher = Application.Current.Dispatcher;
                        await Task.Run(() =>
                        {
                            _cachedOperationResult = _transformOperation.HandleData(_cachedOperationResult);
                            dispatcher.Invoke(() =>
                            {
                                CanLoad = true;
                                CanExtract = true;
                            });
                        });
                    });
                }

                return _transformCommand;
            }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new RelayCommand(async () =>
                    {
                        CanLoad = false;
                        CanExtract = false;
                        CanTransform = false;
                        var dispatcher = Application.Current.Dispatcher;
                        await Task.Run(() =>
                        {
                            _cachedOperationResult = _loadOperation.HandleData(_cachedOperationResult);
                            dispatcher.Invoke(() => CanExtract = true);
                        });
                    });
                }

                return _loadCommand;
            }
        }

        private ICommand _etlCommand;
        public ICommand EtlCommand
        {
            get
            {
                if (_etlCommand == null)
                {
                    _etlCommand = new RelayCommand(async () =>
                    {
                        if (!ValidateProductNumber(ProductNumber))
                        {
                            MessageBox.Show("Please provide product number.");
                            return;
                        }

                        CanTransform = false;
                        CanLoad = false;
                        CanExtract = false;
                        await Task.Run(() => ExecuteEtl(ProductNumber));
                        CanExtract = true;
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

        private ICommand _showPreviewCommand;
        public ICommand ShowPreviewCommand
        {
            get
            {
                if (_showPreviewCommand == null)
                {
                    _showPreviewCommand = new RelayCommand(() =>
                    {
                        var window = new PreviewWindow();
                        window.Show();
                    });
                };

                return _showPreviewCommand;
            }
        }

        private string _productNumber = "30688215";
        public string ProductNumber
        {
            get { return _productNumber; }
            set
            {
                SetProperty(ref _productNumber, value);
            }
        }

        public int TabIndex
        {
            get { return OutputList.Count - 1; }
        }

        private ObservableCollection<string> _outputList = new ObservableCollection<string>();
        public ObservableCollection<string> OutputList
        {
            get { return _outputList; }
            set
            {
                SetProperty(ref _outputList, value);
            }
        }
        #endregion

        #region Methods [private/protected]
        private void ExecuteEtl(object input)
        {
            try
            {
                foreach (var operation in _etlOperations)
                {
                    input = operation.HandleData(input);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can no parse this request.");
            }
        }

        private bool ValidateProductNumber(string prodNumber)
        {
            return int.TryParse(prodNumber, out int result);
        }

        private void ClearDb()
        {
            var path = DbManager.GetDbPath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            MessageBox.Show("Database cleared.");
        }
        #endregion
    }
}
