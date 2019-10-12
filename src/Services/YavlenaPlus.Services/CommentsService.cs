using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YavlenaPlus.Data;
using YavlenaPlus.Data.Models;
using System.Security.Claims;

namespace YavlenaPlus.Services
{
    public class CommentsService
    {
        private YavlenaPlusContext _context;
        private UserManager<YavlenaPlusUser> _userManager;

        public CommentsService(YavlenaPlusContext ctx, UserManager<YavlenaPlusUser> userManager)
        {
            this._context = ctx;
            this._userManager = userManager;
        }
    }
}
