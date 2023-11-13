using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeleTraderAssignment.Command;
using TeleTraderAssignment.Database;
using TeleTraderAssignment.Models;

namespace TeleTraderAssignment.ViewModel
{
    public class SymbolManipulationViewModel : INotifyPropertyChanged
    {
        private Symbol symbol;
        private bool _isEdit;
        private NewDbContext _context;
        private bool isValid = true;

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (isValid != value)
                {
                    isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public Symbol Symbol
        {
            get { return symbol; }
            set
            {
                if (symbol != value)
                {
                    symbol = value;
                    OnPropertyChanged(nameof(Symbol));
                }
            }
        }

        private List<Models.Type> types;

        public List<Models.Type> Types
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

        private List<Exchange> exchanges;

        public List<Exchange> Exchanges
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

        private Models.Type selectedType;

        public Models.Type SelectedType
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

        private Exchange selectedExchange;

        public Exchange SelectedExchange
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

        //ovom prozoru se prosledjuje kontekst iz glavnog prozora gde se vec nalazi baza kako bi oba prozora koristila iste podatke
        //posto se ovaj prozor koristi i za add i za view/edit funkcionalnosti, prosledjujemo isEdit flag koji govori 
        //da li se radi o dodavanju ili azuriranju
        public SymbolManipulationViewModel(Symbol selectedSymbol, NewDbContext context, bool isEdit)
        {
            symbol = selectedSymbol ?? new Symbol();
            if(selectedSymbol == null)
            {
                Symbol.DateAdded = DateTime.Now;
            }
            else
            {
                SelectedType = Symbol.Type;
                SelectedExchange = Symbol.Exchange;
            }
            _context = context;
            _isEdit = isEdit;

            Types = _context.Type.ToList();
            Exchanges = _context.Exchange.ToList();
        }

        private ICommand saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get
            {
                if (saveChangesCommand == null)
                {
                    saveChangesCommand = new RelayCommand(ExecuteSaveChangesCommand, CanExecuteSaveChangesCommand);
                }
                return saveChangesCommand;
            }
        }

        //dodavanje i azuriranje obe zahtevaju cuvanje podataka u bazu pa se koristi ista metoda za taj rad
        //na osnovu flag-a odlucujemo koje konkretne korake preduzimamo u cuvanju podataka
        private void ExecuteSaveChangesCommand(object parameter)
        {
            //validacije kako bismo omogucili da se podaci dobrog formata upisuju u bazu
            if(!(SelectedExchange != null && SelectedType != null))
            {
                isValid = false;
                MessageBox.Show("You must select exchange and type to finish action!");
                return;
               
            }

            if (string.IsNullOrWhiteSpace(Symbol.Name) || string.IsNullOrWhiteSpace(Symbol.Ticker) ||
                string.IsNullOrWhiteSpace(Symbol.CurrencyCode) || string.IsNullOrWhiteSpace(Symbol.Isin))
            {
                isValid = false;
                MessageBox.Show("All text fields must be fullfilled!");
                return;
            }

            if(Symbol.Price <= 0 || Symbol.Price == null)
            {
                isValid = false;
                MessageBox.Show("Price cannot be null or negative!");
                return;
            }

            if(Symbol.DateAdded.Date == DateTime.MinValue.Date)
            {
                isValid = false;
                MessageBox.Show("Date must be valid!");
                return;
            }

            Symbol.Exchange = SelectedExchange;
            Symbol.ExchangeId = SelectedExchange.Id;
            Symbol.Type = SelectedType;
            Symbol.TypeId = SelectedType.Id;

            if (_isEdit)
            {
                _context.Symbol.Update(symbol);
            }
            else
            {
                _context.Symbol.Add(symbol);
            }
            _context.SaveChanges();

            MessageBox.Show("Operation successfully completed!");

            var window = parameter as Window;
            if (window != null)
            {
                var mainViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;
                mainViewModel.Refresh();
                window.Close();
            }
        }

        private bool CanExecuteSaveChangesCommand(object parameter)
        {
            return true;
        }

        // Implementacija INotifyPropertyChanged interfejsa
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
