using Microsoft.EntityFrameworkCore;
using MunicipalitiesTaxes.Contracts;
using MunicipalitiesTaxes.Database;
using MunicipalitiesTaxes.Interfaces;
using MunicipalitiesTaxes.Model;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MunicipalitiesTaxes.Implementations
{
    public class MunicipalitiesManager
    {
        public MunicipalitiesManager(IGenericRepository<MunicipalitiesTaxesDbContext, Municipality> repository, ITaxRecordOperations taxRecordCalculator)
        {
            this.Repository = repository;
            this.taxRecordOperations = taxRecordCalculator;
            this.taxRecordPeriodValidator = new TaxRecordPeriodValidator();
        }

        public Municipality AddMunicipality(Municipality newMunicipality)
        {
            var municiaplityExists = this.Repository.GetAll().Any(m => m.Name.ToLower() == newMunicipality.Name.ToLower());
            if (municiaplityExists)
            {
                throw new FileNotFoundException($"Municipality with name {newMunicipality.Name} already exists");
            }

            this.Repository.Add(newMunicipality);
            this.Repository.Save();

            return newMunicipality;
        }

        public Municipality AddTaxRecordForMunicipality(int municipalityId, TaxRecord newTaxRecord)
        {
            var municipality = this.GetAllMunicipalities().SingleOrDefault(m => m.Id == municipalityId);
            if (municipality == null)
            {
                throw new Exception($"Municipality with id {municipality} was not found");
            }

            if (this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, municipality.Taxes.ToList()) == false)
            {
                throw new Exception($"Invalid period for new tax record");
            }

            if (newTaxRecord.TaxValue < 0 || newTaxRecord.TaxValue > 1)
            {
                throw new Exception("Tax Value must be between 0 and 1!");
            }

            newTaxRecord.MunicipalityId = municipalityId;

            municipality.Taxes.Add(newTaxRecord);
            this.Repository.Update(municipality);
            this.Repository.Save();

            return municipality;
        }

        public Municipality UpdateTaxRecordForMunicipality(int municipalityId, int taxRecordId, TaxRecord updateTaxRecord)
        {
            var municipality = this.GetAllMunicipalities().SingleOrDefault(m => m.Id == municipalityId);
            if (municipality == null)
            {
                throw new Exception($"Municipality with id {municipality} was not found");
            }

            var existingTaxRecord = municipality.Taxes.SingleOrDefault(t => t.Id == taxRecordId); 
            if (existingTaxRecord == null)
            {
                throw new Exception("Could not find existing tax record to update");
            }

            if (updateTaxRecord.TaxValue < 0 || updateTaxRecord.TaxValue > 1)
            {
                throw new Exception("Tax Value must be between 0 and 1!");
            }

            municipality.Taxes.Remove(existingTaxRecord);
            
            if (this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(updateTaxRecord, municipality.Taxes.ToList()) == false)
            {
                throw new Exception($"Invalid period for new tax record");
            }

            updateTaxRecord.Id = taxRecordId;
            updateTaxRecord.MunicipalityId = municipalityId;

            municipality.Taxes.Add(updateTaxRecord);
            this.Repository.Update(municipality);
            this.Repository.Save();


            return municipality;
        }

        public IQueryable<Municipality> GetAllMunicipalities()
        {
            return this.Repository.GetAll().Include(m => m.Taxes);            
        }

        public Municipality GetTaxByMunicipalityNameAndDate(string municipalityName, DateTime date)
        {
            var municipality = this.GetAllMunicipalities().SingleOrDefault(m => m.Name.ToLower().Equals(municipalityName.ToLower()));
            if (municipality == null)
            {
                throw new Exception($"Municipality with name {municipalityName} not found");
            }

            var tax = this.taxRecordOperations.CalculateTaxRecordForDate(municipality.Taxes, date);

            if (tax == null)
            {
                throw new Exception($"Tax not found for given date for municipality {municipalityName}");
            }

            municipality.Taxes = new List<TaxRecord>() { tax };
            return municipality;
        }

        protected IGenericRepository<MunicipalitiesTaxesDbContext, Municipality> Repository { get; }
        private ITaxRecordOperations taxRecordOperations;
        private TaxRecordPeriodValidator taxRecordPeriodValidator;
    }
}
