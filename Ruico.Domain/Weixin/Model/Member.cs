using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Domain.Weixin.Model
{
    public class Member
    {
        public Member(string userid, string name, string weixinId)
        {
            this.Userid = userid;
            this.Name = name;
            this.WeixinId = weixinId;
            this.Department = new List<int>();
        }

        public string Userid { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 成员所属部门id列表
        /// </summary>
        public List<int> Department { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WeixinId { get; set; }

        /// <summary>
        /// 职位信息
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 性别，0表示未定义，1表示男性，2表示女性
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 关注状态: 1=已关注，2=已冻结，4=未关注
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 头像url。注：如果要获取小图将url最后的"/0"改成"/64"即可
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 启用/禁用成员。1表示启用成员，0表示禁用成员
        /// </summary>
        public int Enable { get; set; }
    }
}
