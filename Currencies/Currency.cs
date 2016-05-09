using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currencies
{
    public class Currency
    {
        public string Base { get; set; }
        public string Date { get; set; }

        public CurrencyRates Rates { get; set; }

    }
}
