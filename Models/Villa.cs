namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SqrFeet { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
