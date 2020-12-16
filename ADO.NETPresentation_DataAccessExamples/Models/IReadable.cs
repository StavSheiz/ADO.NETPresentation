using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NETPresentation_DataAccessExamples.Models
{
    interface IReadable
    {
        void PopulateObject(SqlDataReader reader);
    }
}
