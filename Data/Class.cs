using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> VillaList = new List<VillaDTO>
        {
            new VillaDTO {Id =1, Name = "Villa 1",SqrFeet = 100, Description = "Excellent" },
            
        };
    }
}
