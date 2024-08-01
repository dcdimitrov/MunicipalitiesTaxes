using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalitiesTaxes.Contracts
{
    public class TaxRecordCreateDto
    {
        public string Type { get; set; }

        public string ValidFrom { get; set; }

        public string ValidTo { get; set; }

        public decimal TaxValue { get; set; }
    }
}
