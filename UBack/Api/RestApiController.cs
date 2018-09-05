using CommonTools;
using DBLib;
using Model;
using Model.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace UBack.Api
{
    public class RestApiController : ApiController
    {
        /// <summary>
        /// 获取接口列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/restapi/getlist")]
        public IHttpActionResult getlist(int page, int limit)
        {
            JsonPage ret = new JsonPage();

            string sql = string.Format(@"select * from sys_restapi order by id asc");

            ret = PageHelper.getPage(sql, page, limit);

            return Json(ret);
        }

        /// <summary>
        /// 根据ID获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/restapi/getByID")]
        public IHttpActionResult getByID(int id)
        {
            RetMsg ret = new RetMsg();

            try
            {
                sys_RestApi r = MySQLDB.QueryById<sys_RestApi, int>(id);

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
        /// 更新接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/restapi/addOrupdate")]
        public IHttpActionResult addOrupdate(JObject job)
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                var id = job["id"]._ToInt32();

                if (id > 0)
                    sql = Util.CreateSqlByJsonData(SQLEnum.UPDATE, "sys_restapi", "id", job);
                else
                    sql = Util.CreateSqlByJsonData(SQLEnum.INSERT, "sys_restapi", "id", job);

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
        /// 删除按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/restapi/del")]
        public IHttpActionResult delmenu(JObject data)
        {
            RetMsg ret = new RetMsg();
            try
            {
                string ids = data["ids"].ToString();

                string sql = string.Format(@"delete from sys_restapi where id in ({0})", ids == "" ? "0" : ids);

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
