using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.WeixinModule.Entities
{
    [Table("weixin.AppMenu")]
    public class AppMenu : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Key { get; set; }

        [MaxLength(150)]
        public string Url { get; set; }

        public int SortOrder { get; set; }

        public int AppId { get; set; }

        public DateTime Created { get; set; }

        public virtual AppMenu Parent { get; set; }

        public virtual ICollection<AppMenu> Children { get; set; }
    }
}
