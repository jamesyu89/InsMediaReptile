using System;

namespace InstagramPhotos.Utility.Authorization
{
    /// <summary>
    /// 登入登出接口
    /// </summary>
    /// <typeparam name="TUser">用户</typeparam>
    public interface ISignSystem<TUser> where TUser : class
    {
        /// <summary>
        /// 登录时把用户信息实体写入 Cookie（登入）
        /// </summary>
        /// <param name="user">用户信息实体</param>
        void SignIn(TUser user);

        /// <summary>
        /// 从 Cookie 中删除用户信息（登出）
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns cref="bool">是否登录</returns>
        bool CheckLogin(out TUser user);

        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns cref="bool">是否登录</returns>
        bool CheckLogin<T>(out T user);

        /// <summary>
        /// 如果赋值，则 Cookie 和 Ticket 过期时间均为该值；如不赋值，则 Cookie 为会话 Cookie，Ticket 使用默认过期时间<see cref="P:Gyyx.Core.Authentication.FormsSignSystem`1.DefaultTicketExpiryTime"/>
        /// </summary>
        TimeSpan ExpiryTime { get; set; }
    }
}
