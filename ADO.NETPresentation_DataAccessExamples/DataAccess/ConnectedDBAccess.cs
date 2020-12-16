using ADO.NETPresentation_DataAccessExamples.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NETPresentation_DataAccessExamples
{
    class ConnectedDBAccess
    {
        private static string connectionString = "Server=localhost;Database=ADONETEx;Trusted_Connection=True;";

        public static ArrayList ExecuteQuery<T>(string query, List<SqlParameter> parameters, CommandType commandType = CommandType.Text) where T: IReadable, new()
        {
            ArrayList arrResults = new ArrayList();

            // Create a connection with a using statement to make sure the connection is closed and disposed
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
       
                SqlCommand command = new SqlCommand(query);

                command.CommandType = commandType;
                command.Parameters.AddRange(parameters.ToArray());
                command.Connection = connection;

                try
                {
                    connection.Open();

                    // Create a reader with a using statement to make sure the reader is disposed
                    using (SqlDataReader reader = command.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            T obj = new T();

                            obj.PopulateObject(reader);
                            arrResults.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR - ExecuteQuery | " + ex.Message);
                }
            }

            return arrResults;
        }

        public static object ExecuteScalar(string query, List<SqlParameter> parameters, CommandType commandType = CommandType.Text) 
        {
            object obj = new object();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(query);

                command.CommandType = commandType;
                command.Parameters.AddRange(parameters.ToArray());
                command.Connection = connection;

                try
                {
                    connection.Open();

                    obj = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR - ExecuteScalar | " + ex.Message);
                }
            }

            return obj;
        }

        public static int ExecuteNonQuery(string query, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            int numOfRowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(query);

                command.CommandType = commandType;
                command.Parameters.AddRange(parameters.ToArray());
                command.Connection = connection;

                try
                {
                    connection.Open();

                    numOfRowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR - ExecuteNonQuery | " + ex.Message);
                }
            }

            return numOfRowsAffected;
        }

        public static int ExecuteNonQueryTransaction(List<string> queries, List<List<SqlParameter>> parameters, List<CommandType> commandTypes)
        {
            int numOfRowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        for (int index = 0; index < queries.Count; index++)
                        {
                            string query = queries.ElementAt(index);
                            List<SqlParameter> currParameters = parameters.ElementAt(index);
                            CommandType commandType = commandTypes.ElementAt(index);

                            SqlCommand command = new SqlCommand(query);
                            command.Connection = connection;
                            command.Transaction = transaction;
                            command.CommandType = commandType;
                            command.Parameters.AddRange(currParameters.ToArray());

                            // Execute command
                            numOfRowsAffected += command.ExecuteNonQuery();
                        }

                        // When finished executing all commands - commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception transactionEx)
                    {
                        Console.WriteLine("ERROR - ExecuteNonQueryTransaction - transaction failed | " + transactionEx.Message);

                        try
                        {
                            // If the transaction failed - rollback to prevent any changes
                            transaction.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            Console.WriteLine("ERROR - ExecuteNonQueryTransaction - Rollback failed | " + rollbackEx.Message);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("ERROR - ExecuteNonQueryTransaction - open connection failed | " + ex.Message);
                }
            }

            return numOfRowsAffected;
        }
    }
}
