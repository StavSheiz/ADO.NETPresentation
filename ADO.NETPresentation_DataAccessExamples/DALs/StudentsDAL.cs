using ADO.NETPresentation_DataAccessExamples.DataAccess;
using ADO.NETPresentation_DataAccessExamples.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static ADO.NETPresentation_DataAccessExamples.Datasets.SchoolDataSet;

namespace ADO.NETPresentation_DataAccessExamples.DALs
{
    class StudentsDAL
    {
        public StudentsDAL() 
        {
            disconnectedDBAccess = new DisconnectedDataAccess<StudentsDataTable>();

            initSelectCommand(disconnectedDBAccess);
            initUpdateCommand(disconnectedDBAccess);
            initInsertCommand(disconnectedDBAccess);
            initDeleteCommand(disconnectedDBAccess);
        }

        private DisconnectedDataAccess<StudentsDataTable> disconnectedDBAccess;

        #region Disconnected public interface

        public StudentsDataTable GetStudentsTable() 
        {
            StudentsDataTable table = new StudentsDataTable();
            disconnectedDBAccess.Fill(table);

            return table;
        }

        public int UpdateStudentsTable(StudentsDataTable table) 
        {
            return disconnectedDBAccess.Update(table);
        }
        #endregion

        #region Connected public interface

        public ArrayList GetAllStudents() 
        {
            string query = "SELECT * FROM Students";

            List<SqlParameter> parameters = new List<SqlParameter>();
            ArrayList results = ConnectedDBAccess.ExecuteQuery<Student>(query, parameters);

            return results;
        }

        public ArrayList GetStudentsByCountry(string country) 
        {
            string query = "SELECT * FROM Students WHERE country = @COUNTRY";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("COUNTRY", country));

            ArrayList results = ConnectedDBAccess.ExecuteQuery<Student>(query, parameters);

            return results;
        }

        public Student GetStudent(int id) 
        {
            string query = "SELECT * FROM Students WHERE id = @ID";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("ID", id));

            ArrayList results = ConnectedDBAccess.ExecuteQuery<Student>(query, parameters);

            return (Student)results[0];
        }

        public bool DeleteStudent(int id) 
        {
            string query = "DELETE FROM Students WHERE id = @ID";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("ID", id));

            int rowsAffected = ConnectedDBAccess.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }
        #endregion

        #region Private functions

        private void initSelectCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
        {
            dbAccess.SelectCommand = new SqlCommand("SELECT id,first_name,last_name,age,country FROM Students");
        }

        private void initUpdateCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
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

        private void initInsertCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
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

        private void initDeleteCommand(DisconnectedDataAccess<StudentsDataTable> dbAccess)
        {
            SqlParameter oldIdParam = new SqlParameter("@oldId", SqlDbType.Int, 5, "id");
            oldIdParam.SourceVersion = DataRowVersion.Original;

            dbAccess.DeleteCommand = new SqlCommand("DELETE FROM Students WHERE id=@oldId");
            dbAccess.DeleteCommand.Parameters.Add(oldIdParam);
        }

        #endregion
    }
}
