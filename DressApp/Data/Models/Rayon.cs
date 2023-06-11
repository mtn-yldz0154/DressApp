using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DressApp.WebUi.Data.Models
{
    public class Rayon
    {
        public int RayonId { get; set; }

        [Required(ErrorMessage ="Rayon Name Not Empty")]
        [StringLength(20,ErrorMessage ="Please only enter 5-20 length charecter",MinimumLength =5)]
        public string RayonName { get; set; }
        public string RayonDescription { get; set; }

        public List<Product> Products { get; set; }
    }
}
