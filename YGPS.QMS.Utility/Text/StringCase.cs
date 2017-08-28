using System;

namespace InstagramPhotos.Utility.Text
{
    /// <summary>
    /// 指示数字、大小写字母
    /// </summary>
    [Flags]
    public enum StringCase
    {
        /// <summary>
        /// 数字
        /// </summary>
        Digit = 1,

        /// <summary>
        /// 小写
        /// </summary>
        Lower = 2,

        /// <summary>
        /// 大写
        /// </summary>
        Upper = 4
    }
}
