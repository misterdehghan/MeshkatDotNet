using System.ComponentModel.DataAnnotations;

namespace Azmoon.Domain.Entities.PublicRelations.Location
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}
