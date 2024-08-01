using MunicipalitiesTaxes.Contracts;
using MunicipalitiesTaxes.Model;

namespace MunicipalitiesTaxes.Extensions
{
    public static class EntityToDtoExtensions
    {
        public static MunicipalityDto ToMunicipalityDto(this Municipality entity)
        {
            return new MunicipalityDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                TaxRecords = entity.Taxes.Select(t => t.ToTaxRecordDto()).ToList()
            };
        }

        public static TaxRecordDto ToTaxRecordDto(this TaxRecord entity)
        {
            return new TaxRecordDto()
            {
                Id = entity.Id,
                TaxValue = entity.TaxValue,
                Type = entity.Type, 
                ValidFrom = entity.ValidFrom.ToString("yyyy-MM-dd"),
                ValidTo = entity.ValidTo.ToString("yyyy-MM-dd")
            };  
        }
    }
}
