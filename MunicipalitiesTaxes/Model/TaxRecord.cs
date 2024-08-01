using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalitiesTaxes.Model
{
    public class TaxRecord
    {
        public int Id { get; set; }

        public int MunicipalityId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Type { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ValidTo { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal TaxValue { get; set; }
    }
}
