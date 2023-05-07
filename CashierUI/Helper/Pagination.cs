using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CashierUI.Helper
{
    public class Pagination : NotifyDataError, INotifyPropertyChanged
    {
        private readonly Action _naayUpdate;
        private int _totalPages;
        private string _enteredPage = "0";
        private int _currentPage = 0;

        public Pagination(Action naayUpdate)
        {
            _naayUpdate = naayUpdate;
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPagesFormatted));
                OnPropertyChanged(nameof(EnteredPage));

            }
        }

        public string TotalPagesFormatted
        {
            get { return $"of {TotalPages:N0}"; }
        }
        public ObservableCollection<string> InputErrors { get; set; } = new ObservableCollection<string>();

        public string EnteredPage
        {
            get => _enteredPage;
            set
            {
                _enteredPage = value;
                var successParse = int.TryParse(value, out int result);
                if (successParse)
                {
                    _naayUpdate();
                    ClearErrors(nameof(EnteredPage));
                }
                else
                {
                    SetErrors(nameof(EnteredPage), new List<string>
                    {
                        "Invalid number format"
                    });
                    InputErrors.Add("Invalid number format");
                }
                OnPropertyChanged(nameof(EnteredPage));
            }

        }

        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = value;
        }

        public int ItemsPerPage { get; set; } = 50;


        public void NextPage()
        {
            if (CurrentPage == TotalPages) return;
            CurrentPage++;
            EnteredPage = CurrentPage.ToString();
            OnPropertyChanged(nameof(EnteredPage));
        }
        public void LastPage()
        {
            if (CurrentPage == TotalPages) return;
            CurrentPage = TotalPages;
            EnteredPage = CurrentPage.ToString();
            OnPropertyChanged(nameof(EnteredPage));
        }

        public void PrevPage()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            EnteredPage = CurrentPage.ToString();

            OnPropertyChanged(nameof(EnteredPage));
        }
        public void FirstPage()
        {
            if (CurrentPage >= 1) return;
            CurrentPage = 1;
            EnteredPage = CurrentPage.ToString();
            OnPropertyChanged(nameof(EnteredPage));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
