using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.BaseModule.Entities
{
    /// <summary>
    /// 通用分类（类型）管理
    /// </summary>
    [Table("base.Category")]
    public class Category : EntityBase
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

        public int SortOrder { get; set; }

        public int Depth { get; set; }

        /// <summary>
        /// 子级编码规则前缀
        /// </summary>
        [MaxLength(10)]
        public string ChildSnRulePrefix { get; set; }

        /// <summary>
        /// 子级编码规则流水号长度，如果流水号不足长度，前面以0补齐，当流水号的长度大于这里的长度限制，则生成编码出错
        /// </summary>
        public int ChildSnRuleNumberLength { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid CreatorId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }
    }
}
