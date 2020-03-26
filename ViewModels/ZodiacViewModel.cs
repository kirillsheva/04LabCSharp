using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PersonListApp.Models;
using PersonListApp.Tools;
using System.Linq;
using PersonListApp.Tools.Managers;

namespace PersonListApp.ViewModels
{
    internal class ZodiacViewModel : BaseViewModel, ILoaderOwner
    {
        #region Fields

        private ObservableCollection<Person> _persons;
        private Person _selectedPerson;

        #region Commands
        private RelayCommand<object> _editPerson;
        private RelayCommand<object> _addPerson;
        private RelayCommand<object> _deletePerson;
        private RelayCommand<object> _saveAll;
        private RelayCommand<object> _sortFirstName;
        private RelayCommand<object> _sortLastName;
        private RelayCommand<object> _sortEmail;
        private RelayCommand<object> _sortBirthday;
        private RelayCommand<object> _sortWestern;
        private RelayCommand<object> _sortChinese;
        private RelayCommand<object> _sortIsAdult;
        private RelayCommand<object> _sortIsBirthday;
        #endregion

        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;


        #endregion

        #region Properties

        internal ZodiacViewModel()
        {
            LoaderManager.Instance.Initialize(this);
            LoaderManager.Instance.ShowLoader();
            _persons = new ObservableCollection<Person>(StationManager.DataStorage.PersonsList);
            LoaderManager.Instance.HideLoader();
        }

        public ObservableCollection<Person> Persons
        {
            get => _persons;
            private set
            {
                _persons = value;
                OnPropertyChanged();
            }
        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set => _selectedPerson = value;
        }

        #region Commands

        public RelayCommand<object> Add =>
            _addPerson ?? (_addPerson = new RelayCommand<object>(
                AddI));

        public RelayCommand<object> Save =>
            _saveAll ?? (_saveAll = new RelayCommand<object>(
                SaveI));

        public RelayCommand<object> Delete
        {
            get
            {
                return _deletePerson ?? (_deletePerson = new RelayCommand<object>(
                           DeleteI, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> Edit
        {
            get
            {
                return _editPerson ?? (_editPerson = new RelayCommand<object>(
                           EditImplementation, o => CanExecuteCommand()));
            }
        }
        public RelayCommand<object> SortFirstName
        {
            get
            {
                return _sortFirstName ?? (_sortFirstName = new RelayCommand<object>(o =>
                    Sort(1)));
            }
        }
        public RelayCommand<object> SortLastName
        {
            get
            {
                return _sortLastName ?? (_sortLastName = new RelayCommand<object>(o =>
                           Sort(2)));
            }
        }
        public RelayCommand<object> SortEmail
        {
            get
            {
                return _sortEmail ?? (_sortEmail = new RelayCommand<object>(o =>
                           Sort(3)));
            }
        }
        public RelayCommand<object> SortBirthday
        {
            get
            {
                return _sortBirthday ?? (_sortBirthday = new RelayCommand<object>(o =>
                           Sort(4)));
            }
        }
        public RelayCommand<object> SortWestern
        {
            get
            {
                return _sortWestern ?? (_sortWestern = new RelayCommand<object>(o =>
                           Sort(5)));
            }
        }
        public RelayCommand<object> SortChinese
        {
            get
            {
                return _sortChinese ?? (_sortChinese = new RelayCommand<object>(o =>
                           Sort(6)));
            }
        }
        public RelayCommand<object> SortIsAdult
        {
            get
            {
                return _sortIsAdult ?? (_sortIsAdult = new RelayCommand<object>(o =>
                           Sort(7)));
            }
        }
        public RelayCommand<object> SortIsBirthday
        {
            get
            {
                return _sortIsBirthday ?? (_sortIsBirthday = new RelayCommand<object>(o =>
                           Sort(8)));
            }
        }
        #endregion

        #endregion

        #region Implementations


        private static async void SaveI(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                StationManager.DataStorage.SaveChanges();
                Thread.Sleep(150);
            });
            LoaderManager.Instance.HideLoader();
        }

        private async void Sort(int i)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                IOrderedEnumerable<Person> sortedPersons;
                switch (i)
                {
                    case 1:
                        sortedPersons = from u in _persons
                                        orderby u.FirstName
                                        select u;
                        break;
                    case 2:
                        sortedPersons = from u in _persons
                                        orderby u.LastName
                                        select u;
                        break;
                    case 3:
                        sortedPersons = from u in _persons
                                        orderby u.Email
                                        select u;
                        break;
                    case 4:
                        sortedPersons = from u in _persons
                                        orderby u.Date
                                        select u;
                        break;
                    case 5:
                        sortedPersons = from u in _persons
                                        orderby u.WesternZodiac
                                        select u;
                        break;
                    case 6:
                        sortedPersons = from u in _persons
                                        orderby u.ChineseZodiac
                                        select u;
                        break;
                    case 7:
                        sortedPersons = from u in _persons
                                        orderby u.IsAdult
                                        select u;
                        break;
                    default:
                        sortedPersons = from u in _persons
                                        orderby u.IsBirthday
                                        select u;
                        break;
                }
                Persons = new ObservableCollection<Person>(sortedPersons);
                StationManager.DataStorage.PersonsList = Persons.ToList();
                Thread.Sleep(300);
            });
            LoaderManager.Instance.HideLoader();
        }

        private async void DeleteI(object obj)
        {
            await Task.Run(() =>
            {
                if (MessageBox.Show("Delete selected person?",
                        "Delete?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
                StationManager.DataStorage.DeleteUser(_selectedPerson);
                _selectedPerson = null;
                Persons = new ObservableCollection<Person>(StationManager.DataStorage.PersonsList);
            });
        }


        private void AddI(object obj)
        {
            IsControlEnabled = false;
            AddEditWindow window = new AddEditWindow();
            window.ShowDialog();
            IsControlEnabled = true;
            Persons = new ObservableCollection<Person>(StationManager.DataStorage.PersonsList);
        }

        private void EditImplementation(object obj)
        {
            IsControlEnabled = false;
            AddEditWindow window = new AddEditWindow(_selectedPerson);
            window.ShowDialog();
            IsControlEnabled = true;
            Persons = new ObservableCollection<Person>(StationManager.DataStorage.PersonsList);
        }

        #endregion

        private bool CanExecuteCommand()
        {
            return _selectedPerson != null;
        }
        public Visibility LoaderVisibility
        {
            get => _loaderVisibility;
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public bool IsControlEnabled
        {
            get => _isControlEnabled;
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
