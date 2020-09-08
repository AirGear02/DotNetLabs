using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Lab1
{
    class Program
    {
        private const string _host = "localhost";
        private const string _database = "photo_studio";
        private const string _user = "root";
        private const string _password = "";


        private delegate void methodToExecute(string table, MySqlCommand query);
        private MySqlConnection _connection;

        private string[] _tables = { "clients", "options", "orders"};


        private void makeConnection()
        {
            string connection_string = $"Database={_database};Datasource={_host};User={_user};Pasword={_password}";
            _connection = new MySqlConnection(connection_string);
        }


       
        private void makeForAllTables(methodToExecute method)
        {
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            using (MySqlCommand query = _connection.CreateCommand())
            {
                foreach (var table in _tables)
                {
                    method(table, query);
                }
            }

            _connection.Close();   
        }

        private void ReadTable(string table, MySqlCommand query)
        {
            MySqlDataReader reader = default;
            try
            {
                query.CommandText = $"SELECT * FROM {table}";
                reader = query.ExecuteReader();
                Console.WriteLine($"======================================={table}=======================================");
                 
                while(reader.Read())
                {
                    object[] objects = new object[reader.FieldCount];
                    reader.GetValues(objects);

                    foreach(object ob in objects)
                    {
                        Console.WriteLine(ob.ToString());
                    }
                    Console.WriteLine();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader?.Close();
            }
        }

        private void UpTable(string table, MySqlCommand query)
        {
            MySqlDataReader reader = default;
            foreach (var line in readFromFile($@"../../../../{table}.txt"))
            {
                try
                {
                    string row = string.Join(',', line);
                    query.CommandText = $"INSERT INTO {table} VALUES({row})";
                    reader = query.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    reader?.Close();
                }
            }
        }

        private void DownTable(string table, MySqlCommand query)
        {
            MySqlDataReader reader = default;
            try
            {
                query.CommandText = $"DELETE FROM {table} WHERE true";
                reader = query.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader?.Close();
            }
        }

        public void ReadAll()
        {
            makeForAllTables(ReadTable);
        }

        public void UpAll()
        {
            makeForAllTables(UpTable);
        }

        public void DownAll()
        {
            makeForAllTables(DownTable);
        }

        private IEnumerable<string[]> readFromFile(string filepath)
        {

            StreamReader reader = default;
            try
            {
                reader = new StreamReader(filepath);
            }
            catch
            {
                Console.WriteLine($"Can not open {filepath}");
                yield break;
            }
           
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if(line.Length > 0) yield return line.Split(';');
            }

            reader.Close();
            
            
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Program program = new Program();
            program.makeConnection();
            program.UpAll();
            program.ReadAll();
            program.DownAll();
        }
    }
}
