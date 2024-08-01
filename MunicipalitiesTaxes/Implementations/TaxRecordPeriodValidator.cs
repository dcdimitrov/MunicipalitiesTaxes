using MunicipalitiesTaxes.Model;

namespace MunicipalitiesTaxes.Implementations
{
    public class TaxRecordPeriodValidator
    {
        public bool ValidateNewTaxRecordPeriod(TaxRecord newTaxRecord, List<TaxRecord> existingTaxRecords)
        {
            if (newTaxRecord.ValidFrom > newTaxRecord.ValidTo)
            {
                return false;
            }

            if (this.ValidateOverlapValidTo(newTaxRecord, existingTaxRecords) == false)
            {
                return false;
            }

            if (this.ValidateOverlapValidFrom(newTaxRecord, existingTaxRecords) == false)
            {
                return false;
            }

            if (this.ValidateOverlapExactDates(newTaxRecord, existingTaxRecords) == false)
            {
                return false;
            }


            return true;
        }

        /// <summary>
        /// Check if newTaxRecord period overlaps with existing period in the end
        /// existingTaxRecord.validFrom < newTaxRecord.validFrom < existingTaxRecord.validTo < newTaxRecord.validTo
        /// </summary>
        public bool ValidateOverlapValidTo(TaxRecord newTaxRecord, List<TaxRecord> existingTaxRecords)
        {
            return !existingTaxRecords.Any(t => t.ValidFrom < newTaxRecord.ValidFrom && newTaxRecord.ValidFrom < t.ValidTo && newTaxRecord.ValidTo > t.ValidTo);
        }

        /// <summary>
        /// Check if newTaxRecord period overlaps with existing period in the beginning
        /// newTaxRecord.validFrom < existingTaxRecord.validFrom < newTaxRecord.validTo < existingTaxRecord.validTo
        /// </summary>
        public bool ValidateOverlapValidFrom(TaxRecord newTaxRecord, List<TaxRecord> existingTaxRecords)
        {
            return !existingTaxRecords.Any(t => t.ValidFrom < newTaxRecord.ValidTo && newTaxRecord.ValidTo < t.ValidTo && newTaxRecord.ValidFrom < t.ValidFrom);
        }

        /// <summary>
        /// Check if newTaxRecord period overlaps with existing period with exact same from-to
        /// newTaxRecord.validFrom = existingTaxRecord.validFrom AND newTaxRecord.validTo = existingTaxRecord.validTo
        /// </summary>
        public bool ValidateOverlapExactDates(TaxRecord newTaxRecord, List<TaxRecord> existingTaxRecords)
        {
            return !existingTaxRecords.Any(t => t.ValidFrom.Date == newTaxRecord.ValidFrom.Date && newTaxRecord.ValidTo.Date == t.ValidTo.Date);
        }
    }
}
