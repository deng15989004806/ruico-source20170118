using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Domain.HrModule.Entities;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.KaoQinModule.Entities
{
    [Table("kaoqin.XiuJia")]
    public class XiuJia : EntityBase
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
        /// 休假起始时间
        /// </summary>
        public DateTime ActionStartTime { get; set; }

        /// <summary>
        /// 休假结束时间
        /// </summary>
        public DateTime ActionEndTime { get; set; }

        /// <summary>
        /// 休假天数
        /// </summary>
        public decimal ActionDays { get; set; }

        /// <summary>
        /// 休假小时数
        /// </summary>
        public decimal ActionHours { get; set; }

        /// <summary>
        /// 休假类型
        /// </summary>
        [MaxLength(50)]
        public string Type { get; set; }

        /// <summary>
        /// 请假原因
        /// </summary>
        [MaxLength(250)]
        public string Reason { get; set; }

        /// <summary>
        /// 部门主管意见
        /// </summary>
        [MaxLength(150)]
        public string DepartmentSupervisorOpinion { get; set; }

        /// <summary>
        /// 部门主管意见审批人Id
        /// </summary>
        [MaxLength(50)]
        public string DepartmentSupervisorOpinionApproverId { get; set; }

        /// <summary>
        /// 部门经理意见
        /// </summary>
        [MaxLength(150)]
        public string DepartmentManagerOpinion { get; set; }

        /// <summary>
        /// 部门经理意见审批人Id
        /// </summary>
        [MaxLength(50)]
        public string DepartmentManagerOpinionApproverId { get; set; }

        /// <summary>
        /// 公司领导意见
        /// </summary>
        [MaxLength(150)]
        public string CompanyLeaderOpinion { get; set; }

        /// <summary>
        /// 公司领导意见审批人Id
        /// </summary>
        [MaxLength(150)]
        public string CompanyLeaderOpinionApproverId { get; set; }

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
