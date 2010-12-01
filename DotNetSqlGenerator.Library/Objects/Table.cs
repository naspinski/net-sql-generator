using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetSqlGenerator.Library.Objects
{
    public class Table
    {
        public IEnumerable<Column> Columns;
        public string Name;
    }
}
