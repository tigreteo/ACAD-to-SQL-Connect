using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADtoSQL
{
    class SQLConnect
    {
        public static SqlConnection openSQL()
        {
            SqlConnection sqlDB = new SqlConnection(@"
                Server=SFVALDB01;
                database=EngData;
                User Id=engclient;
                Password=engdata;
                MultipleActiveResultSets=true;");

        return sqlDB;
        }
    }
}
