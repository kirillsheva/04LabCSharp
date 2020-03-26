using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using PersonListApp.Tools.Exceptions;

namespace PersonListApp.Models
{
    [Serializable]
    public class Person
    {
        #region Fields

        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly DateTime _date;

        private readonly bool _isAdult;
        private readonly bool _isBirthday;
        private readonly string _westernZodiac;
        private readonly string _chineseZodiac;

        #endregion

        #region Properties

        public string FirstName => _firstName;

        public string LastName => _lastName;

        public string Email => _email;

        public DateTime Date => _date;


        public bool IsAdult
        {
            get => _isAdult;
            set => throw new NotImplementedException();
        }

        public bool IsBirthday
        {
            get => _isBirthday;
            set => throw new NotImplementedException();
        }

        public string WesternZodiac
        {
            get => _westernZodiac;
            set => throw new NotImplementedException();
        }

        public string ChineseZodiac
        {
            get => _chineseZodiac;
            set => throw new NotImplementedException();
        }
        #endregion

        #region Constructors
        internal Person(string firstName, string lastName, string email, DateTime date)
        {
            try
            {
                _firstName = firstName;
                _lastName = lastName;
   
                if (new EmailAddressAttribute().IsValid(email))
                {
                    _email = email;
                }
                else
                {
                    throw new InvalidEmailException("Incorrect Email!");
                }
                _date = date;
                _isAdult = (ComputeAge() >= 18);
                _isBirthday = CheckForBirthday();
                _westernZodiac = ComputeWesternZodiac();
                _chineseZodiac = ChooseChineseAnimal();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public Person(string firstName, string lastName, string email) :
            this(firstName, lastName, email, DateTime.Today)
        {
        }

        public Person(string firstName, string lastName, DateTime date) :
            this(firstName, lastName, "default", date)
        {
        }
        #endregion

        private int ComputeAge()
        {
            DateTime atTheMoment = DateTime.Today;
            int resultAge = atTheMoment.Year - Date.Year;

            if (((atTheMoment.Month == Date.Month) && (Date.Day > atTheMoment.Day)) || Date.Month > atTheMoment.Month)
            {
                resultAge--;
            }

            if (resultAge < 0)
            {
                throw new BirthdayInFutureException("Error! Invalid birthday date!");
            }
            if (resultAge > 135)
            {
                throw new TooOldException("Error! You picked irrelevant date.(Person is probably dead)");
            }
            return resultAge;
        }

        private bool CheckForBirthday()
        {
            return Date.Day == DateTime.Today.Day && Date.Month == DateTime.Today.Month;
        }

        private string ComputeWesternZodiac()
        {
            string res = "";
            if ((Date.Month == 1 && Date.Day >= 21) || (Date.Month == 2 && Date.Day <= 20))
                res += "Aquarius";
            if ((Date.Month == 2 && Date.Day >= 21) || (Date.Month == 3 && Date.Day <= 20))
                res += "Pisces";
            if ((Date.Month == 3 && Date.Day >= 21) || (Date.Month == 4 && Date.Day <= 20))
                res += "Aries";
            if ((Date.Month == 4 && Date.Day >= 21) || (Date.Month == 5 && Date.Day <= 20))
                res += "Taurus";
            if ((Date.Month == 5 && Date.Day >= 21) || (Date.Month == 6 && Date.Day <= 21))
                res += "Gemini";
            if ((Date.Month == 6 && Date.Day >= 22) || (Date.Month == 7 && Date.Day <= 22))
                res += "Cancer";
            if ((Date.Month == 7 && Date.Day >= 23) || (Date.Month == 8 && Date.Day <= 23))
                res += "Leo";
            if ((Date.Month == 8 && Date.Day >= 24) || (Date.Month == 9 && Date.Day <= 23))
                res += "Virgo";
            if ((Date.Month == 9 && Date.Day >= 24) || (Date.Month == 10 && Date.Day <= 22))
                res += "Libra";
            if ((Date.Month == 10 && Date.Day >= 23) || (Date.Month == 11 && Date.Day <= 22))
                res += "Scorpio";
            if ((Date.Month == 11 && Date.Day >= 23) || (Date.Month == 12 && Date.Day <= 21))
                res += "Sagittarius";
            if ((Date.Month == 12 && Date.Day >= 22) || (Date.Month == 1 && Date.Day <= 20))
                res += "Capricorn";
            return res;
        }


        private string ChooseChineseAnimal()
        {
            int year = Date.Year % 12;
            switch (year)
            {
                case 1:
                    return "Rooster";
                case 2:
                    return "Dog";
                case 3:
                    return "Pig";
                case 4:
                    return "Rat";
                case 5:
                    return "Ox";
                case 6:
                    return "Tiger";
                case 7:
                    return "Rabbit";
                case 8:
                    return "Dragon";
                case 9:
                    return "Snake";
                case 10:
                    return "Horse";
                case 11:
                    return "Goat";
                default:
                    return "Monkey";
            }
        }
    }
}
