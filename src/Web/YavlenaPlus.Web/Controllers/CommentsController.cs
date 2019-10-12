using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YavlenaPlus.Data;
using YavlenaPlus.Data.Models;
using YavlenaPlus.Web.Controllers.Contracts;
using YavlenaPlus.Web.Models;

namespace YavlenaPlus.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly YavlenaPlusContext _context;
        private UserManager<YavlenaPlusUser> _userManager;
      //  private IOffersController _offersController;
        private YavlenaPlusUser _currentUser => _context.Users.Find(_userManager.GetUserAsync(this.User).GetAwaiter().GetResult().Id);

        public CommentsController(YavlenaPlusContext context,  UserManager<YavlenaPlusUser> userManager) //, IOffersController offersController)
        {
            _context = context;
            this._userManager = userManager;
            //_offersController = offersController;
        }

        public YavlenaPlusUser CurrentUser => _context.Users.FirstOrDefault(x=>x.Id == _userManager.GetUserAsync(this.User).GetAwaiter().GetResult().Id);
        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComment()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
            return View($"Offers/details/{comment.OfferId}");
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        
        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult<Comment>> CreateComment(string content, int offerId)
        //{
        //    Comment comment = new Comment() { YavlenaPlusUserId = this._currentUser.Id, OfferId = offerId, Content = content };
        //    _context.Comments.Add(comment);
        //    await _context.SaveChangesAsync();

        //    return (_offersController.Details(offerId));
        //    //return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        //}

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
