using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Расчёт_ЖКХ.Interfaces
{
    internal interface ICalculation
    {
        double carge(double V, double T);
        double counterV(double MPrev, double MCurr);
        double normV(double N, int n);
        double termEnV(double V, double N);
    }
}
