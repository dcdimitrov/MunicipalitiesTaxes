using MunicipalitiesTaxes.Interfaces;
using MunicipalitiesTaxes.Model;

namespace MunicipalitiesTaxes.Implementations
{
    public class TaxRecordOperations : ITaxRecordOperations
    {
        /// <summary>
        /// Get the most appropriate tax rate for a given date based on which tax record period is most concrete and the target date is part of it.
        /// Most concrete means with the smalles duration - order starts from single day duration (daily taxes) 
        /// </summary>
        /// <param name="taxes"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public TaxRecord? CalculateTaxRecordForDate(IList<TaxRecord> taxes, DateTime date)
        {
            TaxRecord? result = null;
            var smallesDurationDays = Double.MaxValue;
            for (int i = 0; i < taxes.Count; i++)
            {
                var currentTaxRecord = taxes[i];
                if (currentTaxRecord.ValidFrom.Date <= date.Date && date.Date <= currentTaxRecord.ValidTo.Date)
                {
                    var currentTaxRecordDurationDays = (currentTaxRecord.ValidTo - currentTaxRecord.ValidFrom).TotalDays;
                    if (currentTaxRecordDurationDays < smallesDurationDays)
                    {
                        result = currentTaxRecord;
                        smallesDurationDays = currentTaxRecordDurationDays;
                    }
                }
            }

            return result;
        }
    }
}
