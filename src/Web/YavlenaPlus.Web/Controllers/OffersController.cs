using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using YavlenaPlus.Data.Models;
using YavlenaPlus.Services.Contracts;
using YavlenaPlus.Data;
using Microsoft.AspNetCore.Authorization;
using YavlenaPlus.Web.Models;
using YavlenaPlus.Web.Controllers.Contracts;

namespace YavlenaPlus.Web.Controllers
{
    public class OffersController : Controller
    {
        private UserManager<YavlenaPlusUser> _userManager;
        private YavlenaPlusContext _context;
        private YavlenaPlusUser _currentUser => _context.Users.Find(_userManager.GetUserAsync(this.User).GetAwaiter().GetResult().Id);
        private IOffersService _offererService;
        //  private RoleManager<IdentityRole> _roleManager;
        // private OffersService _offersService;

        public OffersController(UserManager<YavlenaPlusUser> userManager, YavlenaPlusContext context, IOffersService offersService)//, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._context = context;
            this._offererService = offersService;
            // this._roleManager = roleManager;
            //this._offersService = offersService;
            //this.CurrentUser = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult();
        }
        //public YavlenaPlusUser CurrentUser { get; set; }

        // GET: Offers
        public ActionResult Index(int id)
        {
            var dict = FillDictionary();

            if (id <= 0)
            {
                this.ViewData.Add("currentPage", 1);
            }
            else if (id > 0)
            {
                this.ViewData.Add("currentPage", id);
            }


            if (dict.Keys.Count() <= 0)
            {
                this.ViewData.Add("totalPages", 1);
            }
            else if (dict.Keys.Count() > 0)
            {
                this.ViewData.Add("totalPages", dict.Keys.Count());
            }

            //return View(allOffers);
            if (id == 0)
            {
                return View(dict[1]);
            }
            else if ((id) > 0)
            {
                return View(dict[id]);
            }

            else
            {
                return View(dict[1]);
            }

        }

        private Dictionary<int, List<Offer>> FillDictionary()
        {
            var allOffers = this._offererService.GetAllOffersAsync().GetAwaiter().GetResult();

            Dictionary<int, List<Offer>> dict = new Dictionary<int, List<Offer>>();
            // var pages = (int)Math.Ceiling((decimal)offers.Count() % 10);
            int keyCounter = 1;
            int offersCounter = 0;
            foreach (var offer in allOffers)
            {
                if (!dict.Keys.Contains(keyCounter))
                {
                    dict.Add(keyCounter, new List<Offer>());
                }
                dict[keyCounter].Add(offer);
                offersCounter++;
                if (offersCounter == 10)
                {
                    keyCounter++;
                    offersCounter = 0;
                }
            }
            return dict;
        }

        // GET: Offers/Details/5
        public ActionResult Details(int id)
        {
            var off = _context.Offers.FirstOrDefault(x => x.Id == id);
            //  this.ViewBag.User = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult();
            // this.ViewData.Add(_userManager.GetUserAsync(this.User).GetAwaiter().GetResult().Id);
            return View(_context.Offers.FirstOrDefault(x => x.Id == id));
        }

        public ActionResult OfferDetails(int id)
        {
            return View();
        }

        // GET: Offers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Offers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Offers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Offers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Offers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [HttpGet]
        [Authorize]
        [Route("/Offers/MyFavoriteOffers")]
        public async Task<ActionResult> MyFavoriteOffers()
        {
            //  var a = 
            // await a.ToAsyncEnumerable().ToList().GetAwaiter().GetResult();
            // var a = await GetFav();
            return View(await GetFav());
        }

        [Authorize]
        private async Task<List<Favorite>> GetFav()
        {
            // var a = _context.Favorites.Where(x => x.YavlenaPlusUserId == this._userManager.GetUserId(this.User));
            return await _context.Favorites.ToAsyncEnumerable<Favorite>().Where(x => x.YavlenaPlusUserId == _currentUser.Id).ToList();// await b;
        }

        //[HttpGet]
        //[Authorize]
        //public ActionResult MyFavoriteOffers()
        //{
        //    return View(_context.Favorites.Where(x => x.YavlenaPlusUserId == this._userManager.GetUserAsync(this.User).Result.Id));
        //}


        [Authorize]
        public async Task<ActionResult> AddToMyFavoriteOffers(int id)
        {
            //var offer = _offererService.GetOfferByIdAsync(id);
            Favorite fav = new Favorite()
            {
                OfferId = (await _offererService.GetOfferByIdAsync(id)).Id,
                YavlenaPlusUserId = _currentUser.Id
            };

            await _context.Favorites.AddAsync(fav);
            await _context.SaveChangesAsync();
            return Redirect("/Offers/MyFavoriteOffers");
            ///Favorite
            //await _context.Favorites.AddAsync(new Favorite()
            //{
            //    Offer = _context.Offers.FirstOrDefault(x => x.Id == id),
            //    OfferId = _context.Offers.FirstOrDefault(x => x.Id == id).Id,
            //    YavlenaPlusUserId = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult().Id,
            //    YavlenaPlusUser = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult()
            //});
            // _context.SaveChangesAsync().GetAwaiter().GetResult();
            //return Redirect("/Offers/MyFavoriteOffers");
            //return  MyFavoriteOffers();
            // return RedirectToAction(nameof(MyFavoriteOffers));
        }

        [Authorize]
        public ActionResult Comment(int id)
        {
            _context.Favorites.Add(new Favorite()
            {
                Offer = _context.Offers.FirstOrDefault(x => x.Id == id),
                OfferId = _context.Offers.FirstOrDefault(x => x.Id == id).Id,
                YavlenaPlusUserId = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult().Id,
                YavlenaPlusUser = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult()
            });
            _context.SaveChangesAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(MyFavoriteOffers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return (this.Details(comment.OfferId));
            //return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> CreateComment(string content, int offerId)
        {
            Comment comment = new Comment() { YavlenaPlusUserId = this._currentUser.Id, OfferId = offerId, Content = content };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Redirect($"/offers/details/{offerId}");// View("Details", _context.Offers.FirstOrDefault(x => x.Id == offerId));
            //return RedirectToAction(nameof(MyFavoriteOffers));
            //return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PrepareComment(string content, int offerId)
        {
            Comment comment = new Comment() { YavlenaPlusUserId = this._currentUser.Id, OfferId = offerId, Content = content };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return (this.Details(comment.OfferId));
            //return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }
    }
}