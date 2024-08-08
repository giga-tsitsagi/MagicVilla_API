using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController:ControllerBase
    {
        private readonly AplicationDbContext _db;

        public VillaApiController(AplicationDbContext db)
        {
            _db = db;
        }

       

        [HttpGet] 
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Cities);
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        /* [ProducesResponseType(200)]
         [ProducesResponseType(404)]
         [ProducesResponseType(400)]*/
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaCreateDTO villaDTO)
        {
            /* I can control and display errors of the model if I'm not using [ApiController]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This villa is not unique");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
            };
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new {id = model.Id},model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || villaDTO.Id != id)
            {
                return BadRequest();
            }
           /* var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;*/

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO )
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            VillaUpdateDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Name = villa.Name,
                Details = villa.Details,
                Sqft = villa.Sqft,
                Rate = villa.Rate,
                Occupancy = villa.Occupancy,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
            };
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
