using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeleTraderAssignment.Command;
using TeleTraderAssignment.Database;
using TeleTraderAssignment.Models;
using TeleTraderAssignment.Views;

namespace TeleTraderAssignment.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private NewDbContext _context;
        private List<Symbol> symbols;
        private List<Symbol> viewSymbols;
        private bool isDbLoaded = false;
        
        public bool IsDbLoaded
        {
            get { return isDbLoaded; }
            set
            {
                if (isDbLoaded != value)
                {
                    isDbLoaded = value;
                    OnPropertyChanged(nameof(IsDbLoaded));
                }
            }
        }

        public List<Symbol> Symbols
        {
            get { return symbols; }
            set
            {
                if (symbols != value)
                {
                    symbols = value;
                    OnPropertyChanged(nameof(Symbols));
                }
            }
        }

        public List<Symbol> ViewSymbols
        {
            get { return viewSymbols; }
            set
            {
                if (viewSymbols != value)
                {
                    viewSymbols = value;
                    OnPropertyChanged(nameof(ViewSymbols));
                }
            }
        }

        private List<string> types;

        public List<string> Types
        {
            get { return types; }
            set
            {
                if (types != value)
                {
                    types = value;
                    OnPropertyChanged(nameof(Types));
                }
            }
        }

        private List<string> exchanges;

        public List<string> Exchanges
        {
            get { return exchanges; }
            set
            {
                if (exchanges != value)
                {
                    exchanges = value;
                    OnPropertyChanged(nameof(Exchanges));
                }
            }
        }

        private string selectedType;

        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                }
            }
        }

        private string selectedExchange;

        public string SelectedExchange
        {
            get { return selectedExchange; }
            set
            {
                if (selectedExchange != value)
                {
                    selectedExchange = value;
                    OnPropertyChanged(nameof(SelectedExchange));
                }
            }
        }

        private Symbol selectedSymbol;

        public Symbol SelectedSymbol
        {
            get { return selectedSymbol; }
            set
            {
                if (selectedSymbol != value)
                {
                    selectedSymbol = value;
                    OnPropertyChanged(nameof(SelectedSymbol));
                }
            }
        }


        private ICommand loadFileCommand;

        public ICommand LoadFileCommand
        {
            get
            {
                if (loadFileCommand == null)
                {
                    loadFileCommand = new RelayCommand(ExecuteLoadFileCommand, CanExecuteLoadFileCommand);
                }
                return loadFileCommand;
            }
        }

        private void ExecuteLoadFileCommand(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "SQLite Database (*.s3db)|*.s3db|All Files (*.*)|*.*",
                Title = "Select a CSV File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                IsDbLoaded = true;
                _context = new NewDbContext(openFileDialog.FileName);
                LoadDataFromDB();
            }
        }

        private bool CanExecuteLoadFileCommand(object parameter)
        {
            return true;
        }

        public void Refresh()
        {
            LoadDataFromDB();
        }

        private void LoadDataFromDB()
        {
            if (IsDbLoaded)
            {
                Types = new List<string> { "All" };
                Exchanges = new List<string> { "All" };

                Symbols = _context.Symbol
                   .Include(s => s.Exchange)
                   .Include(s => s.Type)
                   .ToList();


                Types.AddRange(_context.Type.Select(e => e.Name).ToList());
                Exchanges.AddRange(_context.Exchange.Select(e => e.Name).ToList());
                ViewSymbols = Symbols;
            }
            else
            {
                Types = null;
                Exchanges = null;
                ViewSymbols = null;
                Symbols = null;
                SelectedExchange = null;
                SelectedType = null;
            }
            
        }

        private ICommand formCloseCommand;

        public ICommand FormCloseCommand
        {
            get
            {
                if (formCloseCommand == null)
                {
                    formCloseCommand = new RelayCommand(ExecuteFormCloseCommand, CanExecuteFormCloseCommand);
                }
                return formCloseCommand;
            }
        }

        private void ExecuteFormCloseCommand(object parameter)
        {
            if (!IsDbLoaded)
            {
                MessageBox.Show("Greška, Baza nije učitana!");
                return;
            }

            _context = new NewDbContext(null);
            IsDbLoaded = false;
            LoadDataFromDB();
        }

        private bool CanExecuteFormCloseCommand(object parameter)
        {
            if (IsDbLoaded)
            {
                return true;
            }
            return false;
        }

        // Implementacija INotifyPropertyChanged interfejsa
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
