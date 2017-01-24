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
using Ruico.Dto.UserSystem;
using Ruico.Dto.UserSystem.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.UserSystemModule.Imp
{
    public class RoleService : IRoleService
    {
        IRoleRepository _Repository;
        IRoleGroupRepository _RoleGroupRepository;
        IPermissionRepository _PermissionRepository;

        #region Constructors

        public RoleService(IRoleRepository repository, IRoleGroupRepository _roleGroupRepository, IPermissionRepository permissionRepository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _RoleGroupRepository = _roleGroupRepository;
            _PermissionRepository = permissionRepository;
        }

        #endregion

        public RoleDTO Add(RoleDTO roleDTO)
        {
            var role = roleDTO.ToModel();
            role.Id = IdentityGenerator.NewSequentialGuid();
            role.Created = DateTime.UtcNow;

            var group = _RoleGroupRepository.Get(roleDTO.RoleGroupId);
            if (group == null)
            {
                throw new DataExistsException(UserSystemMessagesResources.RoleGroup_NotExists);
            }
            role.RoleGroup = group;

            if (role.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (_Repository.Exists(role))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.Role_Exists_WithValue, role.Name));
            }

            _Repository.Add(role);

            #region 操作日志

            var roleDto = role.ToDto();

            OperateRecorder.RecordOperation(roleDto.Id.ToString(),
                UserSystemMessagesResources.Add_Role,
                roleDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return role.ToDto();
        }

        public void Update(RoleDTO roleDTO)
        {
            var role = _Repository.Get(roleDTO.Id);

            if (role == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Role_NotExists);
            }

            var oldDTO = role.ToDto();

            var current = roleDTO.ToModel();
            role.Name = current.Name;
            role.Description = current.Description;
            role.SortOrder = current.SortOrder;

            if (role.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (_Repository.Exists(role))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.Role_Exists_WithValue, role.Name));
            }

            #region 操作日志

            var roleDto = role.ToDto();

            OperateRecorder.RecordOperation(oldDTO.Id.ToString(),
                UserSystemMessagesResources.Update_Role,
                roleDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var role = _Repository.Get(id);

            if (role == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Role_NotExists);
            }

            var roleDto = role.ToDto();

            _Repository.Remove(role);

            #region 操作日志

            OperateRecorder.RecordOperation(roleDto.Id.ToString(),
                UserSystemMessagesResources.Remove_Role,
                roleDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public List<RoleDTO> FindAll()
        {
            return _Repository.FindAll().OrderBy(x => x.SortOrder)
                .Select(x => x.ToDto()).ToList();
        }

        public RoleDTO FindBy(Guid id)
        {
            return _Repository.Get(id).ToDto();
        }

        public IPagedList<RoleDTO> FindBy(Guid roleGroupId, string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(roleGroupId, name, pageNumber, pageSize);
            return new StaticPagedList<RoleDTO>(
               list.ToList().Select(x => x.ToDto()),
               pageNumber,
               pageSize,
               list.TotalItemCount);
        }

        public void UpdateRolePermission(Guid id, List<Guid> permissions)
        {
            var role = _Repository.Get(id);

            if (role != null)
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

                var oldPermissions = role.Permissions.ToList().Copy();

                var roleDto = role.ToDto();

                // 删除旧的权限
                role.Permissions.Clear();
                // 添加新的权限
                role.Permissions = newPermissions;

                #region 操作日志

                var addPermissions = newPermissions.Except(oldPermissions).ToList();
                var removePermissions = oldPermissions.Except(newPermissions).ToList();

                if (addPermissions.Any())
                {
                    OperateRecorder.RecordOperation(roleDto.Id.ToString(),
                        UserSystemMessagesResources.Add_RolePermission,
                        roleDto.GetOperationLogForUpdatePermission(addPermissions));
                }
                if (removePermissions.Any())
                {
                    OperateRecorder.RecordOperation(roleDto.Id.ToString(),
                        UserSystemMessagesResources.Remove_RolePermission,
                        roleDto.GetOperationLogForUpdatePermission(removePermissions));
                }

                #endregion

                //commit unit of work
                _Repository.UnitOfWork.Commit();
            }
        }

        public List<PermissionDTO> GetRolePermission(Guid id)
        {
            var role = _Repository.Get(id);

            if (role != null)
            {
                return role.Permissions.Select(x => x.ToDto()).ToList();
            }

            return new List<PermissionDTO>();
        }
    }
}
