using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YavlenaPlus.Data.Models;

namespace YavlenaPlus.Web.Controllers.Contracts
{
    public interface IOffersController
    {
        Task<ActionResult> AddToMyFavoriteOffers(int id);
        ActionResult Comment(int id);
        ActionResult Create();
        ActionResult Create(IFormCollection collection);
        Task<ActionResult<Comment>> CreateComment(string content, int offerId);
        ActionResult Delete(int id);
        ActionResult Delete(int id, IFormCollection collection);
        ActionResult Details(int id);
        ActionResult Edit(int id);
        ActionResult Edit(int id, IFormCollection collection);
        ActionResult Index(int id);
        Task<ActionResult> MyFavoriteOffers();
        ActionResult OfferDetails(int id);
        Task<ActionResult<Comment>> PostComment(Comment comment);
        Task<ActionResult<Comment>> PrepareComment(string content, int offerId);
    }
}