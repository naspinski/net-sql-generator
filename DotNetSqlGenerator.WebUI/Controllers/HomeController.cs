using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using DotNetSqlGenerator.Library.Objects;

namespace DotNetSqlGenerator.WebUI.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string query = "SELECT a.attname as \"Column\", pg_catalog.format_type(a.atttypid, a.atttypmod) as \"Datatype\" " +
                            "FROM pg_catalog.pg_attribute a " +
                            "WHERE a.attnum > 0 AND NOT a.attisdropped AND a.attrelid = (" +
                                "SELECT c.oid FROM pg_catalog.pg_class c " +
                                    "LEFT JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace " +
                                "WHERE c.relname ~ '^(people)$' AND pg_catalog.pg_table_is_visible(c.oid));";

            PgSqlGenerator pg = new PgSqlGenerator("127.0.0.1", 5432, "postgres", "sql", "testing");
            var reader = pg.RunReader(query);
            var columns = pg.GetColumns("people");
            string o = string.Empty;

            foreach (Column c in columns)
                o += c.Name + " - " + c.SqlType.ToString() + " - " + c.DotNetType.ToString() + " - " + c.Limit + " | ";
            //while (reader.Read())
            //    o += reader[1].ToString() + " - ";
            
            
            ViewData["Message"] = o;

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
