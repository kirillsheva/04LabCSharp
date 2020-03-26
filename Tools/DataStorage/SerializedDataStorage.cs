using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PersonListApp.Models;
using PersonListApp.Tools.Managers;

namespace PersonListApp.Tools.DataStorage
{
    class SerializedDataStorage : IDataStorage
    {
        private List<Person> _persons;
        private readonly string[] _names = { "Glad", "Petro", "Jose", "Papich", "Maga", "Rafik", "Jorik", "Joao", "Vovan", "Arsen", "Mykhailo", "Max", "Glek", "Vasya", "Oleg", "Slavon", "Dimas", "Danyla", "Artem", "John" };
        private readonly string[] _surnames = { "Valakas", "Poroshenko", "Mourinho", "Papich", "Vartanov", "Groysman", "Yanukovich", "Apple", "Yatseniuk", "Belka", "Mudakoff", "Ukrainec", "Patriot", "Yuschenko", "Tron", "Glybovets", "Petrosyan", "Makarov", "Kolbaskin" };

        internal SerializedDataStorage()
        {
            try
            {
                _persons = SerializationManager.Deserialize<List<Person>>(FileFolderHelper.StorageFilePath);
            }
            catch (FileNotFoundException)
            {
                _persons = new List<Person>();
                Random rand = new Random();
                for (int i = 0; i < 50; i++)
                {
                    AddUser(new Person($"{_names[(rand.Next(1, 20))]}",
                        $"{_surnames[(rand.Next(1, 19))]}",
                        $"{_surnames[(rand.Next(1, 19))]}@{_names[(rand.Next(1, 20))]}.com",
                        new DateTime(rand.Next(1920, 2019), rand.Next(1, 12), rand.Next(1, 30))));
                }

                SaveChanges();
            }
        }

      
        public List<Person> PersonsList
        {
            get => _persons.ToList();
            set => _persons = value;
        }

        public void EditUser(Person p1, Person p2)
        {
            if (CanAddOrChange(p2))
                _persons[_persons.IndexOf(p1)] = p2;
            else throw new Exception();
        }

        public void SaveChanges()
        {
            SerializationManager.Serialize(_persons, FileFolderHelper.StorageFilePath);
        }

        public void AddUser(Person person)
        {
            if (CanAddOrChange(person))
            {
                _persons.Add(person);
                SaveChanges();
            }
            else throw new Exception();
        }

        public void DeleteUser(Person person)
        {
            _persons.Remove(person);
            SaveChanges();
        }

        private bool CanAddOrChange(Person p)
        {
            return !string.IsNullOrWhiteSpace(p.FirstName) && !string.IsNullOrWhiteSpace(p.LastName) && !string.IsNullOrWhiteSpace(p.Email);
        }
    }
}
