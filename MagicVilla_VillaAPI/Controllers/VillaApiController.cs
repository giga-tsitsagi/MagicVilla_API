using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController:ControllerBase
    {
        private readonly AplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaApiController(AplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
       

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        /* [ProducesResponseType(200)]
         [ProducesResponseType(404)]
         [ProducesResponseType(400)]*/
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa =await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO CreateDTO)
        {
            /* I can control and display errors of the model if I'm not using [ApiController]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == CreateDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This villa is not unique");
                return BadRequest(ModelState);
            }
            if (CreateDTO == null)
            {
                return BadRequest(CreateDTO);
            }
            Villa model = _mapper.Map<Villa>(CreateDTO);
            /*Villa model = new()
            {
                Amenity = CreateDTO.Amenity,
                Name = CreateDTO.Name,
                Details = CreateDTO.Details,
                Sqft = CreateDTO.Sqft,
                Rate = CreateDTO.Rate,
                Occupancy = CreateDTO.Occupancy,
                ImageUrl = CreateDTO.ImageUrl,
            };*/
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new {id = model.Id},model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO UpdateDTO)
        {
            if (UpdateDTO == null || UpdateDTO.Id != id)
            {
                return BadRequest();
            }
            /* var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
             villa.Name = villaDTO.Name;
             villa.Occupancy = villaDTO.Occupancy;
             villa.Sqft = villaDTO.Sqft;*/

            Villa model = _mapper.Map<Villa>(UpdateDTO);

            /*Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
            };*/
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO )
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa); 

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);
          
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
