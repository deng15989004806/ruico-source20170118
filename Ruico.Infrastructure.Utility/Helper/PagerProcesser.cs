using System;
using System.Collections.Generic;
using System.Linq;

namespace Ruico.Infrastructure.Utility.Helper
{
    public static class PagerProcesserExtensions
    {
        public static void ProcessWithPager<T>(this IEnumerable<T> list, int pageSize, Action<IList<T>> handle)
        {
            var enumerable = list as IList<T> ?? list.ToList();

            // 分页处理，每页pageSize个
            var pagerProcesser = new PagerProcesser(pageSize);
            do
            {
                // 从父列表根据当前的页码和分页大小取得子列表
                var subList = pagerProcesser.GetList(enumerable);

                // 处理逻辑
                handle(subList);
            }
            while (pagerProcesser.MoveNext());
        }
    }

    /*
        // 分页处理，每页1000个
        var pagerProcesser = new PagerProcesser(1000);
        do
        {
            // 从父列表根据当前的页码和分页大小取得子列表
            var subList = pagerProcesser.GetList(list);

            // 处理逻辑               
        }
        while (pagerProcesser.MoveNext());
     */

    /// <summary>
    /// 分页处理
    /// </summary>
    public class PagerProcesser
    {
        private int TotalCount { get; set; }

        private int PageSize { get; set; }

        public int PageIndex { get; private set; }

        private int TotalPage
        {
            get
            {
                return this.TotalCount / this.PageSize + (this.TotalCount % this.PageSize == 0 ? 0 : 1);
            }
        }

        public PagerProcesser(int pageSize = 10)
        {
            this.PageSize = pageSize;
            this.PageIndex = 1;
        }

        public bool MoveNext()
        {
            this.PageIndex++;
            return this.TotalCount > 0 && this.TotalPage >= this.PageIndex;
        }

        public IList<T> GetList<T>(IEnumerable<T> list)
        {
            var result = list.Skip(this.PageSize * (this.PageIndex - 1)).Take(this.PageSize).ToList();
            this.TotalCount = list.Count();

            return result;
        }
    }
}
