using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.HrModule.Entities
{
    [Table("hr.Member")]
    public class Member : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Userid { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [MaxLength(50)]
        public string WeixinId { get; set; }

        /// <summary>
        /// 职位信息
        /// </summary>
        [MaxLength(50)]
        public string Position { get; set; }

        /// <summary>
        /// 性别，0表示未定义，1表示男性，2表示女性
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(50)]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(150)]
        public string Email { get; set; }

        /// <summary>
        /// 关注状态: 1=已关注，2=已冻结，4=未关注
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 头像url。注：如果要获取小图将url最后的"/0"改成"/64"即可
        /// </summary>
        [MaxLength(300)]
        public string Avatar { get; set; }

        /// <summary>
        /// 启用/禁用成员。1表示启用成员，0表示禁用成员
        /// </summary>
        public int Enable { get; set; }

        public DateTime Created { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
