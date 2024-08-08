using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.City_DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/Cityapi")]
    [ApiController]
    public class CityApiController : ControllerBase
    {
        private readonly AplicationDbContext _db;

        public CityApiController(AplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<City>> GetCities()
        {
            return _db.Cities;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<City>> GetCity(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var city = _db.Cities.FirstOrDefault(u => u.Id == id);
            if (city == null)
            {
                
                    return NotFound();
                
            }
            return Ok(city);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CityDTO> CreateCity([FromBody] CityCreateDTO cityDTO)
        {
            if (_db.Cities.FirstOrDefault(u => u.Name.ToLower() == cityDTO.Name.ToLower()) != null)
            {

                /* ModelState.AddModelError("CustomError", "This villa is not unique");
                 return BadRequest(ModelState);*/
            }
            if (cityDTO == null)
            {
                return null;
            }
            City model = new City()
            {
                Name = cityDTO.Name,
                Population = cityDTO.Population,
                Area = cityDTO.Area,
            };
            _db.Cities.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }
    }
}
