using System.ComponentModel.DataAnnotations;

namespace eBroker.DAL.Models
{
    public class EquityDB
    {
        [Key]
        public int EquityId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
