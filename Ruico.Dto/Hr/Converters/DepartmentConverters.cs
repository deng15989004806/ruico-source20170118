using AutoMapper;
using Ruico.Domain.HrModule.Entities;

namespace Ruico.Dto.Hr.Converters
{
    public static class DepartmentConverters
    {
        static DepartmentConverters()
        {
            Mapper.CreateMap<Department, DepartmentDTO>();
            Mapper.CreateMap<DepartmentDTO, Department>();
        }

        public static Department ToModel(this DepartmentDTO dto)
        {
            return Mapper.Map<Department>(dto);
        }

        public static DepartmentDTO ToDto(this Department model)
        {
            return Mapper.Map<DepartmentDTO>(model);
        }
    }
}
