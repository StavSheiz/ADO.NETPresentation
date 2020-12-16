using ADO.NETPresentation_DataAccessExamples.DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ADO.NETPresentation_DataAccessExamples.Datasets.SchoolDataSet;

namespace ADO.NETPresentation_DataAccessExamples.Examples
{
    class DisconnectedExamples
    {
        public static void DisconnectedArchitecture_9()
        {
            DisconnectedDataAccess<StudentsDataTable> dbAccess = new DisconnectedDataAccess<StudentsDataTable>();

            initSelectCommand(dbAccess);
            initUpdateCommand(dbAccess);
            initInsertCommand(dbAccess);
            initDeleteCommand(dbAccess);

            StudentsDataTable table = new StudentsDataTable();
            dbAccess.Fill(table);

            Console.WriteLine("All datatable data:");
            printRows(table.Rows);

            // Change row in DataTable
            StudentsRow row = (StudentsRow)table.Rows[0];
            row.first_name = "Mickey";

            Console.WriteLine("All datatable data - after datarow change:");
            printRows(table.Rows);

            // Compare to data in db
            Console.WriteLine("All db data - before update:");
            ConnectedExamples.BasicReaderQuery_1();

            dbAccess.Update(table);

            // Compare to data in db after update
            Console.WriteLine("All db data - after update:");
            ConnectedExamples.BasicReaderQuery_1();

            // Do not forget to dispose!
            dbAccess.Dispose();
        }

        private static void initSelectCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
        {
            dbAccess.SelectCommand = new SqlCommand("SELECT id,first_name,last_name,age,country FROM Students");
        }

        private static void initUpdateCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
        {
            dbAccess.UpdateCommand = new SqlCommand("UPDATE Students SET " +
                "id=@id, " +
                "first_name=@first_name, " +
                "last_name=@last_name, " +
                "age=@age, " +
                "country=@country " +
                "WHERE id=@oldId");

            SqlParameter oldIdParam = new SqlParameter("@oldId", SqlDbType.Int, 5, "id");
            oldIdParam.SourceVersion = DataRowVersion.Original;

            SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 5, "id");
            SqlParameter firstNameParam = new SqlParameter("@first_name", SqlDbType.VarChar, 40, "first_name");
            SqlParameter lastNameParam = new SqlParameter("@last_name", SqlDbType.VarChar, 40, "last_name");
            SqlParameter ageParam = new SqlParameter("@age", SqlDbType.Int, 5, "age");
            SqlParameter countryParam = new SqlParameter("@country", SqlDbType.VarChar, 40, "country");

            dbAccess.UpdateCommand.Parameters.Add(oldIdParam);
            dbAccess.UpdateCommand.Parameters.Add(idParam);
            dbAccess.UpdateCommand.Parameters.Add(firstNameParam);
            dbAccess.UpdateCommand.Parameters.Add(lastNameParam);
            dbAccess.UpdateCommand.Parameters.Add(ageParam);
            dbAccess.UpdateCommand.Parameters.Add(countryParam);

        }

        private static void initInsertCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
        {
            dbAccess.InsertCommand = new SqlCommand("INSERT INTO Students(id, first_name, last_name, age, country) " +
                "Values(@id,@first_name,@last_name,@age,@country)");

            SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 5, "id");
            SqlParameter firstNameParam = new SqlParameter("@first_name", SqlDbType.VarChar, 40, "first_name");
            SqlParameter lastNameParam = new SqlParameter("@last_name", SqlDbType.VarChar, 40, "last_name");
            SqlParameter ageParam = new SqlParameter("@age", SqlDbType.Int, 5, "age");
            SqlParameter countryParam = new SqlParameter("@country", SqlDbType.VarChar, 40, "country");

            dbAccess.InsertCommand.Parameters.Add(idParam);
            dbAccess.InsertCommand.Parameters.Add(firstNameParam);
            dbAccess.InsertCommand.Parameters.Add(lastNameParam);
            dbAccess.InsertCommand.Parameters.Add(ageParam);
            dbAccess.InsertCommand.Parameters.Add(countryParam);
        }

        private static void initDeleteCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
        {
            SqlParameter oldIdParam = new SqlParameter("@oldId", SqlDbType.Int, 5, "id");
            oldIdParam.SourceVersion = DataRowVersion.Original;

            dbAccess.DeleteCommand = new SqlCommand("DELETE FROM Students WHERE id=@oldId");
            dbAccess.DeleteCommand.Parameters.Add(oldIdParam);
        }

        private static void printRows(DataRowCollection rows) 
        {
            foreach (StudentsRow row in rows) 
            {
                Console.WriteLine(row.id + ", " + row.first_name + ", " + row.last_name + ", " + row.age + ", " + row.country);
            }
        }
    }
}
