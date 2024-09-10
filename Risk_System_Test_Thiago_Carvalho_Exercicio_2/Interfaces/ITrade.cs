using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_System_Test_Thiago_Carvalho_Exercicio_2.Interfaces
{
    public interface ITrade
    {
        double Value { get; }
        string ClientSector { get; } 
        DateTime NextPaymentDate { get; } 
        bool IsPoliticallyExposed { get; }
    }
}
