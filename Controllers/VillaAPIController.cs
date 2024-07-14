using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")] // or use [Route("api/[controller]")] but if u change the controller name you have to notify the person who is consuming your api end-point the new route

    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.VillaList);
        }

        
        [HttpGet("{id:int}", Name = "GetVilla")] // or [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id <= 0)
                return BadRequest();
            var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla(VillaDTO villa) 
        {
            //We can add ModelState for custome validations or if  [ApiController] is not used
            if(VillaStore.VillaList.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("DuplicateName", "Name Exists");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest();
            }
            if (villa.Id != 0)
                return StatusCode(StatusCodes.Status500InternalServerError);
            villa.Id = VillaStore.VillaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }
        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();
            var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            if(villa == null)
                return NotFound();
            VillaStore.VillaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villa)
        {
            if(id == 0 || id != villa.Id || villa == null)
            {
                return BadRequest();
            }
            var villaObj = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            if(villaObj == null)
                return NotFound();
            villaObj.Name = villa.Name;
            villaObj.SqrFeet = villa.SqrFeet;
            villaObj.Description = villa.Description;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            patchDTO.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
