using System;
using System.Web;
using System.Web.Security;

namespace InstagramPhotos.Utility.Authorization
{
    /// <summary>
    ///     使用 Forms 身份验证服务在 Cookie 中存储自定义用户实体<typeparamref name="TUser" />（登入登出系统）
    /// </summary>
    /// <typeparam name="TUser">自定义用户实体</typeparam>
    public abstract class FormsSignSystem<TUser> : ISignSystem<TUser> where TUser : class
    {
        /// <summary>
        ///     构建登入登出系统
        /// </summary>
        protected FormsSignSystem()
        {
            DefaultTicketExpiryTime = FormsAuthentication.Timeout; //new TimeSpan(0, 20, 0);
        }

        /// <summary>
        ///     Cookie 域
        /// </summary>
        /// <value cref="string"></value>
        protected string Domain { get; set; }

        /// <summary>
        ///     默认身份票据过期时间
        /// </summary>
        /// <value cref="TimeSpan"></value>
        protected TimeSpan DefaultTicketExpiryTime { get; set; }

        /// <summary>
        ///     如果赋值，则 Cookie 和 Ticket 过期时间均为该值；如不赋值，则 Cookie 为会话 Cookie，Ticket 使用默认过期时间<see cref="DefaultTicketExpiryTime" />
        /// </summary>
        public TimeSpan ExpiryTime { get; set; }

        /// <summary>
        ///     登录时把用户信息实体写入 Cookie（登入）
        /// </summary>
        /// <param name="user">用户信息实体</param>
        /// <typeparam name="TUser">自定义用户实体</typeparam>
        public virtual void SignIn(TUser user)
        {
            HttpContext.Current.Response.Cookies.Set(CreateCookie(user));
        }

        /// <summary>
        ///     从 Cookie 中删除用户信息（登出）
        /// </summary>
        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();

            HttpContext.Current.Response.Cookies.Set(CreateCookie(FormsAuthentication.FormsCookieName, "",
                new TimeSpan(-1)));

            if (HttpContext.Current.Session == null)
            {
                return;
            }

            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        ///     获取登录状态
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns cref="bool">是否登录</returns>
        public virtual bool CheckLogin(out TUser user)
        {
            user = GetUser();
            return user != null;
        }

        public virtual bool CheckLogin<T>(out T user)
        {
            user = default(T);
            return false;
        }

        /// <summary>
        ///     串行化用户信息实体
        /// </summary>
        /// <param name="user">用户信息实体</param>
        /// <returns cref="string">串行化后的用户数据</returns>
        protected abstract string Serialize(TUser user);

        /// <summary>
        ///     并行化用户数据
        /// </summary>
        /// <param name="userData">用户数据</param>
        /// <returns cref="T:`1">并行化后的用户信息实体</returns>
        protected abstract TUser Deserialize(string userData);

        /// <summary>
        ///     返回用户信息实体中的用户名
        /// </summary>
        /// <param name="user">用户信息实体</param>
        /// <returns cref="string">用户名</returns>
        protected abstract string GetAccount(TUser user);

        /// <summary>
        ///     创建 Cookie 内容
        /// </summary>
        /// <param name="user">用户信息实体</param>
        /// <returns cref="HttpCookie">存储有用户数据的 Cookie</returns>
        private HttpCookie CreateCookie(TUser user)
        {
            TimeSpan ticketTimeSpan = ExpiryTime;
            if (ticketTimeSpan == default(TimeSpan))
            {
                ticketTimeSpan = DefaultTicketExpiryTime;
            }

            var formsAuthenticationTicket = new FormsAuthenticationTicket(3,
                GetAccount(user),
                DateTime.Now,
                DateTime.Now.Add(ticketTimeSpan),
                false,
                Serialize(user),
                FormsAuthentication.FormsCookiePath);
            return CreateCookie(FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(formsAuthenticationTicket), DefaultTicketExpiryTime);
        }

        /// <summary>
        ///     创建 Forms 认证 Cookie
        /// </summary>
        /// <param name="name">Cookie 名称</param>
        /// <param name="value">Cookie 值</param>
        /// <param name="expiryTime">登录过期时间</param>
        /// <returns cref="HttpCookie">用户登录 Cookie</returns>
        private HttpCookie CreateCookie(string name, string value, TimeSpan expiryTime = default(TimeSpan))
        {
            var cookie = new HttpCookie(name)
            {
                Path = "/",
                Value = value,
                Secure = FormsAuthentication.RequireSSL,
                HttpOnly = true
            };
            if (expiryTime != default(TimeSpan))
            {
                cookie.Expires = DateTime.Now.Add(expiryTime);
            }
            if (!string.IsNullOrEmpty(Domain) &&
                HttpContext.Current.Request.Url.Host.ToLower().EndsWith(Domain.ToLower()))
            {
                cookie.Domain = string.IsNullOrEmpty(Domain) ? FormsAuthentication.CookieDomain : Domain;
            }
            else
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            return cookie;
        }

        /// <summary>
        ///     获取当前用户信息实体，如果没有找到则返回 default(TUser)
        /// </summary>
        /// <returns cref="T:`1">当前用户信息实体</returns>
        protected TUser GetUser()
        {
            if (!IsAuthenticated())
            {
                return default(TUser);
            }

            var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
            return formsIdentity == null ? default(TUser) : Deserialize(formsIdentity.Ticket.UserData);
        }

        /// <summary>
        ///     当前上下文中是否存在已验证的用户
        /// </summary>
        /// <returns cref="bool">当前上下文中是否存在已验证的用户</returns>
        private static bool IsAuthenticated()
        {
            return HttpContext.Current.User != null &&
                   HttpContext.Current.User.Identity.IsAuthenticated &&
                   //HttpContext.Current.User.Identity.Name != string.Empty &&
                   HttpContext.Current.User.Identity.AuthenticationType == "Forms";
        }
    }
}