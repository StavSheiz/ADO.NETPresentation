using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NETPresentation_DataAccessExamples.DataAccess
{
    class DisconnectedDataAccess<T> : IDisposable where T : DataTable
    {
        static string connectionString = "Server=localhost;Database=ADONETEx;Trusted_Connection=True;";

        public SqlCommand SelectCommand { get; set; }
        public SqlCommand UpdateCommand { get; set; }
        public SqlCommand InsertCommand { get; set; }
        public SqlCommand DeleteCommand { get; set; }

        public void Dispose()
        {
            SelectCommand.Dispose();
            UpdateCommand.Dispose();
            InsertCommand.Dispose();
            DeleteCommand.Dispose();
        }

        public int Fill(T dataTable) 
        {
            int rows = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SelectCommand.Connection = connection;

                try
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(SelectCommand))
                    {
                        rows = adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("ERROR - Fill | " + ex.Message);
                }
            }

            return rows;
        }

        public int Update(T dataTable) 
        {
            int rows = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                UpdateCommand.Connection = connection;
                InsertCommand.Connection = connection;
                DeleteCommand.Connection = connection;

                try
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.UpdateCommand = UpdateCommand;
                        adapter.InsertCommand = InsertCommand;
                        adapter.DeleteCommand = DeleteCommand;

                        rows = adapter.Update(dataTable);
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("ERROR - Update | " + ex.Message);
                }
            }

            return rows;
        }

    }
}
