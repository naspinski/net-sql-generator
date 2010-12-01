using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using DotNetSqlGenerator.Library.Objects;
using DotNetPostgresSqlGenerator.Library;

namespace DotNetSqlGenerator.WebUI.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PgSqlGenerator pg = new PgSqlGenerator("127.0.0.1", 5432, "postgres", "sql", "testing");

            try
            {
                var people = pg.GetTable("people");
                ////string query = GenerateSql.Insert.For(people);
                string query = GenerateSql.Select.For(people, pg, 2);
                ViewData["query"] = "select statement: " + query + "<br /><br />";

                ViewData["query"] += "delete statement: " + GenerateSql.Delete.For(people, pg) + "<br /><br />";

                ViewData["query"] += "insert statement: " + GenerateSql.Insert.For(people) + "<br /><br />";

                ViewData["query"] += "insert bulk statement: " + GenerateSql.Insert.BulkFor(people, 3) + "<br /><br />";

                ViewData["query"] += "update statement: " + GenerateSql.Update.For(people, pg) + "<br /><br />";

                //pg.RunNonQuery("DELETE FROM people WHERE id <> 11;");
                //int inserted = pg.RunNonQuery(query);

                //ViewData["output"] = inserted.ToString();

                //string show = "";
                //var reader = pg.RunReader("SELECT * FROM people");
                //while (reader.Read())
                //    show += reader["id"] + " | " + reader["name"] + "<br />";

                //show += "<h2>single random record</h2>";

                //foreach (object o in pg.GetSingleRandomRecordFrom(people))
                //    show += o.ToString() + "<br />";

                //ViewData["table"] = show;
            }
            finally { pg.Dispose(); }

            return View(pg);
        }

        public ActionResult GeneratorTest()
        {
            return View(new PgSqlGenerator("127.0.0.1", 5432, "postgres", "sql", "testing", "MINPOOLSIZE=20;MAXPOOLSIZE=50;"));
        }

        public ActionResult Fields(string id)
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
