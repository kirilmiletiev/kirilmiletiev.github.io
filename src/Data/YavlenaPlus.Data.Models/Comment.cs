using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YavlenaPlus.Data.Models.Common;

namespace YavlenaPlus.Data.Models
{
    public class Comment: BaseModel<int>
    {
        public Comment()
        {
            this.CommentTime = DateTime.Now;
        }

        [Required]
        public string YavlenaPlusUserId { get; set; }
        public YavlenaPlusUser YavlenaPlusUser { get; set; }

        [Required]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        public DateTime CommentTime { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Content { get; set; }
    }
}
