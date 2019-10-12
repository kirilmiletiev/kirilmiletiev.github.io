using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YavlenaPlus.Web.Models
{
    public class CreateCommentViewModel
    {
        public int OfferId { get; set; }

        public string Content { get; set; }
    }
}
