using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Transaction
    {
        public string payer { get; set; }
        public string receiver { get; set; }
        public double amount { get; set; }
        public Transaction(string p, string r, double a)
        {
            payer = p;
            receiver = r;
            amount = a;
        }
    }
}
