using AutoMapper;
using Ruico.Domain.KaoQinModule.Entities;

namespace Ruico.Dto.KaoQin.Converters
{
    public static class WeiDaKaConverters
    {
        static WeiDaKaConverters()
        {
            Mapper.CreateMap<WeiDaKa, WeiDaKaDTO>();
            Mapper.CreateMap<WeiDaKaDTO, WeiDaKa>();
        }

        public static WeiDaKa ToModel(this WeiDaKaDTO dto)
        {
            return Mapper.Map<WeiDaKa>(dto);
        }

        public static WeiDaKaDTO ToDto(this WeiDaKa model)
        {
            return Mapper.Map<WeiDaKaDTO>(model);
        }
    }
}
