using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogMyWork.Models
{
    public class Rate
    {
        [Key]
        public int RateID { get; set; }
        public double RateValue { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}