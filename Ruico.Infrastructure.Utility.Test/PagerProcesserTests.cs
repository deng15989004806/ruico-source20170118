using Ruico.Infrastructure.Utility.Helper;
using System;
using System.Linq;
using Xunit;

namespace Ruico.Infrastructure.Utility.Test
{
    public class PagerProcesserTests
    {
        [Fact]
        public void TestPagerProcesser()
        {
            var arr = new int[] { 1, 3, 5, 7, 9, 11, 12 };
            var pagerProcesser = new PagerProcesser(2);
            do
            {
                var list = pagerProcesser.GetList(arr);
                Console.WriteLine(string.Join(",", list.ToArray()));
            }
            while (pagerProcesser.MoveNext());
        }

        [Fact]
        public void TestPagerProcesserExtensions()
        {
            var arr = new int[] { 1, 3, 5, 7, 9, 11, 12 };

            arr.ProcessWithPager(2,
                subList => Console.WriteLine(string.Join(",", subList.ToArray())));
        }
    }
}
