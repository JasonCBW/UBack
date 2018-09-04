using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class sys_User
    {
        public int id { get; set; }

        public string username { get; set; }

        public string logincode { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string pwd { get; set; }

        public string roleid { get; set; }

        public int unitid { get; set; }

        public int depid { get; set; }

        public string specialpower { get; set; }

        public int status { get; set; }

        public DateTime createdate { get; set; }

        public DateTime lastupdate { get; set; }

        public DateTime lastlogin { get; set; }

        public string remark { get; set; }
    }
}
