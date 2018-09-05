using CommonTools;
using DBLib;
using Model;
using Model.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace UBack.Api
{
    public class RoleApiController : ApiController
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/role/getlist")]
        public IHttpActionResult getRoleList(int page, int limit)
        {
            JsonPage ret = new JsonPage();

            //List<sys_Role> rlist = new List<sys_Role>();

            //rlist = MySQLDB.QueryAll<sys_Role>("sys_role");

            ret = PageHelper.getPage("sys_role", "*", page, limit);

            return Json(ret);
        }

        /// <summary>
        /// 根据ID获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/role/getByID")]
        public IHttpActionResult getByID(int id)
        {
            RetMsg ret = new RetMsg();

            try
            {
                sys_Role r = MySQLDB.QueryById<sys_Role, int>(id);

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
        /// 添加或更新角色
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/role/addOrupdate")]
        public IHttpActionResult addOrupdate(JObject job)
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                var id = job["id"]._ToInt32();

                if (id > 0)
                    sql = Util.CreateSqlByJsonData(SQLEnum.UPDATE, "sys_role", "id", job);
                else
                    sql = Util.CreateSqlByJsonData(SQLEnum.INSERT, "sys_role", "id", job);

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
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/role/del")]
        public IHttpActionResult delrole(JObject data)
        {
            RetMsg ret = new RetMsg();
            try
            {
                string ids = data["ids"].ToString();

                string sql = string.Format(@"delete from sys_role where id in ({0})", ids == "" ? "0" : ids);

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
