namespace InstagramPhotos.Utility.Page
{
    public class Page
    {
        /// <summary>
        ///     当前页面位置
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     数据总条数
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        ///     每页多少条数据
        /// </summary>
        public int PageSize { get; set; }

        public int ShowTag { get; set; }
    }
}