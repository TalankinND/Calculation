using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Расчёт_ЖКХ.Interfaces;
using Microsoft.Data.Sqlite;
using Расчёт_ЖКХ.Model;

namespace Расчёт_ЖКХ.Impl
{
    internal class Repository : IRepository
    {
        public void AddData(double X, double GTN, double GTE, double E, double HM, double GTNM, double GTEM, double EENM, double EEDM, SqliteConnection con)
        {
            String sqlExpression = string.Format("INSERT INTO Calculation (HVS, GVSTN, GVSTE, EE, HM, GTNM, GTEM, EENM, EEDM) VALUES ('{0:#.##}', '{1:#.##}', '{2:#.##}', '{3:#.##}', '{4:#.##}', '{5:#.##}', '{6:#.##}', '{7:#.##}', '{8:#.##}')", X, GTN, GTE, E, HM, GTNM, GTEM, EENM, EEDM);
            var command = new SqliteCommand(sqlExpression, con);
            command.ExecuteNonQuery();
        }

        public void CreateTable(SqliteConnection con)
        {
            String sqlExpression = "create table if not exists Calculation(_id integer primary key autoincrement not null, HVS text, GVSTN text, GVSTE text, EE text, HM text, GTNM text, GTEM text, EENM text, EEDM text)";
            var command = new SqliteCommand(sqlExpression, con);
            command.ExecuteNonQuery();
        }

        public List<CalculationModel> GetAllData(SqliteConnection con)
        {
            List<CalculationModel> list = new List<CalculationModel>();

            String sqlExpression = "SELECT * FROM Calculation";
            var command = new SqliteCommand(sqlExpression, con);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        double h = 0.0;
                        if (!Double.TryParse(reader.GetString(5), out h)) { 
                            h = 0.0;
                        }

                        double gtn = 0.0;
                        if (!Double.TryParse(reader.GetString(6), out gtn))
                        {
                            gtn = 0.0;
                        }

                        double gte = 0.0;
                        if (!Double.TryParse(reader.GetString(7), out gte))
                        {
                            gte = 0.0;
                        }

                        double eN = 0.0;
                        if (!Double.TryParse(reader.GetString(8), out eN))
                        {
                            eN = 0.0;
                        }

                        double eD = 0.0;
                        if (!Double.TryParse(reader.GetString(9), out eD))
                        {
                            eD = 0.0;
                        }

                        CalculationModel model = new CalculationModel() {
                            _id = reader.GetInt32(0),
                            HVS = Double.Parse(reader.GetString(1)),
                            GVSTN = Double.Parse(reader.GetString(2)),
                            GVSTE = Double.Parse(reader.GetString(3)),
                            EE = Double.Parse(reader.GetString(4)),
                            HM = h,
                            GTNM = gtn,
                            GTEM = gte,
                            EENM = eN,
                            EEDM = eD,
                        };

                        list.Add(model);
                    }
                }
            }

            return list;
        }
    }
}
