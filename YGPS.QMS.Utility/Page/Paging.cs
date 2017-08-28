using System.Collections.Generic;

namespace InstagramPhotos.Utility.Page
{
    /// <summary>
    ///     分页实体对象
    /// </summary>
    public class Paging
    {
        /// <summary>
        ///     当前页面位置
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     第一页
        /// </summary>
        public int StartPage { get; set; }

        /// <summary>
        ///     最后一页
        /// </summary>
        public int EndPage { get; set; }

        /// <summary>
        ///     下一页
        /// </summary>
        public int NextPage { get; set; }

        /// <summary>
        ///     上一页
        /// </summary>
        public int PrvePage { get; set; }


        /// <summary>
        ///     数据总条数
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        ///     总共页数
        /// </summary>
        public int PageCount { get; set; }


        /// <summary>
        ///     页面上显示的页数
        /// </summary>
        public List<Paging> Pages { get; set; }

        /// <summary>
        ///     每页多少条数据
        /// </summary>
        public int PageSize { get; set; }
    }
}