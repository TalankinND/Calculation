using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Расчёт_ЖКХ.Interfaces;

namespace Расчёт_ЖКХ.Impl
{
    internal class CalcImpl : ICalculation
    {
        public double carge(double V, double T)
        {
            return V * T;
        }

        public double counterV(double MPrev, double MCurr)
        {
            return MCurr - MPrev;
        }

        public double normV(double N, int n)
        {
            return n * N;
        }

        public double termEnV(double V, double N)
        {
            return V * N;
        }
    }
}
