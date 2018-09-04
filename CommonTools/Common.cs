using System;

namespace CommonTools
{
    public static class Common
    {
        public static string GetUserID(this HttpSessionStateBase session)
        {
            if (session == null)
            {
                return string.Empty;
            }
            string uid = session["userid"].ToString();
            return uid;
        }
    }
}
