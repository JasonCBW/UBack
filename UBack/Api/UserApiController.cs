using System;
using System.Web.Http;
using CommonTools;
using Model;
using Model.Tools;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using DBLib;
using System.Web;
using System.Text;

namespace UBack.Api
{
    public class UserApiController : ApiController
    {
        /// <summary>
        /// 获取用户列表(状态,0有效用户,1无效用户)
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/getlist")]
        public IHttpActionResult getList(int page, int limit, string val)
        {
            JsonPage ret = new JsonPage();

            string sql = string.Format(@"select a.*,b.unitname,c.depname from sys_user a left join sys_unit b on a.unitid=b.id left join sys_dep c on a.depid=c.id where 1=1 and b.`status`=1 and c.`status`=1 {0} order by a.createdate desc", val == "" ? "" : " and a.username like '%" + val + "%'");

            ret = PageHelper.getPage(sql, page, limit);

            return Json(ret);
        }

        /// <summary>
        /// 根据ID获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/getByID")]
        public IHttpActionResult getByID(int id)
        {
            RetMsg ret = new RetMsg();

            try
            {
                sys_User r = MySQLDB.QueryById<sys_User, int>(id);

                ret.flag = true;
                ret.data = r;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }

        /// <summary>
        /// 添加或更新用户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/addOrupdate")]
        public IHttpActionResult addOrupdate(JObject job)
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                var id = job["id"]._ToInt32();

                if (id > 0)
                    sql = Util.CreateSqlByJsonData(SQLEnum.UPDATE, "sys_user", "id", job);
                else
                    sql = Util.CreateSqlByJsonData(SQLEnum.INSERT, "sys_user", "id", job);

                int effect = MySQLDB.ExecuteSql(sql);

                ret.flag = effect > 0;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/del")]
        public IHttpActionResult del(JObject data)
        {
            RetMsg ret = new RetMsg();
            try
            {
                string ids = data["ids"].ToString();

                string sql = string.Format(@"delete from sys_user where id in ({0})", ids == "" ? "0" : ids);

                ret.flag = MySQLDB.ExecuteSql(sql) > 0;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }
    }
}
