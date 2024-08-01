namespace MunicipalitiesTaxes.Contracts
{
    public class MunicipalityDto : MunicipalityCreateDto
    {
        public int Id { get; set; }

        public List<TaxRecordDto> TaxRecords { get; set; }
    }
}
