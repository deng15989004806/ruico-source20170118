using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.BaseModule.Entities
{
    /// <summary>
    /// 公司
    /// </summary>
    [Table("base.Company")]
    public class Company : EntityBase
    {
        public override Guid Id { get; set; }
        
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Index]
        [MaxLength(32)]
        public string Sn { get; set; }
        
        /// <summary>
        /// 公司名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [MaxLength(50)]
        public string Contact { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [MaxLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [MaxLength(50)]
        public string Fax { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(50)]
        public string Mobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(250)]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [MaxLength(50)]
        public string Postcode { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(150)]
        public string Email { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 类型,船东、货主、代理(代理是在列船东或列货主时都出现)
        /// </summary>
        public virtual Category Category { get; set; }

    }
}
