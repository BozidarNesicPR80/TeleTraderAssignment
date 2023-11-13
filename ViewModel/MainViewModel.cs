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

        private ICommand filterSymbolsCommand;

        public ICommand FilterSymbolsCommand
        {
            get
            {
                if (filterSymbolsCommand == null)
                {
                    filterSymbolsCommand = new RelayCommand(ExecuteFilterSymbolsCommand, CanExecuteFilterSymbolsCommand);
                }
                return filterSymbolsCommand;
            }
        }

        private void ExecuteFilterSymbolsCommand(object parameter)
        {
            if (!IsDbLoaded)
            {
                MessageBox.Show("Greska, Baza nije ucitana!");
                return;
            }

            string selectedType = SelectedType;
            string selectedExchange = SelectedExchange;

            List<Symbol> filteredSymbols = symbols.FindAll(symbol =>
            {
                bool typeCondition = selectedType == "All" || symbol.Type.Name == selectedType;
                bool exchangeCondition = selectedExchange == "All" || symbol.Exchange.Name == selectedExchange;

                return typeCondition && exchangeCondition;
            });

            ViewSymbols = filteredSymbols;
        }

        private bool CanExecuteFilterSymbolsCommand(object parameter)
        {
            if (IsDbLoaded)
            {
                return true;
            }
            return false;
        }


        private ICommand viewEditSymbolCommand;

        public ICommand ViewEditSymbolCommand
        {
            get
            {
                if (viewEditSymbolCommand == null)
                {
                    viewEditSymbolCommand = new RelayCommand(ExecuteViewEditSymbolsCommand, CanExecuteViewEditSymbolsCommand);
                }
                return viewEditSymbolCommand;
            }
        }

        private void ExecuteViewEditSymbolsCommand(object parameter)
        {
            if (!IsDbLoaded)
            {
                MessageBox.Show("Greska, Baza nije ucitana!");
                return;
            }

            if (SelectedSymbol != null)
            {
                SymbolManipulationViewModel symbolDetailsViewModel = new SymbolManipulationViewModel(selectedSymbol, _context, true);
                SymbolManipulationWindow symbolDetailsWindow = new SymbolManipulationWindow(symbolDetailsViewModel);
                symbolDetailsWindow.ShowDialog();
            }
        }

        private bool CanExecuteViewEditSymbolsCommand(object parameter)
        {
            if (IsDbLoaded)
            {
                return true;
            }
            return false;
        }

        private ICommand addSymbolCommand;

        public ICommand AddSymbolCommand
        {
            get
            {
                if (addSymbolCommand == null)
                {
                    addSymbolCommand = new RelayCommand(ExecuteAddSymbolsCommand, CanExecuteAddSymbolsCommand);
                }
                return addSymbolCommand;
            }
        }

        private void ExecuteAddSymbolsCommand(object parameter)
        {
            if (!IsDbLoaded)
            {
                MessageBox.Show("Greska, Baza nije ucitana!");
                return;
            }

            SymbolManipulationViewModel symbolDetailsViewModel = new SymbolManipulationViewModel(null, _context, false);
            SymbolManipulationWindow symbolDetailsWindow = new SymbolManipulationWindow(symbolDetailsViewModel);
            symbolDetailsWindow.ShowDialog();
        }

        private bool CanExecuteAddSymbolsCommand(object parameter)
        {
            if (IsDbLoaded)
            {
                return true;
            }
            return false;
        }

        private ICommand deleteSymbolCommand;

        public ICommand DeleteSymbolCommand
        {
            get
            {
                if (deleteSymbolCommand == null)
                {
                    deleteSymbolCommand = new RelayCommand(ExecuteDeleteSymbolCommand, CanExecuteDeleteSymbolCommand);
                }
                return deleteSymbolCommand;
            }
        }

        private void ExecuteDeleteSymbolCommand(object parameter)
        {
            if (!IsDbLoaded)
            {
                MessageBox.Show("Greska, Baza nije ucitana!");
                return;
            }

            if (SelectedSymbol != null)
            {
                // Pitajte korisnika da potvrdi brisanje
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj red?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Obrišite odabrani red iz baze podataka ili liste, zavisno od vašeg scenarija
                    _context.Symbol.Remove(selectedSymbol);
                    _context.SaveChanges();

                    // Ponovo učitajte podatke kako biste osvežili DataGrid
                    LoadDataFromDB();

                    // Resetujte trenutno odabrani red
                    selectedSymbol = null;
                }
            }
        }

        private bool CanExecuteDeleteSymbolCommand(object parameter)
        {
            if (IsDbLoaded)
            {
                return true;
            }
            return false;
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
