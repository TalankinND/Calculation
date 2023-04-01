using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Расчёт_ЖКХ.Model
{
    internal class CalculationModel
    {
        public int _id { get; set; }
        public double HVS { get; set; }
        public double GVSTN { get; set; }
        public double GVSTE { get; set; }
        public double EE { get; set; }
        public double HM { get; set; }
        public double GTNM { get; set; }
        public double GTEM { get; set; }
        public double EENM { get; set; }
        public double EEDM { get; set; }
        public DateOnly date { get; set; }

        public override string ToString()
        {
            return string.Format("ХВС: {0:#.##}\nГВСТН: {1:#.##}\nГВСТЕ: {1:#.##}\nЭЭ: {2:#.##}\nДата: {3}", HVS, GVSTN, GVSTE, EE, date);
        }
    }
}
