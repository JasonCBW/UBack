using DBLib;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace UBack.Areas.WebAdmin.Controllers
{
    public class MenuController : Controller
    {
        // GET: WebAdmin/Menu
        public ActionResult Index()
        {
            string sql = string.Empty;

            DataTable r = new DataTable();

            Dictionary<string, List<DataRow>> dic = new Dictionary<string, List<DataRow>>();

            sql = "select b.id,b.modulename,a.modulename as parentname,cast(a.id as char) as parentid,b.`status` from sys_module a inner join sys_module b on a.id=b.parentid";

            r = MySQLDB.Query(sql);
             
            var query = from g in r.AsEnumerable()
                        group g by new { t2 = g.Field<string>("parentname") } into menus
                        select new { parentname = menus.Key.t2, StallInfo = menus };

            foreach (var userInfo in query)
            {
                List<DataRow> dataRows = userInfo.StallInfo.ToList();

                dic.Add(userInfo.parentname, dataRows);
            }

            return View(dic);
        }
    }
}