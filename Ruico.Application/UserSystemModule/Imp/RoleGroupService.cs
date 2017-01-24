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
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.UserSystemModule.Imp
{
    public class RoleGroupService : IRoleGroupService
    {
        IRoleGroupRepository _Repository;
        IUserRepository _UserRepository;

        #region Constructors

        public RoleGroupService(IRoleGroupRepository repository,IUserRepository userRepository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _UserRepository = userRepository;
        }

        #endregion

        public RoleGroupDTO Add(RoleGroupDTO roleGroupDTO)
        {
            var roleGroup = roleGroupDTO.ToModel();
            roleGroup.Id = IdentityGenerator.NewSequentialGuid();
            roleGroup.Created = DateTime.UtcNow;

            if (roleGroup.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (_Repository.Exists(roleGroup))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.RoleGroup_Exists_WithValue, roleGroup.Name));
            }

            _Repository.Add(roleGroup);

            #region 操作日志

            var roleGroupDto = roleGroup.ToDto();

            OperateRecorder.RecordOperation(roleGroupDto.Id.ToString(),
                UserSystemMessagesResources.Add_RoleGroup,
                roleGroupDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return roleGroup.ToDto();
        }

        public void Update(RoleGroupDTO roleGroupDTO)
        {
            var group = _Repository.Get(roleGroupDTO.Id);

            if (group == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.RoleGroup_NotExists);
            }

            var oldDTO = group.ToDto();

            var current = roleGroupDTO.ToModel();
            group.Name = current.Name;
            group.Description = current.Description;
            group.SortOrder = current.SortOrder;

            if (group.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (_Repository.Exists(group))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.RoleGroup_Exists_WithValue, group.Name));
            }

            #region 操作日志

            var groupDto = group.ToDto();

            OperateRecorder.RecordOperation(oldDTO.Id.ToString(),
                UserSystemMessagesResources.Update_RoleGroup,
                groupDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var roleGroup = _Repository.Get(id);

            if (roleGroup == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.RoleGroup_NotExists);
            }

            _Repository.Remove(roleGroup);

            #region 操作日志

            var roleGroupDto = roleGroup.ToDto();

            OperateRecorder.RecordOperation(roleGroupDto.Id.ToString(),
                UserSystemMessagesResources.Remove_RoleGroup,
                roleGroupDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public List<RoleGroupDTO> FindAll()
        {
            return _Repository.FindAll().OrderBy(x => x.SortOrder)
                .Select(x => x.ToDto()).ToList();
        }

        public RoleGroupDTO FindBy(Guid id)
        {
            return _Repository.Get(id).ToDto();
        }

        public IPagedList<RoleGroupDTO> FindBy(string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, pageNumber, pageSize);
            return new StaticPagedList<RoleGroupDTO>(
               list.ToList().Select(x => x.ToDto()),
               pageNumber,
               pageSize,
               list.TotalItemCount);
        }

        public List<IdNameDTO> GetUsersIdName(Guid groupId)
        {
            var group = _Repository.Get(groupId);

            if (group == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.RoleGroup_NotExists);
            }

            var result = group.Users.Select(x => new IdNameDTO()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return result;
        }
        public void UpdateGroupUsers(Guid id, List<Guid> users)
        {
            var group = _Repository.Get(id);

            if (group != null)
            {
                var newUsers = new List<User>();
                foreach (var uid in users)
                {
                    var p = _UserRepository.Get(uid);
                    if (p != null)
                    {
                        newUsers.Add(p);
                    }
                }

                var oldUsers = group.Users.ToList().Copy();

                var roleGroupDto = group.ToDto();

                // 删除旧的用户
                group.Users.Clear();
                // 添加新的用户
                group.Users = newUsers;

                #region 操作日志

                var addUsers = newUsers.Except(oldUsers).ToList();
                var removeUsers = oldUsers.Except(newUsers).ToList();

                if (addUsers.Any())
                {
                    OperateRecorder.RecordOperation(roleGroupDto.Id.ToString(),
                        UserSystemMessagesResources.Add_RoleGroupUser,
                        roleGroupDto.GetOperationLogForUpdateUser(addUsers));
                }
                if (removeUsers.Any())
                {
                    OperateRecorder.RecordOperation(roleGroupDto.Id.ToString(),
                        UserSystemMessagesResources.Remove_RoleGroupUser,
                        roleGroupDto.GetOperationLogForUpdateUser(removeUsers));
                }

                #endregion

                //commit unit of work
                _Repository.UnitOfWork.Commit();
            }
        }
    }
}
