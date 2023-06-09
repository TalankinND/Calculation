﻿using System;
using Microsoft.Data.Sqlite;
using Расчёт_ЖКХ.Impl;
using Расчёт_ЖКХ.Interfaces;
using Расчёт_ЖКХ.Model;

namespace Расчёт_ЖКХ
{
    class Programm
    {

        const double HT = 35.78;
        const double HN = 4.85;
        const double ET = 4.28;
        const double EN = 164;
        const double ETD = 4.9;
        const double ETN = 2.31;
        const double GTT = 35.78;
        const double GTN = 4.01;
        const double GET = 998.69;
        const double GEN = 0.05349;

        static void Look(SqliteConnection con, IRepository repository)
        {
            List<CalculationModel> data = repository.GetAllData(con);

            if (data != null)
            {
                foreach (CalculationModel model in data)
                {
                    Console.WriteLine(model.ToString());
                }
            }
        }


        private static void Calc(SqliteConnection con, IRepository repository)
        {
            ICalculation calculation = new CalcImpl();

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);

            List<CalculationModel> data = repository.GetAllData(con);

            int N = 1;

            foreach (CalculationModel model in data)
            {
                if (date.Year == model.date.Year && date.Month == model.date.Month) { N++; } 
            }

            double HM = 0.0;
            double HR = 0.0;

            double GTNM = 0.0;
            double GTNR = 0.0;
            double VGTN = 0.0;

            double GTENM = 0.0;
            double GTER = 0.0;

            double EDM = 0.0;
            double ENM = 0.0;
            double ER = 0.0;

            int n = 0;
            string ans = "";

            double V = 0.0;

            Console.WriteLine("Кол-во человек проживающих в помещении: ");
            if (!int.TryParse(Console.ReadLine() ,out n))
            {
                throw new Exception("Ошибка ввода");
            }

            Console.WriteLine("Имеется прибор учета по услуге ХВС? (y/n): ");
            if ((ans = Console.ReadLine()) == "y")
            {
                Console.WriteLine("Введите показания счётчика: ");
                if (!Double.TryParse(Console.ReadLine(), out HM))
                {
                    throw new Exception("Ошибка ввода");
                }

                double Mprev = 0;

                if (data.Count > 1)
                {
                    Mprev = data[data.Count - 1].HM;
                }

                V = calculation.counterV(Mprev, HM);

                HR = calculation.carge(V, HT);

            } else if (ans == "n")
            {
                V = calculation.normV(HN, n);
                HR = calculation.carge(V, HT);

                for (int i = 1; i< N; i++)
                {
                    Console.WriteLine("Кол-во человек проживающих в помещении: ");
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        throw new Exception("Ошибка ввода");
                    }

                    V = calculation.normV(HN, n);
                    HR += calculation.carge(V, HT);
                }
            }
            else
            {
                throw new Exception("Ошибка ввода");
            }

            Console.WriteLine("Имеется прибор учета по услуге ГВС ТН? (y/n): ");
            if ((ans = Console.ReadLine()) == "y")
            {
                Console.WriteLine("Введите показания счётчика: ");
                if (!Double.TryParse(Console.ReadLine(), out GTNM))
                {
                    throw new Exception("Ошибка ввода");
                }

                double Mprev = 0;

                if (data.Count > 1)
                {
                    Mprev = data[data.Count - 1].GTNM;
                }

                VGTN = calculation.counterV(Mprev, GTNM);

                GTNR = calculation.carge(VGTN, GTT);

            }
            else if (ans == "n")
            {
                VGTN = calculation.normV(GTN, n);
                GTNR += calculation.carge(VGTN, GTT);

                for (int i = 1; i < N; i++)
                {
                    Console.WriteLine("Кол-во человек проживающих в помещении: ");
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        throw new Exception("Ошибка ввода");
                    }

                    VGTN = calculation.normV(GTN, n);
                    GTNR += calculation.carge(VGTN, GTT);
                }
            }
            else
            {
                throw new Exception("Ошибка ввода");
            }

            Console.WriteLine("Имеется прибор учета по услуге ГВС ТЕ? (y/n): ");
            if ((ans = Console.ReadLine()) == "y")
            {
                Console.WriteLine("Введите показания счётчика: ");
                if (!Double.TryParse(Console.ReadLine(), out GTENM))
                {
                    throw new Exception("Ошибка ввода");
                }

                double Mprev = 0;

                if (data.Count > 1)
                {
                    Mprev = data[data.Count - 1].GTEM;
                }

                V = calculation.counterV(Mprev, GTENM);
                GTER = calculation.carge(V, GET);
            }
            else if (ans == "n")
            {
                V = calculation.termEnV(VGTN, GEN);
                GTER = calculation.carge(V, GET);
            }
            else
            {
                throw new Exception("Ошибка ввода");
            }

           

            Console.WriteLine("Имеется прибор учета по услуге ЭЭ? (y/n): ");
            if ((ans = Console.ReadLine()) == "y")
            {
                Console.WriteLine("Введите показания счётчика за день: ");
                if (!Double.TryParse(Console.ReadLine(), out EDM))
                {
                    throw new Exception("Ошибка ввода");
                }
                Console.WriteLine("Введите показания счётчика за ночь: ");
                if (!Double.TryParse(Console.ReadLine(), out ENM))
                {
                    throw new Exception("Ошибка ввода");
                }

                double mPrevD = 0;
                double mPrevN = 0;

                if (data.Count > 1)
                {
                    mPrevD = data[data.Count - 1].EEDM;
                    mPrevN = data[data.Count - 1].EENM;
                }

                V = calculation.counterV(mPrevD, EDM);

                ER = calculation.carge(V, ETD);

                V = calculation.counterV(mPrevN, ENM);

                ER += calculation.carge(V, ETN);
            }
            else if (ans == "n")
            {
                V = calculation.normV(EN, n);

                ER = calculation.carge(V, ET);

                for (int i = 1; i < N; i++)
                {
                    Console.WriteLine("Кол-во человек проживающих в помещении: ");
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        throw new Exception("Ошибка ввода");
                    }

                    V = calculation.normV(EN, n);
                    ER += calculation.carge(V, GET);
                }
            }
            else
            {
                throw new Exception("Ошибка ввода");
            }

            Console.WriteLine(string.Format("Результаты расчёта за текущий пирод: \nХВС: {0:#.##}\nГВС ТН: {1:#.##}\nГВС ТЕ: {2:#.##}\nЕЕ: {3:#.##}", HR, GTNR, GTER, ER));
            repository.AddData(HR, GTNR, GTER, ER, HM, GTNM, GTENM, ENM, EDM, date, con);
        }

        private static void Summ(SqliteConnection con, IRepository repository)
        {
            List<CalculationModel> data = repository.GetAllData(con);

            double sumHVS = 0;
            double sumGVTNS = 0;
            double sumGVTES = 0;
            double sumEE = 0;
            double sum = 0;

            foreach (CalculationModel model in data)
            {
                sumHVS += model.HVS;
                sumGVTNS += model.GVSTN;
                sumGVTES += model.GTEM;
                sumEE += model.EE;
                sum += sumHVS + sumGVTES + sumGVTNS + sumEE;
            }

            Console.WriteLine(string.Format("Общая ХВС: {0:#.##}\nОбщая ГВС ТН: {1:#.##}\nОбщая ГВС ТЕ: {2:#.##}\nОбщая ЭЭ: {3:#.##}\nИтог: {4:#.##}", sumHVS, sumGVTNS, sumGVTES, sumEE, sum));
        }

        static void Main(string[] args)
        {
            
            string ans = "";
            IRepository repository = new Repository();

            using (var db = new SqliteConnection("Data source = ЖКХ.db"))
            {
                db.Open();

                repository.CreateTable(db);

                while (ans != "Quit")
                {
                    Console.WriteLine("Меню:");
                    Console.WriteLine("1 - Просмотреть все расчёты.");
                    Console.WriteLine("2 - Расчёт.");
                    Console.WriteLine("3 - Итог.");
                    Console.WriteLine("4 - Выход.");

                    ans = Console.ReadLine();

                    switch (ans)
                    {
                        case "1":
                            Look(db, repository);
                            break;
                        case "2":
                            Calc(db, repository);
                            break;
                        case "3":
                            Summ(db, repository);
                            break;
                        case "4":
                            ans = "Quit";
                            break;
                        default:
                            Console.WriteLine("Неверный ввод");
                            break;
                    }
                }
            }

            Console.Read();
        }
    }
}