using Microsoft.VisualStudio.TestTools.UnitTesting;
using MunicipalitiesTaxes.Implementations;
using MunicipalitiesTaxes.Interfaces;
using MunicipalitiesTaxes.Model;

namespace MunicipalitiesTaxes.Tests
{
    [TestClass]
    public class TaxRecordOperationsTests
    {
        public TaxRecordOperationsTests()
        {
            this.taxRecordOperations = new TaxRecordOperations();
        }

        [TestMethod]
        public void EmptyTaxes_RandomDate_NullResult()
        {
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(new List<TaxRecord>(), DateTime.Now);
            Assert.IsNull(taxResult);
        }

        [TestMethod]
        public void SingleYearlyTax_ValidDate_FirstResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-06-01"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.yearlyTaxRecord, taxResult);
        }

        [TestMethod]
        public void SingleYearlyTax_NextYearDate_NullResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2025-06-01"));
            Assert.IsNull(taxResult);
        }

        [TestMethod]
        public void YearlyAndMonthlyTax_DateNotInMonth_YearlyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-06-01"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.yearlyTaxRecord, taxResult);

            taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.yearlyTaxRecord);
            taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-06-01"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.yearlyTaxRecord, taxResult);
        }

        [TestMethod]
        public void YearlyAndMonthlyTax_DateInMonth_MonthlyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-03-15"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.monthlyTaxRecord, taxResult);

            taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.yearlyTaxRecord);
            taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-03-15"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.monthlyTaxRecord, taxResult);
        }

        [TestMethod]
        public void YearlyAndMonthlyTax_DateNotInRangle_NullResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2025-06-01"));
            Assert.IsNull(taxResult);
        }

        [TestMethod]
        public void YearlyAndMonthlyAndWeeklyTax_DateInWeek_WeeklyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-07-23"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.weeklyTaxRecord, taxResult);

            taxes = new List<TaxRecord>();
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.yearlyTaxRecord);
            taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-07-23"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.weeklyTaxRecord, taxResult);

            taxes = new List<TaxRecord>();
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.yearlyTaxRecord);
            taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-07-23"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.weeklyTaxRecord, taxResult);
        }

        [TestMethod]
        public void YearlyAndMonthlyIncludingWeeklyTax_DateInIncludedWeek_WeeklyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.weeklyInMonthlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-03-12"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.weeklyInMonthlyTaxRecord, taxResult);
        }

        [TestMethod]
        public void YearlyAndMonthlyIncludingWeeklyTax_DateNotInIncludedWeek_WeeklyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.weeklyInMonthlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-07-23"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.weeklyTaxRecord, taxResult);
        }

        [TestMethod]
        public void DailyTax_DateOnly_DailyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.daily1);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-01-01"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.daily1, taxResult);
        }

        [TestMethod]
        public void AllTaxes_DateInWeekOnly_DailyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.weeklyInMonthlyTaxRecord);
            taxes.Add(this.dailyInWeeklyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-07-25"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.dailyInWeeklyTaxRecord, taxResult);
        }

        [TestMethod]
        public void AllTaxes_DateInWeekInMonth_DailyTaxResult()
        {
            var taxes = new List<TaxRecord>();
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.weeklyInMonthlyTaxRecord);
            taxes.Add(this.dailyInWeeklyInMonthlyTaxRecord);
            var taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-03-15"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.dailyInWeeklyInMonthlyTaxRecord, taxResult);

            taxes = new List<TaxRecord>();
            taxes.Add(this.dailyInWeeklyInMonthlyTaxRecord);
            taxes.Add(this.yearlyTaxRecord);
            taxes.Add(this.monthlyTaxRecord);
            taxes.Add(this.weeklyTaxRecord);
            taxes.Add(this.weeklyInMonthlyTaxRecord);
            taxResult = this.taxRecordOperations.CalculateTaxRecordForDate(taxes, DateTime.Parse("2024-03-15"));
            Assert.IsNotNull(taxResult);
            this.AssertTaxRecordsAreEqual(this.dailyInWeeklyInMonthlyTaxRecord, taxResult);
        }

        private void AssertTaxRecordsAreEqual(TaxRecord expected, TaxRecord actual)
        {
            Assert.AreSame(expected, actual);
        }

        private TaxRecord yearlyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.2m,
            Type = "Yearly",
            ValidFrom = DateTime.Parse("2024-01-01"),
            ValidTo = DateTime.Parse("2024-12-31")
        };

        private TaxRecord monthlyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.4m,
            Type = "Monthly",
            ValidFrom = DateTime.Parse("2024-03-01"),
            ValidTo = DateTime.Parse("2024-03-31")
        };

        private TaxRecord weeklyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.3m,
            Type = "Yearly",
            ValidFrom = DateTime.Parse("2024-07-22"),
            ValidTo = DateTime.Parse("2024-07-28")
        };

        private TaxRecord weeklyInMonthlyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.23m,
            Type = "Yearly",
            ValidFrom = DateTime.Parse("2024-03-11"),
            ValidTo = DateTime.Parse("2024-03-17")
        };

        private TaxRecord daily1 = new TaxRecord()
        {
            TaxValue = 0.1m,
            Type = "Yearly",
            ValidFrom = DateTime.Parse("2024-01-01"),
            ValidTo = DateTime.Parse("2024-01-01")
        };

        private TaxRecord dailyInWeeklyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.12m,
            Type = "Yearly",
            ValidFrom = DateTime.Parse("2024-07-25"),
            ValidTo = DateTime.Parse("2024-07-25")
        };

        private TaxRecord dailyInWeeklyInMonthlyTaxRecord = new TaxRecord()
        {
            TaxValue = 0.15m,
            Type = "Yearly",
            ValidFrom = DateTime.Parse("2024-03-15"),
            ValidTo = DateTime.Parse("2024-03-15")
        };

        private ITaxRecordOperations taxRecordOperations;
    }
}