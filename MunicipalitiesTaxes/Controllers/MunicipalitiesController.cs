using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalitiesTaxes.Contracts;
using MunicipalitiesTaxes.Extensions;
using MunicipalitiesTaxes.Implementations;
using MunicipalitiesTaxes.Model;

namespace MunicipalitiesTaxes.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MunicipalitiesController : Controller
    {
        private readonly MunicipalitiesManager municipalitiesManager;

        public MunicipalitiesController(MunicipalitiesManager municipalitiesManager)
        {
            this.municipalitiesManager = municipalitiesManager;
        }

        [HttpGet]
        public IActionResult GetMunicipalities([FromQuery] string? municipalityName, [FromQuery] DateTime? targetDate)
        {
            if (string.IsNullOrEmpty(municipalityName))
            {
                return this.Ok(this.municipalitiesManager.GetAllMunicipalities().Select(m => m.ToMunicipalityDto()));
            }

            if (targetDate == null)
            {
                return this.BadRequest($"Please provide date to get {municipalityName} tax record");
            }

            var municipality = this.municipalitiesManager.GetTaxByMunicipalityNameAndDate(municipalityName, targetDate.Value);
            if (municipality == null)
            {
                return this.BadRequest($"Could not find tax record for municipality {municipalityName} at date {targetDate.Value.ToShortDateString()}");
            }

            return this.Ok(municipality.ToMunicipalityDto());
        }
        
        [HttpPost]
        public IActionResult CreateMunicipality([FromBody] MunicipalityCreateDto newMunicipality)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = this.municipalitiesManager.AddMunicipality(new Municipality { Name = newMunicipality.Name});

            return this.CreatedAtAction("CreateMunicipality", result.ToMunicipalityDto());
        }

        [HttpPost("{municipalityId:int}/taxrecords")]
        public IActionResult CreateTaxRecordForMunicipality([FromRoute] int municipalityId, [FromBody] TaxRecordCreateDto newTaxRecord)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var municipality = this.municipalitiesManager.AddTaxRecordForMunicipality(municipalityId, new TaxRecord() 
            { TaxValue = newTaxRecord.TaxValue, Type = newTaxRecord.Type, ValidFrom = DateTime.Parse(newTaxRecord.ValidFrom), ValidTo= DateTime.Parse(newTaxRecord.ValidTo) });

            return this.CreatedAtAction("CreateTaxRecordForMunicipality", municipality.ToMunicipalityDto());
        }

        [HttpPut("{municipalityId:int}/taxrecords/{taxRecordId:int}")]
        public IActionResult UpdateTaxRecordForMunicipality([FromRoute] int municipalityId, [FromRoute] int taxRecordId, [FromBody] TaxRecordDto updateTaxRecord)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var municipality = this.municipalitiesManager.UpdateTaxRecordForMunicipality(municipalityId, taxRecordId, new TaxRecord()
            { TaxValue = updateTaxRecord.TaxValue, Type = updateTaxRecord.Type, ValidFrom = DateTime.Parse(updateTaxRecord.ValidFrom), ValidTo = DateTime.Parse(updateTaxRecord.ValidTo) });

            return this.Ok(municipality.ToMunicipalityDto());

        }
    }
}
