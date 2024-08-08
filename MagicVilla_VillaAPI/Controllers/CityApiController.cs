using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.City_DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }


        [HttpDelete]
        public IActionResult DeleteCity(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            City city = _db.Cities.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            _db.Cities.Remove(city);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateCity")]
        public IActionResult PostCity(int id, [FromBody] CityUpdateDTO cityDTO)
        {
            if (id != cityDTO.Id || cityDTO == null)
            {
                return BadRequest();
            }
            City model = new()
            {
                Id = id,
                Name = cityDTO.Name,
                Population = cityDTO.Population,
                Area = cityDTO.Area,
            };
            _db.Cities.Update(model);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialCity")]
        public IActionResult UpdateCity(int id, JsonPatchDocument<CityUpdateDTO> patchDTO)
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var city = _db.Cities.FirstOrDefault(u => u.Id == id);
            CityUpdateDTO cityDTO = new()
            {
                Id = city.Id,
                Name = city.Name,
                Population = city.Population,
                Area = city.Area,
            };
            if (city == null)// ar jobs rom if 105 xaze iyos?
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(cityDTO, ModelState);
            City model = new City()
            {
                Id = city.Id,
                Name = city.Name,
                Population = city.Population,
                Area = city.Area,
            };
            _db.Cities.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
