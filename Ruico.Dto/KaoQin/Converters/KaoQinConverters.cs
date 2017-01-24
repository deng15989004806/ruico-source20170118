using AutoMapper;
using Ruico.Domain.KaoQinModule.Entities;

namespace Ruico.Dto.KaoQin.Converters
{
    public static class KaoQinConverters
    {
        static KaoQinConverters()
        {
            Mapper.CreateMap<KaoQinCondition, KaoQinConditionDTO>();
            Mapper.CreateMap<KaoQinConditionDTO, KaoQinCondition>();
        }

        public static KaoQinCondition ToModel(this KaoQinConditionDTO dto)
        {
            return Mapper.Map<KaoQinCondition>(dto);
        }

        public static KaoQinConditionDTO ToDto(this KaoQinCondition model)
        {
            return Mapper.Map<KaoQinConditionDTO>(model);
        }
    }
}
