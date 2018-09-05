using CommonTools;
using DBLib;
using Model.Tools;
using System;
using System.Data;
using System.Web.Mvc;

namespace UBack.Areas.WebAdmin.Controllers
{
    public class LoginController : Controller
    {
        // GET: WebAdmin/Login
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="account">登录码</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpGet]
        public string userlogin(string account, string pwd)
        {
            RetMsg ret = new RetMsg();
            ret.flag = true;
            string sql = "";
            int effect = 0;
            try
            {
                sql = string.Format(@"select count(0) from sys_user where logincode='{0}'", account);

                effect = MySQLDB.FirstValue(sql)._ToInt32();
                if (effect <= 0)
                {
                    ret.flag = false;
                    ret.msg = "用户不存在!";
                    return ret._ToJson();
                }

                sql = string.Format(@"select * from sys_user where logincode='{0}' and pwd=md5('{1}')", account, pwd);

                DataTable dt = MySQLDB.Query(sql);

                effect = dt.Rows.Count;

                if (effect <= 0)
                {
                    ret.flag = false;
                    ret.msg = "用户密码错误!";
                    return ret._ToJson();
                }

                Session["userid"] = dt.Rows[0]["id"]._ToInt32();
                Session["roleid"] = dt.Rows[0]["roleid"].ToString();
                Session["unitid"] = dt.Rows[0]["unitid"].ToString();
                Session["depid"] = dt.Rows[0]["depid"].ToString();
                ret.data = dt;
            }
            catch (Exception ex)
            {
                ret.flag = false;
                ret.msg = ex.Message;
            }
            return ret._ToJson();
        }
    }
}