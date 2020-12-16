using ADO.NETPresentation_DataAccessExamples.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NETPresentation_DataAccessExamples.Examples
{
    class ConnectedExamples
    {

        // Reader query - Basic - Get all rows from table Students
        public static void BasicReaderQuery_1()
        {
            string query = "SELECT * FROM Students";

            List<SqlParameter> parameters = new List<SqlParameter>();
            ArrayList results = ConnectedDBAccess.ExecuteQuery<Student>(query, parameters);

            printResults(results);
        }

        // Reader query - with parameter - Get all rows from table Students where age < 40
        public static void ReaderQueryWithParam_2()
        {
            string query = "SELECT * FROM Students WHERE age < @AGE";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("AGE", 40));

            ArrayList results = ConnectedDBAccess.ExecuteQuery<Student>(query, parameters);

            printResults(results);
        }

        // Scalar query - basic - Get current date
        public static void BasicScalarQuery_3()
        {
            string query = "SELECT CAST( GETDATE() AS Date ) ;";

            List<SqlParameter> parameters = new List<SqlParameter>();
            object result = ConnectedDBAccess.ExecuteScalar(query, parameters);

            Console.WriteLine(result);
        }

        // NonQuery - basic - set the age of all students from usa to 45
        public static void BasicNonQuery_4()
        {
            string query = "UPDATE Students SET age=45 WHERE country='USA'";

            List<SqlParameter> parameters = new List<SqlParameter>();

            int rowsAffected = ConnectedDBAccess.ExecuteNonQuery(query, parameters);

            Console.WriteLine(rowsAffected + " rows affected");
        }

        // NonQuery - with parameters - insert a new row to table students
        public static void NonQueryWithParams_5()
        {
            string query = "INSERT INTO Students(first_name,last_name,age,country) VALUES(@FIRST_NAME,@LAST_NAME,@AGE,@COUNTRY)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("FIRST_NAME", "Alberto"));
            parameters.Add(new SqlParameter("LAST_NAME", "Pancas"));
            parameters.Add(new SqlParameter("AGE", 22));
            parameters.Add(new SqlParameter("COUNTRY", "Spain"));

            int rowsAffected = ConnectedDBAccess.ExecuteNonQuery(query, parameters);

            Console.WriteLine(rowsAffected + " rows affected");
        }

        // Reader query - Stored procedure - Get all rows from table Students
        public static void BasicReaderQueryStoredPorcedure_6()
        {
            /*
             * Defined in DB as:
             * CREATE PROCEDURE getAllStudents AS
             * SELECT * FROM dbo.Students
             */
            string query = "getAllStudents";

            List<SqlParameter> parameters = new List<SqlParameter>();
            ArrayList results = ConnectedDBAccess.ExecuteQuery<Student>(query, parameters, CommandType.StoredProcedure);

            printResults(results);
        }

        // NonQuery - Stored procedure - insert a new row to table students
        public static void NonQueryStoredProcedure_7()
        {
            /*
             * Defined in DB as:
             * CREATE PROCEDURE updateAge AS
             * UPDATE dbo.Students SET age = 25 WHERE first_name='Dan'
             */
            string queryName = "updateAge";

            List<SqlParameter> parameters = new List<SqlParameter>();

            int rowsAffected = ConnectedDBAccess.ExecuteNonQuery(queryName, parameters, CommandType.StoredProcedure);

            Console.WriteLine(rowsAffected + " rows affected");
        }

        // Transaction - NonQuery - multiple insert on the same transaction
        public static void NonQueryTransaction_8()
        {
            string query1 = "INSERT INTO Students(first_name,last_name,age,country) VALUES(@FIRST_NAME,@LAST_NAME,@AGE,@COUNTRY)";

            List<SqlParameter> parameters1 = new List<SqlParameter>();
            parameters1.Add(new SqlParameter("FIRST_NAME", "Mia"));
            parameters1.Add(new SqlParameter("LAST_NAME", "Lola"));
            parameters1.Add(new SqlParameter("AGE", 22));
            parameters1.Add(new SqlParameter("COUNTRY", "Spain"));

            string query2 = "INSERT INTO Students(first_name,last_name,age,country) VALUES(@FIRST_NAME,@LAST_NAME,@AGE,@COUNTRY)";

            List<SqlParameter> parameters2 = new List<SqlParameter>();
            parameters2.Add(new SqlParameter("FIRST_NAME", "Donald"));
            parameters2.Add(new SqlParameter("LAST_NAME", "Duck"));
            parameters2.Add(new SqlParameter("AGE", 100));
            parameters2.Add(new SqlParameter("COUNTRY", "USA"));

            List<string> queries = new List<string> { query1, query2 };
            List<List<SqlParameter>> parameters = new List<List<SqlParameter>> { parameters1, parameters2 };
            List<CommandType> commandTypes = new List<CommandType> { CommandType.Text, CommandType.Text };

            int rowsAffected = ConnectedDBAccess.ExecuteNonQueryTransaction(queries, parameters, commandTypes);

            Console.WriteLine(rowsAffected + " rows affected");
        }

        public static void printResults(ArrayList results)
        {
            foreach (object obj in results)
            {
                Console.WriteLine(obj);
            }
        }
    }
}
