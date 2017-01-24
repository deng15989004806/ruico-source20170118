using System;
using System.Collections.Generic;
using System.Linq;

namespace Ruico.Infrastructure.Utility.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 复制列表内项目的引用（源列表项目发生增删也不受影响）
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TSource> Copy<TSource>
            (this List<TSource> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<TSource>();
            }

            var list = new TSource[source.Count];
            source.CopyTo(list);

            return new List<TSource>(list);
        }

        /// <summary>
        /// 列表为null也不会出错
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TSource> OpSafe<TSource>
            (this List<TSource> source)
        {
            return source ?? new List<TSource>();
        }
    }
}
