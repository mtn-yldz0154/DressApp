using System.Collections.Generic;

namespace DressApp.WebUi.Data.Models
{
    public class Like
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<LikeItem> LikeItems { get; set; }

    }
}
