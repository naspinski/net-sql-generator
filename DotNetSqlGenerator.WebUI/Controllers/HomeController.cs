using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;

namespace DotNetSqlGenerator.WebUI.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PgSqlGenerator pg = new PgSqlGenerator("127.0.0.1", 5432, "postgres", "sql", "testing");
            var reader = pg.RunReader("SELECT table_name FROM information_schema.tables WHERE table_schema='public';");
            string o = string.Empty;
            bool test = false;
            while (reader.Read())
            {
                o += reader[0].ToString() + " - ";
                test = true;
            }
            var names = pg.TableNames();
            ViewData["Message"] = o + names.Count();

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
