using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Domain.HrModule.Entities;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.KaoQinModule.Entities
{
    [Table("kaoqin.WeiDaKa")]
    public class WeiDaKa : EntityBase
    {
        public override Guid Id { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [MaxLength(50)]
        public string Department { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [MaxLength(50)]
        public string Position { get; set; }

        /// <summary>
        /// 填写时间
        /// </summary>
        public DateTime InputTime { get; set; }

        /// <summary>
        /// 未打卡时间
        /// </summary>
        public DateTime ActionTime { get; set; }

        /// <summary>
        /// 未打卡类型
        /// </summary>
        [MaxLength(50)]
        public string Type { get; set; }

        /// <summary>
        /// 未打卡原因
        /// </summary>
        [MaxLength(250)]
        public string Reason { get; set; }

        /// <summary>
        /// 部门/公司意见
        /// </summary>
        [MaxLength(150)]
        public string DepartmentOrCompanyOpinion { get; set; }

        /// <summary>
        /// 部门/公司意见审批人Id
        /// </summary>
        [MaxLength(50)]
        public string DepartmentOrCompanyOpinionApproverId { get; set; }

        /// <summary>
        /// 提交状态
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime Canceled { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime Approved { get; set; }
    }
}
