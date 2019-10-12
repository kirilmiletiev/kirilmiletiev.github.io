using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YavlenaPlus.Data.Models.Common;

namespace YavlenaPlus.Data.Models
{
    public class Favorite : BaseModel<int>
    {
        public Favorite()
        {
            this.IsActive = true;
        }

        [Required]
        public string YavlenaPlusUserId { get; set; }

        public YavlenaPlusUser YavlenaPlusUser { get; set; }

        [Required]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        public bool IsActive { get; set; }
    }
}
