using System.Collections.Generic;

namespace InstagramPhotos.Utility.Page
{
    /// <summary>
    ///     分页
    /// </summary>
    public static class PageOperate
    {
        /// <summary>
        ///     分页算法
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="dataCount">总记录数</param>
        /// <param name="showTag">显示几个页面选项</param>
        /// <returns></returns>
        public static Paging GetPaging(int pageIndex, int pageSize, int dataCount, int showTag)
        {
            int minPage = 0;
            int maxPage = 0;
            const int firstPage = 1;
            int lastPage = 0;
            int show = 0;

            var paging = new Paging();
            paging.PageSize = pageSize;
            paging.PageIndex = pageIndex;
            paging.DataCount = dataCount;
            paging.PrvePage = pageIndex - 1;
            paging.NextPage = pageIndex + 1;
            paging.StartPage = 1;
            paging.EndPage = dataCount/paging.PageSize;
            paging.Pages = new List<Paging>();
            if (dataCount%paging.PageSize != 0)
            {
                paging.EndPage += 1;
            }
            paging.PageCount = paging.EndPage;
            lastPage = paging.EndPage;
            if (paging.PrvePage <= 0)
            {
                paging.PrvePage = 1;
            }
            if (paging.NextPage >= lastPage)
            {
                paging.NextPage = lastPage;
            }

            if (showTag%2 == 1)
            {
                show = showTag;
            }
            else
            {
                show = showTag + 1;
            }
            if (show > lastPage)
            {
                show = dataCount;
            }
            minPage = pageIndex - showTag/2;
            if (minPage <= firstPage)
            {
                minPage = firstPage;
                if (lastPage >= show)
                {
                    maxPage = show;
                }
                else
                {
                    maxPage = lastPage;
                }
            }
            else
            {
                maxPage = pageIndex + showTag/2;
                if (maxPage >= lastPage)
                {
                    maxPage = lastPage;
                    minPage = lastPage - show;
                    if (minPage <= 0)
                    {
                        minPage = 1;
                    }
                }
            }
            for (int i = minPage; i <= maxPage; i++)
            {
                var temp = new Paging();
                temp.DataCount = dataCount;
                temp.EndPage = paging.EndPage;
                temp.StartPage = 1;
                temp.PageIndex = i;
                paging.Pages.Add(temp);
            }
            return paging;
        }
    }
}