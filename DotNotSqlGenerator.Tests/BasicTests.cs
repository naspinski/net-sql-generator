using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using System.Data;

namespace DotNotPostgreSqlGenerator.Tests
{
    [TestFixture]
    class BasicTests
    {
        PgSqlGenerator pg;

        [SetUp]
        public void Setup()
        {
            pg = new PgSqlGenerator("127.0.0.1", 5432, "postgres", "sql", "testing");
            //make new table for testing
            //pg.RunNonQuery("CREATE TABLE Test_People ( Id SERIAL, Name VARCHAR(100) NOT NULL, DOB DATE NOT NULL);");
        }

        //[Test]
        //public void GetTables()
        //{
        //    IEnumerable<string> names = pg.TableNames();
        //    Assert.IsTrue(names.Count() > 0);
        //    Assert.IsTrue(names.Contains("test_people"));
        //    var reader = pg.RunReader("SELECT * FROM people");
        //}

        [Test]
        public void AddAndDeleteRow()
        {
            pg.RunNonQuery("INSERT INTO Test_People (Name, DOB) VALUES ('Stan Naspinski','12/08/1981');");
           // int affected = pg.RunNonQuery("DELETE FROM Test_People WHERE name = 'Stan Naspinski';");
           // Assert.AreEqual(1, affected);
        }

        //[Test]
        //public void GetColumns()
        //{
        //    IDataReader reader = pg.GetColumns("Test_People");
        //    Assert.AreEqual(2, reader.FieldCount);
        //    int count = 0;
        //    string datatype = string.Empty, column = string.Empty;
        //    while (reader.Read())
        //    {
        //        if (count == 0)
        //        {
        //            //column = reader["column"].ToString();
        //            //datatype = reader[1].ToString();
        //        }
        //      //  Console.WriteLine(reader[0].ToString());
        //        count++;
        //    }
        //    Assert.AreEqual(3, count);
        //    Assert.AreEqual(2, reader.FieldCount);
        //    //Assert.AreEqual("id", column);
        //   // Assert.AreEqual("integer", datatype);
        //}

        [TearDown]
        public void TearDown()
        {
        //    pg.RunNonQuery("DROP TABLE Test_People;");
        }
    }
}
