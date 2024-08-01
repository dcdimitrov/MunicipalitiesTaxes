using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalitiesTaxes.Contracts
{
    public class TaxRecordDto : TaxRecordCreateDto
    {
        public int Id { get; set; }
    }
}
