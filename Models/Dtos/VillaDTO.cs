using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dtos
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        public double SqrFeet { get; set; }
        public string Description { get; set; }
    }
}
