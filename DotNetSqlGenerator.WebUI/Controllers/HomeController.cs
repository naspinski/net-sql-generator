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
            
            var people = pg.GetTable("people");
            //string query = GenerateSql.Insert.For(people);
            string query = GenerateSql.Select.For(people, pg.GetSingleRandomRecordFrom(people), 2, 3);
            ViewData["query"] = query;

            //pg.RunNonQuery("DELETE FROM people WHERE id <> 11;");
            //int inserted = pg.RunNonQuery(query);

            //ViewData["output"] = inserted.ToString();

            string show = "";
            var reader = pg.RunReader("SELECT * FROM people");
            //while (reader.Read())
            //    show += reader["id"] + " | " + reader["name"] + "<br />";

            show += "<h2>single random record</h2>";

            foreach(object o in pg.GetSingleRandomRecordFrom(people))
                show += o.ToString() + "<br />";

            ViewData["table"] = show;

            return View(pg);
        }

        public ActionResult GeneratorTest()
        {
            return View(new PgSqlGenerator("127.0.0.1", 5432, "postgres", "sql", "testing"));
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
