using System;
using PagedList;
using Ruico.Dto.Hr;

namespace Ruico.Application.HrModule
{
    public interface IDepartmentService
    {
        DepartmentDTO Add(DepartmentDTO item);

        void Update(DepartmentDTO item);

        DepartmentDTO Get(int departmentId);

        void Remove(Guid id);

        DepartmentDTO FindBy(Guid id);

        IPagedList<DepartmentDTO> FindBy(int? parentId, string name, int pageNumber, int pageSize);

        void DownloadDepartments();

        void UploadDepartments();

        void RemoveNotExistDepartmentInWeixin();
    }
}
