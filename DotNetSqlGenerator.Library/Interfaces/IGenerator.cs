using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotNetSqlGenerator.Library.Interfaces
{
    public interface IGenerator
    {
        //string ConnectionString;
        //IDbConnection Connection;

        int RunNonQuery(string query);
        void Dispose();
    }
}
