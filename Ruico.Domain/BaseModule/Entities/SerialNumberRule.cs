using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.BaseModule.Entities
{
    /// <summary>
    /// 流水号规则：前缀+日期(如果有)+流水号(有长度限制)
    /// </summary>
    [Table("base.SerialNumberRule")]
    public class SerialNumberRule : EntityBase
    {
        public override Guid Id { get; set; }
        
        /// <summary>
        /// 规则名称，用于跟编码实体类型对应
        /// </summary>
        [Required]
        [Index]
        [MaxLength(20)]
        public string RuleName { get; set; }
        
        /// <summary>
        /// 生成规则前缀
        /// </summary>
        [MaxLength(10)]
        public string Prefix { get; set; }

        /// <summary>
        /// 生成规则是否使用日期
        /// </summary>
        public bool UseDateNumber { get; set; }

        /// <summary>
        /// 流水号长度，如果流水号不足长度，前面以0补齐，当流水号的长度大于这里的长度限制，则生成编码出错
        /// </summary>
        public int NumberLength { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid CreatorId { get; set; }

    }
}
