using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class sys_Role
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string rolename { get; set; }

        /// <summary>
        /// 系统模块
        /// </summary>
        public string modules { get; set; }

        /// <summary>
        /// 按钮模块
        /// </summary>
        public string menus { get; set; }

        /// <summary>
        /// 状态,0启用,1未启用
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 角色备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate { get; set; }
    }
}
