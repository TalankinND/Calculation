using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Расчёт_ЖКХ.Model;

namespace Расчёт_ЖКХ.Interfaces
{
    interface IRepository
    {
        List<CalculationModel> GetAllData(SqliteConnection con);
        void AddData(double X, double GTN, double GTE, double E, double HM, double GTNM, double GTEM, double EENM, double EEDM, SqliteConnection con);
        void CreateTable(SqliteConnection con);
    }
}