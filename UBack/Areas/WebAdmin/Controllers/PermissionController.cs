using DBLib;
using Model.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace UBack.Areas.WebAdmin.Controllers
{
    public class PermissionController : Controller
    {
        // GET: WebAdmin/Permission
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 左侧菜单列表
        /// </summary>
        /// <returns></returns>
        public RetMsg getLeftRoleTree(string userid)
        {
            RetMsg ret = new RetMsg();

            string roles = string.Empty;

            string sql = string.Empty;

            DataTable r = new DataTable();

            List<object> listo = new List<object>();
            try
            {
                sql = string.Format(@"select roleid from sys_user where id={0}", userid);

                roles = MySQLDB.FirstValue(sql).ToString();

                if (roles != "")
                {
                    sql = string.Format(@"select GROUP_CONCAT(modules) from sys_role where id in ({0})", roles);

                    string modulesid = MySQLDB.FirstValue(sql).ToString();

                    sql = string.Format(@"select b.id,b.modulename text,b.url href,a.modulename as parentname,cast(a.id as char) as parentid,b.`status`,'' as icon from sys_module a inner join sys_module b on a.id=b.parentid where b.id in ({0}) order by b.id desc", modulesid);

                    r = MySQLDB.Query(sql);

                    var query = from g in r.AsEnumerable()
                                group g by new { t2 = g.Field<string>("parentname") } into modules
                                select new { parentname = modules.Key.t2, StallInfo = modules };

                    foreach (var moduleInfo in query)
                    {
                        DataTable dt = moduleInfo.StallInfo.ToList().CopyToDataTable();

                        object o = new { text = moduleInfo.parentname, icon = "", subset = dt };

                        listo.Add(o);
                    }
                    ret.flag = true;
                    ret.data = listo;
                }
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
                ret.flag = false;
            }

            return ret;
        }
    }
}