using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class sys_Module
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string modulename { get; set; }

        /// <summary>
        /// 模块路径
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 父级模块
        /// </summary>
        public int parentid { get; set; }

        /// <summary>
        /// 状态,0启用1不启用
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
