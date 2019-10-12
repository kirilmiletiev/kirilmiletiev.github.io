using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using YavlenaPlus.Data.Models;

namespace YavlenaPlus.Services.Contracts
{
    public interface IOffersService
    {
        IBrowsingContext BroContext { get; }
        IConfiguration config { get; }
        IDocument Document { get; }
        string FirstPageAllSofiaOffersOrderByNew { get; }

        IEnumerable<Offer> GetAllOffers();
        Task<Offer> GetOfferByIdAsync(int id);
        Task<List<Offer>> GetAllOffersAsync();
        int GetCountOfCommentsForOffer(int offerId);
        Task Seed();
        int GetPercentOfInternalRateOfReturn(Offer offer);
    }
}