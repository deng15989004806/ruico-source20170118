using System.Linq;
using AutoMapper;
using Ruico.Domain.HrModule.Entities;

namespace Ruico.Dto.Hr.Converters
{
    public static class MemberConverters
    {
        static MemberConverters()
        {
            Mapper.CreateMap<Member, MemberDTO>()
                .ForMember(x => x.Departments, opt => opt.MapFrom(s => s.Departments.Select(x => x.ToDto()).ToList()));
            Mapper.CreateMap<MemberDTO, Member>()
                .ForMember(x => x.Departments, opt => opt.MapFrom(s => s.Departments.Select(x => x.ToModel()).ToList()));
        }

        public static Member ToModel(this MemberDTO dto)
        {
            return Mapper.Map<Member>(dto);
        }

        public static MemberDTO ToDto(this Member model)
        {
            return Mapper.Map<MemberDTO>(model);
        }
    }
}
