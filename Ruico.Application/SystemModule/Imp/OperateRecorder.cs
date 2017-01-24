using System;
using System.Web.Mvc;
using log4net;
using Ruico.Dto.System;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Authorize;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.SystemModule.Imp
{
    public partial class OperateRecorder
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(OperateRecorder));

        protected static IServiceResolver ServiceResolver
        {
            get { return (IServiceResolver)DependencyResolver.Current.GetService(typeof(IServiceResolver)); }
        }
        
        protected static IAuthorizeManager AuthorizeManager
        {
            get { return ServiceResolver.Resolve<IAuthorizeManager>(); }
        }
        
        protected static IOperateRecordService OperateRecordService
        {
            get { return ServiceResolver.Resolve<IOperateRecordService>(); }
        }

        private static OperatorDTO FakeOperator
        {
            get { return new OperatorDTO() {OperatorName = "N/A", UserId = Guid.Empty}; }
        }

        private static void SaveOperateRecord(OperateRecordDTO record, bool commit = true)
        {
            OperateRecordService.Add(record, commit);
        }

        public static OperatorDTO GetCurrentOperator()
        {
            var currentUser = AuthorizeManager.GetCurrentUserInfo();

            if (currentUser != null)
            {
                return new OperatorDTO()
                {
                    UserId = currentUser.UserId,
                    OperatorName = currentUser.UserName
                };
            }

            return FakeOperator;
        }

        /// <summary>
        /// 同步写操作日志
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        /// <param name="user"></param>
        /// <param name="commit"></param>
        public static void RecordOperation(string sn, string operation, string message,
            UserDTO user = null, bool commit = false)
        {
            if (sn.IsNullOrBlank())
            {
                throw new Exception("sn can not be empty");
            }

            if (sn == Guid.Empty.ToString())
            {
                throw new Exception("sn can not be Guid.Empty");
            }

            var ip = HttpHelper.GetRealIP();
            OperatorDTO @operator = null;
            if (user == null)
            {
                @operator = GetCurrentOperator();
            }
            else
            {
                @operator = new OperatorDTO()
                {
                    UserId = user.Id,
                    OperatorName = user.Name
                };
            }

            var record = new OperateRecordDTO()
            {
                Sn = sn,
                UserId = @operator.UserId == Guid.Empty ? (Guid?)null : @operator.UserId,
                OperatorName = @operator.OperatorName,
                Operation = operation,
                Message = message,
                Ip = ip
            };

            SaveOperateRecord(record, commit);
        }
    }
}