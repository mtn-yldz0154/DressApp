using System.ComponentModel.DataAnnotations;

namespace DressApp.WebUi.Data.Models
{
	public class Admin
	{
		[Key]
		public int AdminId { get; set; }

		[StringLength(20, ErrorMessage = "Please Enter 5-20 Character"), MinLength(5)]
		public string AdminUserName { get; set; }

		[StringLength(20, ErrorMessage = "Please Enter 5-20 Character"), MinLength(5)]
		public string AdminPassword { get; set; }

		[StringLength(1, ErrorMessage = "Please Enter 5-20 Character")]
		public string AdminRole { get; set; }

    
    }
}
