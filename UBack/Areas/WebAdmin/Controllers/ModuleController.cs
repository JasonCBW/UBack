using DBLib;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace UBack.Areas.WebAdmin.Controllers
{
    public class ModuleController : Controller
    {
        // GET: WebAdmin/SysModule
        public ActionResult Index()
        {
            string sql = string.Empty;

            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();

            DataTable r = new DataTable();

            sql = "select id,modulename from sys_module where `status`= 1 and parentid=0";

            r = MySQLDB.Query(sql);

            dic.Add("module", r); 

            return View(dic);
        }
    }
}