using System;
using System.Windows;
using PersonListApp.Models;
using PersonListApp.Tools;
using PersonListApp.Tools.Managers;

namespace PersonListApp.ViewModels
{
    internal class AddEditViewModel: BaseViewModel
    {
        #region Fields
        private readonly Person _person;
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _date = DateTime.Today;
        #endregion
        
        #region Commands
        private RelayCommand<object> _submitCommand;
        #endregion
        internal AddEditViewModel()
        {
        }

        internal AddEditViewModel(Person person)
        {
            _person = person;
            Name = person.FirstName;
            Surname = person.LastName;
            Email = person.Email;
            Date = person.Date;
        }

        #region Properties

        public Action Close { get; set; }
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand<object> SubmitCommand
        {
            get
            {
                return _submitCommand ?? (_submitCommand = new RelayCommand<object>(
                           SubmitI, o => CanExecuteCommand()));
            }
        }

        #endregion


        private void SubmitI(object obj)
        {
            try
            {
                if (_person != null)
                {
                    if (MessageBox.Show("Submit?", "Edit?",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
                    StationManager.DataStorage.EditUser(_person, new Person(Name, Surname, Email, Date));

                    Close();
                }
                else
                {
                    StationManager.DataStorage.AddUser(new Person(Name, Surname, Email, Date));
                    Close();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(_name) &&
                   !string.IsNullOrWhiteSpace(_surname) &&
                   !string.IsNullOrWhiteSpace(_email);
        }
    }
}
