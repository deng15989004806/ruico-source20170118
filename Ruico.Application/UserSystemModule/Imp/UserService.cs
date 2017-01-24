using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Dto.Common;
using Ruico.Dto.UserSystem;
using Ruico.Dto.UserSystem.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.UserSystemModule.Imp
{
    public class UserService : IUserService
    {
        IUserRepository _Repository;
        IPermissionRepository _PermissionRepository;

        #region Constructors

        public UserService(IUserRepository repository, IPermissionRepository permissionRepository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _PermissionRepository = permissionRepository;
        }

        #endregion

        public UserDTO Add(UserDTO userDTO)
        {
            var user = userDTO.ToModel();
            user.Id = IdentityGenerator.NewSequentialGuid();
            user.Created = DateTime.UtcNow;
            user.LastLogin = Const.SqlServerNullDateTime;

            if (user.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (user.Email.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Email_Empty);
            }

            if (user.LoginName.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.LoginName_Empty);
            }

            if (_Repository.ExistsName(user))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.User_Exists_Name_WithValue, user.Name));
            }

            if (_Repository.ExistsLoginName(user))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.User_Exists_LoginName_WithValue, user.LoginName));
            }

            if (_Repository.ExistsEmail(user))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.User_Exists_Email, user.Email));
            }

            user.LoginPwd = AuthService.EncryptPassword(user.LoginPwd);
            _Repository.Add(user);

            #region 操作日志

            var userDto = user.ToDto();

            OperateRecorder.RecordOperation(userDto.Id.ToString(),
                UserSystemMessagesResources.Add_User,
                userDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return user.ToDto();
        }

        public void Update(UserDTO userDTO)
        {
            var user = _Repository.Get(userDTO.Id);

            if (user == null) //if customer exist
            {
                throw new DataNotFoundException(UserSystemMessagesResources.User_NotExists);
            }

            var oldDTO = user.ToDto();

            var current = userDTO.ToModel();

            //最后登录时间字段为空时，数据为datetime默认的{0001/1/1 0:00:00}，新增或修改用户时报错
            user.LastLogin = current.LastLogin;
            if (user.LastLogin == DateTime.MinValue)
            {
                user.LastLogin = Const.SqlServerNullDateTime;
            }

            user.Name = current.Name;
            user.LoginName = current.LoginName;
            user.Email = current.Email;

            if (user.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (user.Email.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Email_Empty);
            }

            if (user.LoginName.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.LoginName_Empty);
            }

            if (_Repository.ExistsName(user))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.User_Exists_Name_WithValue, user.Name));
            }

            if (_Repository.ExistsLoginName(user))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.User_Exists_LoginName_WithValue, user.LoginName));
            }

            if (_Repository.ExistsEmail(user))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.User_Exists_Email, user.Email));
            }

            #region 操作日志

            var userDto = user.ToDto();

            OperateRecorder.RecordOperation(oldDTO.Id.ToString(),
                UserSystemMessagesResources.Update_User,
                userDto.GetOperationLog(oldDTO));

            #endregion
            
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var user = _Repository.Get(id);

            if (user == null) //if exist
            {
                throw new DataNotFoundException(UserSystemMessagesResources.User_NotExists);
            }

            var userDto = user.ToDto();

            _Repository.Remove(user);

            #region 操作日志

            OperateRecorder.RecordOperation(userDto.Id.ToString(),
                UserSystemMessagesResources.Remove_User,
                userDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }
        public UserDTO FindBy(Guid id)
        {
            return _Repository.Get(id).ToDto();
        }


        public IPagedList<UserDTO> FindBy(string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, pageNumber, pageSize);
            return new StaticPagedList<UserDTO>(
               list.ToList().Select(x => x.ToDto()),
               pageNumber,
               pageSize,
               list.TotalItemCount);
        }

        public void UpdateUserPermission(Guid id, List<Guid> permissions)
        {
            var user = _Repository.Get(id);

            if (user != null)
            {
                var newPermissions = new List<Permission>();
                foreach (var pid in permissions)
                {
                    var p = _PermissionRepository.Get(pid);
                    if (p != null)
                    {
                        newPermissions.Add(p);
                    }
                }

                var oldPermissions = user.Permissions.ToList().Copy();

                var userDto = user.ToDto();

                // 删除旧的权限
                user.Permissions.Clear();
                // 添加新的权限
                user.Permissions = newPermissions;

                #region 操作日志

                var addPermissions = newPermissions.Except(oldPermissions).ToList();
                var removePermissions = oldPermissions.Except(newPermissions).ToList();

                if (addPermissions.Any())
                {
                    OperateRecorder.RecordOperation(userDto.Id.ToString(),
                        UserSystemMessagesResources.Add_UserPermission,
                        userDto.GetOperationLogForUpdatePermission(addPermissions));
                }
                if (removePermissions.Any())
                {
                    OperateRecorder.RecordOperation(userDto.Id.ToString(),
                        UserSystemMessagesResources.Remove_UserPermission,
                        userDto.GetOperationLogForUpdatePermission(removePermissions));
                }

                #endregion

                //commit unit of work
                _Repository.UnitOfWork.Commit();
            }
        }

        public List<PermissionDTO> GetUserPermission(Guid id)
        {
            var user = _Repository.Get(id);

            if (user != null)
            {
                return user.Permissions.Select(x => x.ToDto()).ToList();
            }

            return new List<PermissionDTO>();
        }

        public List<IdNameDTO> GetAllUsersIdName()
        {
            return _Repository.Collection.Select(x => new IdNameDTO()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
