using System;

namespace InstagramPhotos.Utility.Security.Principal
{
    public interface ILoginHandler
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>Tuple{System.BooleanSystem.String[]}.</returns>
        Boolean Login(string username, string password);
    }
}
