using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalitiesTaxes.Model
{
    public class Municipality
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

        public IList<TaxRecord> Taxes { get; set; } = new List<TaxRecord>();
    }
}
