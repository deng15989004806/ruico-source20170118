using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ruico.Application.SystemModule;
using Ruico.Application.SystemModule.Imp;
using Xunit;

namespace Ruico.Application.Test
{
    public class OperateRecorderTest
    {
        [Fact]
        public void TestQueryOperateRecord()
        {
            var result = OperateRecorder.QueryOperateRecord("sn1234", 50, true);

            Assert.NotNull(result);
            //Assert.NotEmpty(result);

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void TestQueryOperateRecord2()
        {
            var result = OperateRecorder.QueryOperateRecord(string.Empty, 50, true);

            Assert.NotNull(result);
            //Assert.NotEmpty(result);

            Console.WriteLine(result.Count);

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
