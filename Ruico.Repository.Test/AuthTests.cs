using System;
using System.Diagnostics;
using System.Linq;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility;
using Ruico.Infrastructure.Utility.Helper;
using Xunit;

namespace Ruico.Repository.Test
{
    public class AuthTests
    {

        [Fact]
        public void GetEncryptPassword()
        {
            //EEE20F30B1BBED9EE160F5882A35129A2C4D9087

            Console.WriteLine(SecurityHelper.EncryptPassword("123456"));
        }


        [Fact]
        public void AddAdmin()
        {
            var sw = new Stopwatch();
            TimeSpan timeCost;

            using (var unitOfWork = new RuicoUnitOfWork())
            {
                sw.Start();

                #region AddOrUpdate

                const string loginName = "admin";
                const string password = "123456";

                var user = unitOfWork.Users.FirstOrDefault(x => x.LoginName.Equals(loginName));

                if (user == null)
                {
                    user = new User()
                    {
                        Id = IdentityGenerator.NewSequentialGuid(),
                        Name = "管理员",
                        LoginName = loginName,
                        LoginPwd = SecurityHelper.EncryptPassword(password),
                        Created = DateTime.UtcNow,
                        LastLogin = Const.SqlServerNullDateTime
                    };

                    unitOfWork.Users.Add(user);
                }
                else
                {
                    user.Name = "管理员";
                    user.LoginPwd = SecurityHelper.EncryptPassword(password);
                    user.Created = DateTime.UtcNow;
                }

                #endregion

                unitOfWork.DbContext.SaveChanges();

                sw.Stop();
                timeCost = sw.Elapsed;
            }

            Console.WriteLine("Elapsed: " + timeCost.Ticks);
        }
    }
}
