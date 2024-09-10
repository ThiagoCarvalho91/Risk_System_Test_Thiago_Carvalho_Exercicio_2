using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Risk_System_Test_Thiago_Carvalho_Exercicio_2.Interfaces;

namespace Risk_System_Test_Thiago_Carvalho_Exercicio_2.Models
{
    public class Trade : ITrade
    {
        public double Value { get; }
        public string ClientSector { get; }
        public DateTime NextPaymentDate { get; }
        public bool IsPoliticallyExposed { get; }

        public Trade(double value, string sector, DateTime nextPaymentDate, bool isPoliticallyExposed)
        {
            Value = value;
            ClientSector = sector;
            NextPaymentDate = nextPaymentDate;
            IsPoliticallyExposed = isPoliticallyExposed;
        }
    }
}

