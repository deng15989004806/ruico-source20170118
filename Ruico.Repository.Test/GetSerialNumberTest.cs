using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Repository.SystemModule.Repositories;
using Xunit;

namespace Ruico.Repository.Test
{
    public class GetSerialNumberTest
    {
        [Fact]
        public void GetSerialNumberTest1()
        {
            var sw = new Stopwatch();
            TimeSpan timeCost;

            using (var unitOfWork = new RuicoUnitOfWork())
            {
                sw.Start();

                var repository = new SerialNumberRepository(unitOfWork);

                var prefix = "AB";
                var dateNumber = "";
                var inc = 1;
                var numberLength = 3;

                var number = repository.GetSerialNumber(prefix, dateNumber, inc);

                var sn = string.Format("{0}{1}{2}", prefix, dateNumber, number.ToString().PadLeft(numberLength, '0'));

                Console.WriteLine("Number: {0}", number);
                Console.WriteLine("SN: {0}", sn);

                sw.Stop();
                timeCost = sw.Elapsed;
            }

            Console.WriteLine("Elapsed: " + timeCost.Ticks);
        }


        [Fact]
        public void GetSerialNumberTest2()
        {
            var sw = new Stopwatch();
            TimeSpan timeCost;

            using (var unitOfWork = new RuicoUnitOfWork())
            {
                sw.Start();

                var repository = new SerialNumberRepository(unitOfWork);

                var prefix = "NBA";
                var dateNumber = DateTime.Now.ToString("yyyyMMdd");
                var inc = 1;
                var numberLength = 5;

                var number = repository.GetSerialNumber(prefix, dateNumber, inc);

                var sn = string.Format("{0}{1}{2}", prefix, dateNumber, number.ToString("0")
                    .PadLeft(numberLength, '0'));

                Console.WriteLine("Number: {0}", number);
                Console.WriteLine("SN: {0}", sn);

                sw.Stop();
                timeCost = sw.Elapsed;
            }

            Console.WriteLine("Elapsed: " + timeCost.Ticks);
        }
    }
}
