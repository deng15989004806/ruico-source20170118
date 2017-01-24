﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.BaseModule.Entities
{
    /// <summary>
    /// 货品
    /// </summary>
    [Table("base.Cargo")]
    public class Cargo : EntityBase
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
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual Category Category { get; set; }

    }
}
