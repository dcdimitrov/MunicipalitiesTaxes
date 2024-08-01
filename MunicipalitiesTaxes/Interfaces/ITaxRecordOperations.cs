using MunicipalitiesTaxes.Model;

namespace MunicipalitiesTaxes.Interfaces
{
    public interface ITaxRecordOperations
    {
        /// <summary>
        /// Get most appropriate tax rate from a list of tax records for a given date
        /// </summary>
        /// <param name="taxes">List of tax records</param>
        /// <param name="date">Target date</param>
        /// <returns>The most appropriate tax rate for the date or null if no such found</returns>
        TaxRecord? CalculateTaxRecordForDate(IList<TaxRecord> taxes, DateTime date);
    }
}