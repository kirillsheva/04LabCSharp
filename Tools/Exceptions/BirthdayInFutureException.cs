using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonListApp.Tools.Exceptions
{
    class BirthdayInFutureException : Exception
    {
        public BirthdayInFutureException(string message) : base(message)
        {
        }
    }
}
