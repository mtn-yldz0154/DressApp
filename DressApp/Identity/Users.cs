using Microsoft.AspNetCore.Identity;

namespace DressApp.WebUi.Identity
{
    public class User:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
