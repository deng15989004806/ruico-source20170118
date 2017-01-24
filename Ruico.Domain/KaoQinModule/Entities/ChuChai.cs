using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Domain.HrModule.Entities;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.KaoQinModule.Entities
{
    [Table("kaoqin.ChuChai")]
    public class ChuChai : EntityBase
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
        /// 出差时间
        /// </summary>
        public DateTime OutTime { get; set; }

        /// <summary>
        /// 返回时间
        /// </summary>
        public DateTime InTime { get; set; }

        /// <summary>
        /// 出差地点
        /// </summary>
        [MaxLength(50)]
        public string OutPlace { get; set; }

        /// <summary>
        /// 出差工作事由
        /// </summary>
        [MaxLength(250)]
        public string OutReason { get; set; }

        /// <summary>
        /// 指派人
        /// </summary>
        [MaxLength(50)]
        public string AppointPerson { get; set; }

        /// <summary>
        /// 部门意见
        /// </summary>
        [MaxLength(150)]
        public string DepartmentOpinion { get; set; }

        /// <summary>
        /// 部门意见审批人Id
        /// </summary>
        [MaxLength(50)]
        public string DepartmentOpinionApproverId { get; set; }

        /// <summary>
        /// 总经办意见
        /// </summary>
        [MaxLength(150)]
        public string GeneralManagerOfficeOpinion { get; set; }

        /// <summary>
        /// 总经办意见审批人Id
        /// </summary>
        [MaxLength(50)]
        public string GeneralManagerOfficeOpinionApproverId { get; set; }

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
