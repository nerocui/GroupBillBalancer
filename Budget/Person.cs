using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Person
    {
        public string name { get; set; }
        public double amount { get; set; }

        public Person(string n, double i)
        {
            name = n;
            amount = i;
        }

    }
}
