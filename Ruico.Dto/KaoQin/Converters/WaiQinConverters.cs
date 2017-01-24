using AutoMapper;
using Ruico.Domain.KaoQinModule.Entities;

namespace Ruico.Dto.KaoQin.Converters
{
    public static class WaiQinConverters
    {
        static WaiQinConverters()
        {
            Mapper.CreateMap<WaiQin, WaiQinDTO>();
            Mapper.CreateMap<WaiQinDTO, WaiQin>();
        }

        public static WaiQin ToModel(this WaiQinDTO dto)
        {
            return Mapper.Map<WaiQin>(dto);
        }

        public static WaiQinDTO ToDto(this WaiQin model)
        {
            return Mapper.Map<WaiQinDTO>(model);
        }
    }
}
