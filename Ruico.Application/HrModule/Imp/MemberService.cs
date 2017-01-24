using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.HrModule.Entities;
using Ruico.Domain.HrModule.Repositories;
using Ruico.Domain.Weixin.Service;
using Ruico.Dto.Hr;
using Ruico.Dto.Hr.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.HrModule.Imp
{
    public class MemberService : IMemberService
    {
        IDepartmentRepository _DepartmentRepository;
        IMemberRepository _Repository;
        IContactsService _contactsService;
        ICommonService _commonService;

        #region Constructors

        public MemberService(IDepartmentRepository departmentRepository,
            IMemberRepository repository,
            IContactsService contactsService,
            ICommonService commonService)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _DepartmentRepository = departmentRepository;
            _Repository = repository;
            _contactsService = contactsService;
            _commonService = commonService;
        }

        #endregion

        private void ValidateModel(Member model)
        {
            if (model.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (model.Userid.IsNullOrBlank())
            {
                throw new DefinedException(HrMessagesResources.Member_UserId_Empty);
            }

            if (_Repository.Exists(model))
            {
                throw new DataExistsException(string.Format(HrMessagesResources.Member_Exists_WithValue, 
                    string.Format("Userid:{0},WeixinId:{1},Email{2}", model.Userid, model.WeixinId, model.Email)));
            }

            if (model.Departments.Count > 0)
            {
                var departments = new List<Domain.HrModule.Entities.Department>();
                foreach (var dep in model.Departments)
                {
                    var existDep = _DepartmentRepository.Find(x => x.DepartmentId == dep.DepartmentId);
                    if (existDep != null)
                    {
                        departments.Add(existDep);
                    }
                    else
                    {
                        dep.Id = IdentityGenerator.NewSequentialGuid();
                        dep.Created = DateTime.UtcNow;
                        _DepartmentRepository.Add(dep);

                        departments.Add(dep);
                    }
                }
                model.Departments = departments;
            }
        }

        private void OperationLog(string action, MemberDTO itemDto, MemberDTO oldDto = null)
        {
            OperateRecorder.RecordOperation(itemDto.Id.ToString(), action,
                itemDto.GetOperationLog(oldDto));
        }

        public MemberDTO Add(MemberDTO itemDto)
        {
            var model = itemDto.ToModel();
            model.Id = IdentityGenerator.NewSequentialGuid();
            model.Created = DateTime.UtcNow;

            // 数据验证
            this.ValidateModel(model);

            _Repository.Add(model);

            this.OperationLog(HrMessagesResources.Add_Member, model.ToDto(),  null);

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return model.ToDto();
        }

        public void Update(MemberDTO itemDto)
        {
            //get persisted item
            var persistedModel = _Repository.Get(itemDto.Id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Member_NotExists);
            }

            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.Userid = itemDto.Userid;
            current.Name = itemDto.Name;
            current.WeixinId = itemDto.WeixinId;
            current.Position = itemDto.Position;
            current.Gender = itemDto.Gender;
            current.Mobile = itemDto.Mobile;
            current.Email = itemDto.Email;
            current.Status = itemDto.Status;
            current.Avatar = itemDto.Avatar;
            current.Enable = itemDto.Enable;

            // 数据验证
            this.ValidateModel(persistedModel);

            this.OperationLog(HrMessagesResources.Update_Member, persistedModel.ToDto(), oldDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Member_NotExists);
            }

            var memberDto = persistedModel.ToDto();

            _Repository.Remove(persistedModel);

            this.OperationLog(HrMessagesResources.Add_Member, memberDto, null);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public MemberDTO FindBy(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Member_NotExists);
            }

            return persistedModel.ToDto();
        }

        public IPagedList<MemberDTO> FindBy(string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<MemberDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public MemberDTO FindByUserId(string userId)
        {
            var persistedModel = _Repository.FindByUserId(userId);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Member_NotExists);
            }

            return persistedModel.ToDto();
        }

        public void UpdateMemberDepartments(Guid memberId, List<Guid> departmentIds)
        {
            var member = _Repository.Get(memberId);

            if (member != null)
            {
                var newDepartments = new List<Department>();
                foreach (var uid in departmentIds)
                {
                    var p = _DepartmentRepository.Get(uid);
                    if (p != null)
                    {
                        newDepartments.Add(p);
                    }
                }

                //var oldDepartments = member.Departments.ToList().Copy();

                //var roleGroupDto = member.ToDto();

                // 删除旧的用户
                member.Departments.Clear();
                // 添加新的用户
                member.Departments = newDepartments;

                //commit unit of work
                _Repository.UnitOfWork.Commit();
            }
        }

        public void DownloadMembers()
        {
            var accessToken = _commonService.GetContactsAccessToken();
            var members = _contactsService.GetMembers(accessToken);

            var departments = _DepartmentRepository.FindBy(null, null, 1, int.MaxValue);

            var sbError = new StringBuilder();
            foreach (var m in members)
            {
                try
                {
                    var dto = new MemberDTO()
                    {
                        Userid = m.Userid,
                        Name = m.Name,
                        WeixinId = m.WeixinId,
                        Position = m.Position,
                        Gender = m.Gender,
                        Mobile = m.Mobile,
                        Email = m.Email,
                        Status = m.Status,
                        Avatar = m.Avatar,
                        Enable = m.Enable,
                        Departments = new List<DepartmentDTO>()
                    };
                    if (m.Department.Count > 0)
                    {
                        foreach (var departmentId in m.Department)
                        {
                            var department = departments.FirstOrDefault(x => x.DepartmentId == departmentId);
                            if (department != null)
                            {
                                dto.Departments.Add(department.ToDto());
                            }
                        }
                    }
                    var model = _Repository.Find(x => x.Userid == m.Userid);
                    if (model == null)
                    {
                        this.Add(dto);
                    }
                    else
                    {
                        dto.Id = model.Id;
                        dto.Created = model.Created;
                        this.Update(dto);
                    }
                }
                catch (Exception ex)
                {
                    sbError.AppendLine(string.Format("{0} {1}", m.Name, ex.Message));
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }

        public void UploadMembers()
        {
            var list = _Repository.FindBy(null, 1, int.MaxValue);

            var accessToken = _commonService.GetContactsAccessToken();
            var members = _contactsService.GetMembers(accessToken);

            var sbError = new StringBuilder();

            foreach (var member in list)
            {
                if (string.IsNullOrWhiteSpace(member.Userid))
                {
                    continue;
                }

                try
                {
                    var item = members.FirstOrDefault(x => x.Userid == member.Userid);
                    var model = new Domain.Weixin.Model.Member(member.Userid, member.Name, member.WeixinId)
                    {
                        Department = new List<int>(member.Departments.Select(x => x.DepartmentId)),
                        Position = member.Position,
                        Gender = member.Gender,
                        Mobile = member.Mobile,
                        Email = member.Email,
                        Status = member.Status,
                        Avatar = member.Avatar,
                        Enable = member.Status == 2 ? 0 : 1
                    };

                    if (item != null)
                    {
                        _contactsService.UpdateMember(accessToken, model);
                    }
                    else
                    {
                        _contactsService.CreateMember(accessToken, model);
                    }
                }
                catch (Exception ex)
                {
                    sbError.AppendLine(string.Format("{0} {1}", member.Userid, ex.Message));
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }

        public void RemoveNotExistMemberInWeixin()
        {
            var list = _Repository.FindBy(null, 1, int.MaxValue);

            var accessToken = _commonService.GetContactsAccessToken();
            var members = _contactsService.GetMembers(accessToken);

            var sbError = new StringBuilder();

            var deletedList = members.Where(x => !string.IsNullOrWhiteSpace(x.Userid) && !list.Select(d => d.Userid).Contains(x.Userid));

            foreach (var member in deletedList)
            {
                try
                {
                    _contactsService.DeleteMember(accessToken, member.Userid);
                }
                catch (Exception ex)
                {
                    sbError.AppendLine(string.Format("{0} {1}", member.Userid, ex.Message));
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }

        public void InviteMember(List<string> userIds)
        {
            var accessToken = _commonService.GetContactsAccessToken();

            var sbError = new StringBuilder();

            foreach (var userId in userIds)
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    continue;
                }

                try
                {
                    _contactsService.InviteMember(accessToken, userId);
                }
                catch (Exception ex)
                {
                    sbError.AppendLine(string.Format("{0} {1}", userId, ex.Message));
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }
    }
}
