using System.Collections.Generic;
using PersonListApp.Models;

namespace PersonListApp.Tools.DataStorage
{
    interface IDataStorage
    {
        void AddUser(Person person);
        void EditUser(Person p1, Person p2);
        void DeleteUser(Person person);
        void SaveChanges();
        List<Person> PersonsList { get; set; }
    }
}
