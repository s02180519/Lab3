/*данные измерений электромагнитного поля в зависимости от времени*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Numerics;
using Lab1_2;

namespace Lab1
{
    class Program
    {
        public static void DataChangedCollector(object sender, DataChangedEventArgs args)
        {
            // Console.WriteLine("DataChangedCollector");
            Console.WriteLine(args.Data + " " + args.ChangeInfo);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Lab3");
            V1MainCollection element_collection = new V1MainCollection();
            element_collection.DataChanged += DataChangedCollector;

            element_collection.AddDefaults();
            V1DataCollection value2;
            DateTime date = new DateTime(10, 10, 10);
            value2 = new V1DataCollection("ID6", date);
            value2.InitRandom(5, 1, 4, 3, 4);
            element_collection.Add(value2);
            element_collection[3] = value2;
            element_collection[3].Data = "ChangeInformation";
            element_collection.Remove("ChangeInformation", date);
            Console.ReadLine();
        }
    }
}