using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace YavlenaPlus.Data.Models
{
    // Add profile data for application users by adding properties to the YavlenaPlusUser class
    public class YavlenaPlusUser : IdentityUser
    {
        public YavlenaPlusUser()
        {
            this.Offers = new HashSet<Offer>();
            this.Favorites = new HashSet<Favorite>();
            this.Comments = new HashSet<Comment>();
        }

        public ICollection<Offer> Offers { get; set; }
        public ICollection<Favorite> Favorites { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
