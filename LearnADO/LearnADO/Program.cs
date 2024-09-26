using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnADO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseTest();
        }

        private static void DatabaseTest()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=DESKTOP-TU7K06B;Initial Catalog=adventureworks;Integrated Security=True;";
                connection.Open();

                string sql = "Select Id, Name From Category";
                var command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();
                var categories = new List<Category>();

                while (reader.Read())
                {
                    Console.WriteLine("Id: {0}, Name: {1}", reader["Id"], reader["Name"]);
                    categories.Add(new Category
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"]
                    });
                }

                var result = categories.Where(c => c.Id.Equals(1));

                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);
                connection.Close(); 
            }
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
