using Lab2.Interfaces;
using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lab2
{
    class Program
    {
        public string OrdersFilePath { get; set; } = "orders.txt";
        public string OptionsFilePath { get; set; } = "options.txt";
        public string ClientsFilePath { get; set; } = "clients.txt";

        private List<Client> _newClients;
        private List<Option> _newOptions;
        private List<Order> _newOrders;

        public Program()
        {
            _newClients = new List<Client>();
            _newOptions = new List<Option>();
            _newOrders = new List<Order>();
        }

        public void WriteData()
        {
            ReadClientsFromFile();
            ReadOptionsFromFile();
            ReadAndWriteOrders();
        }


        public void PrintTables()
        {
            using (var context = new PhotoStudioContext())
            {
                PrintTable<Client>("clients", context.Clients);
                PrintTable<Option>("options", context.Options);
                PrintTable<Order>("orders", context.Orders);
            }
        }

        public void RemoveData()
        {
            using (var context = new PhotoStudioContext())
            {
                context.Orders.RemoveRange(context.Orders);
                context.Clients.RemoveRange(context.Clients);
                context.Options.RemoveRange(context.Options);
                context.SaveChanges();

            }
        }
        public void ReadAndWriteOrders()
        {
            using (StreamReader reader = new StreamReader(OrdersFilePath))
            {
                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(';');
                    Order order = new Order();
                    order.Client = _newClients[Convert.ToInt32(values[0]) - 1];
                    order.Option = _newOptions[Convert.ToInt32(values[1]) - 1];
                    order.Quantity = Convert.ToInt32(values[2]);
                    order.DateStart = DateTime.Parse(values[3]);
                    order.DateFinish = DateTime.Parse(values[4]);
                    _newOrders.Add(order);
                }
            }
            using (var context = new PhotoStudioContext())
            {
                context.AddRange(_newOrders);
                context.SaveChanges();
            }
        }

        private void ReadOptionsFromFile()
        {
            List<IReadableFromString> options = readFromFile<Option>(OptionsFilePath);
            foreach(var obj in options)
            {
                _newOptions.Add(obj as Option);
            }
        }

        private void ReadClientsFromFile()
        {
            List<IReadableFromString> clients = readFromFile<Client>(ClientsFilePath);
            foreach (var obj in clients)
            {
                _newClients.Add(obj as Client);
            }
        }

        private void PrintTable<T>(string tableName, IEnumerable<T> rows)
        {
            Console.WriteLine($"==============={tableName}====================");
            foreach (T row in rows)
            {
                Console.WriteLine(row.ToString());
            }
        }

        private List<IReadableFromString> readFromFile<T>(string filename) where T : IReadableFromString
        {
            List<IReadableFromString> objects = new List<IReadableFromString>();
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {

                    string line = reader.ReadLine();
                    if (line.Length > 0)
                    {
                        var obj = Activator.CreateInstance(typeof(T)) as IReadableFromString;
                        try
                        {
                            obj.ReadFromStringArray(line.Split(';'));
                            objects.Add(obj);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine($"Can not parse line: {line}");
                        }
                    }
                }
            }
            return objects;
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.WriteData();
            program.PrintTables();
            program.RemoveData();

        }
    }
}
