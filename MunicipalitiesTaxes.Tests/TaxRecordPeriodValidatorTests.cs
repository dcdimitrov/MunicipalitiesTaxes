using MunicipalitiesTaxes.Implementations;
using MunicipalitiesTaxes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalitiesTaxes.Tests
{
    [TestClass]
    public class TaxRecordPeriodValidatorTests
    {
        private TaxRecordPeriodValidator taxRecordPeriodValidator;

        public TaxRecordPeriodValidatorTests()
        {
            this.taxRecordPeriodValidator = new TaxRecordPeriodValidator();
        }

        [TestMethod]
        public void EmptyTaxes_AnyPeriod_PassValidator()
        {
            var isValid = this.taxRecordPeriodValidator.ValidateOverlapExactDates(this.monthlyTaxRecord, new List<TaxRecord>());
            Assert.IsTrue(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateOverlapValidFrom(this.monthlyTaxRecord, new List<TaxRecord>());
            Assert.IsTrue(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateOverlapValidTo(this.monthlyTaxRecord, new List<TaxRecord>());
            Assert.IsTrue(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(this.monthlyTaxRecord, new List<TaxRecord>());
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void PresentMonthlyTax_PeriodOverlapStartDate_FailValidator()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Weekly",
                ValidFrom = DateTime.Parse("2024-02-26"),
                ValidTo = DateTime.Parse("2024-03-05")
            };

            // newTaxRecord.ValidFrom = 2024-02-26
            // existingTaxRecord.ValidFrom = 2024-03-01
            // newTaxRecord.ValidTo = 2024-03-05
            // existingTaxRecord.ValidTo = 2024-03-31
            var isValid = this.taxRecordPeriodValidator.ValidateOverlapValidFrom(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void PresentMonthlyTax_PeriodExactStartDateAndDiffEnd_PassValidator()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Weekly",
                ValidFrom = DateTime.Parse("2024-03-01"),
                ValidTo = DateTime.Parse("2024-03-05")
            };

            // newTaxRecord.ValidFrom = 2024-03-01
            // existingTaxRecord.ValidFrom = 2024-03-01
            // newTaxRecord.ValidTo = 2024-03-05
            // existingTaxRecord.ValidTo = 2024-03-31
            var isValid = this.taxRecordPeriodValidator.ValidateOverlapValidFrom(newTaxRecord, taxes);
            Assert.IsTrue(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void PresentMonthlyTax_PeriodOverlapEndDate_FailValidator()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Weekly",
                ValidFrom = DateTime.Parse("2024-03-26"),
                ValidTo = DateTime.Parse("2024-04-05")
            };

            // existingTaxRecord.ValidFrom = 2024-03-01
            // newTaxRecord.ValidFrom = 2024-03-26
            // existingTaxRecord.ValidTo = 2024-03-31
            // newTaxRecord.ValidTo = 2024-04-05
            var isValid = this.taxRecordPeriodValidator.ValidateOverlapValidTo(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void PresentMonthlyTax_PeriodExactEndDateAndDiffStart_PassValidator()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Weekly",
                ValidFrom = DateTime.Parse("2024-03-26"),
                ValidTo = DateTime.Parse("2024-03-31")
            };

            // existingTaxRecord.ValidFrom = 2024-03-01
            // newTaxRecord.ValidFrom = 2024-03-26
            // existingTaxRecord.ValidTo = 2024-03-31
            // newTaxRecord.ValidTo = 2024-03-31
            var isValid = this.taxRecordPeriodValidator.ValidateOverlapValidTo(newTaxRecord, taxes);
            Assert.IsTrue(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void PresentMonthlyTax_PeriodIsSame_FailValidator()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Monthly",
                ValidFrom = DateTime.Parse("2024-03-01"),
                ValidTo = DateTime.Parse("2024-03-31")
            };

            var isValid = this.taxRecordPeriodValidator.ValidateOverlapExactDates(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
            isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void PresentMonthlyTax_SingleDayTax_AlwaysValid()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Monthly",
                ValidFrom = DateTime.Parse("2024-03-01"),
                ValidTo = DateTime.Parse("2024-03-01")
            };

            var isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void PresentDailyTax_SingleDayTaxDuplicate_FailValidator()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.dailyTaxRecord);
            var newTaxRecord = new TaxRecord()
            {
                MunicipalityId = 1,
                TaxValue = 0.5m,
                Type = "Monthly",
                ValidFrom = DateTime.Parse("2024-03-01"),
                ValidTo = DateTime.Parse("2024-03-01")
            };

            var isValid = this.taxRecordPeriodValidator.ValidateNewTaxRecordPeriod(newTaxRecord, taxes);
            Assert.IsFalse(isValid);
        }

        private TaxRecord monthlyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.4m,
            Type = "Monthly",
            ValidFrom = DateTime.Parse("2024-03-01"),
            ValidTo = DateTime.Parse("2024-03-31")
        };

        private TaxRecord dailyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.4m,
            Type = "Monthly",
            ValidFrom = DateTime.Parse("2024-03-01"),
            ValidTo = DateTime.Parse("2024-03-01")
        };
    }
}
