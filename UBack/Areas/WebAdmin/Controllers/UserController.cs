using DBLib;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace UBack.Areas.WebAdmin.Controllers
{
    public class UserController : Controller
    {
        // GET: WebAdmin/User

        public ActionResult Index()
        {
            string sql = string.Empty;

            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();

            DataTable r = new DataTable();

            sql = "select id,unitname from sys_unit where `status`=1";

            r = MySQLDB.Query(sql);

            dic.Add("units", r);

            sql = "SELECT id,rolename FROM sys_role where status=1";

            r = MySQLDB.Query(sql);

            dic.Add("roles", r);

            return View(dic);
        }
    }
}