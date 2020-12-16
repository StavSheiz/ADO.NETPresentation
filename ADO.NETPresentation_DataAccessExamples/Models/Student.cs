using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NETPresentation_DataAccessExamples.Models
{
    class Student : IReadable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        public void PopulateObject(SqlDataReader reader)
        {
            this.FirstName = (string)reader["first_name"];
            this.LastName = (string)reader["last_name"];
            this.Age = (int)reader["age"];
            this.Country = (string)reader["country"];
            this.Id = (int)reader["id"];
        }

        public override string ToString() 
        {
            return Id + ", " + FirstName + " " + LastName + ", " + Age + ", " + Country;
        }
    }
}
