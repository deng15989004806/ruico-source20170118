using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Domain.Weixin.Model;
using Senparc.Weixin.Entities;

namespace Ruico.Domain.Extend.Weixin
{
    public static class Extensions
    {
        public static QyCallResult ToQyCallResult(this QyJsonResult result)
        {
            return new QyCallResult()
            {
                Code = (int)result.errcode,
                CodeRemark = result.errcode.ToString(),
                Msg = result.errmsg
            };
        }
    }
}
