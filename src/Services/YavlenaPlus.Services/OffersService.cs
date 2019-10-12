using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using YavlenaPlus.Data.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using AngleSharp;
using System.Net;
using YavlenaPlus.Services.Contracts;
using System.Text.RegularExpressions;
using YavlenaPlus.Data;
using System.Timers;
using AngleSharp.Io;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;

namespace YavlenaPlus.Services
{
    public class OffersService : IOffersService
    {
        private YavlenaPlusContext _context;
        // private UserManager<YavlenaPlusUser> _userManager;
        //private RoleManager<IdentityRole> _roleManager;

        public OffersService()
            : base()
        {
        }

        public OffersService(YavlenaPlusContext context)//, UserManager<YavlenaPlusUser> userManager) //RoleManager<IdentityRole> roleManager
            : this()
        {
            this._context = context;
        }
        public IConfiguration config => Configuration.Default.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
        public IBrowsingContext BroContext => BrowsingContext.New(config);

        public string FirstPageAllSofiaOffersOrderByNew => "https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4we3r7&f1=1";
        public IDocument Document => BroContext.OpenAsync(FirstPageAllSofiaOffersOrderByNew).GetAwaiter().GetResult();

        public IEnumerable<Offer> GetAllOffers()
        {
            return _context.Offers.ToListAsync().GetAwaiter().GetResult();
        }
        public async Task<List<Offer>> GetAllOffersAsync() => await _context.Offers.OrderByDescending(x => x.Id).ToListAsync();

        public async Task<Offer> GetOfferByIdAsync(int id) => await _context.Offers.FirstOrDefaultAsync(x => x.Id == id);

        public int GetPercentOfInternalRateOfReturn(Offer offer)
        {
            //TODO: 
            return 5;
        }

        public async Task Seed()
        {
            RegexOptions options = RegexOptions.Multiline;
            Console.OutputEncoding = Encoding.UTF8;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var parser = BroContext.GetService<IHtmlParser>();

            List<string> textContentList = new List<string>();


            ///while (true) ??????
            while (true)
            {
                try
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        await Task.Delay(5000);
                        AngleSharp.Dom.IDocument document;

                        if (i == 1)
                        {
                            document = this.Document;
                        }
                        else
                        {
                            var address = $"https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4we3r7&f1={i}";
                            document = await GetDocument(BroContext, address); //BIG DELAY
                        }

                        for (int j = 11; j <= 50; j++)
                        {
                            var selector = $"body > div:nth-child(2) > table:nth-child(4) > tbody > tr:nth-child(1) > td:nth-child(1) > table:nth-child({j}) > tbody";
                            var item = document.QuerySelectorAll(selector)[0];

                            var price = -1;
                            try
                            {
                                price = int.Parse(Regex.Match(item.TextContent, @"(( ?[0-9 ]{1,}) (EUR|ЛЕВ|лв[.]|ЛВ|eur))", options).Value.Replace("EUR", "").ToString().Replace(" ", ""));
                            }
                            catch (Exception e)
                            {
                                price = 0;
                                textContentList.Add(e.ToString());
                            }
                            var type = Regex.Match(item.TextContent, @"Продава (\d-СТАЕН)|(Продава КЪЩА)|(Продава МНОГОСТАЕН)|(Продава МЕЗОНЕТ)|(Продава АТЕЛИЕ, ТАВАН)|(Продава ОФИС)|(Продава ОФИС)|(Продава ПАРЦЕЛ)|(Продава ГАРАЖ)|(Продава МАГАЗИН)|(Продава ЕТАЖ ОТ КЪЩА)", options).Value.Replace("Продава ", "");
                            var location = Regex.Match(item.TextContent, @"град ([А-Я][а-я]+), (.){3,}", options).Value.Replace("град ", "");
                            var phoneNumber = Regex.Match(item.TextContent, @"тел.: ([\d \/-]+)", options).Value.Replace("тел.: ", "").Replace(" ", "").Replace("-", "").Replace("/", "").Replace("\\", "");
                            var size = int.Parse(Regex.Match(item.TextContent, @"  \d{1,7} кв.м", options).Value.Replace("  ", "").Replace(" кв.м", ""));

                            var test = await this.IsAnyOffer(_context, new Offer(price, type, location, size, phoneNumber));
                            if (!test)
                            {
                                var picAndDescriptionSelector = $"body > div:nth-child(2) > table:nth-child(4) > tbody > tr:nth-child(1) > td:nth-child(1) > table:nth-child({j}) > tbody > tr:nth-child(2) > td:nth-child(1) > table > tbody > tr > td > a";
                                var kaboom = document.QuerySelectorAll(picAndDescriptionSelector);
                                var picAndFullDescr = kaboom[0];

                                var picUrl = picAndFullDescr.InnerHtml.Split("\"")[1];
                                var linkToOffer = kaboom.OfType<IHtmlAnchorElement>().Select(x => x.Href).ToList()[0];


                                await Task.Delay(5000);
                                string fullDescription = "";
                                try
                                {
                                    fullDescription = BroContext.OpenAsync(linkToOffer).Result.GetElementById("description_div").TextContent;///////----
                                }
                                catch (Exception)
                                {
                                    fullDescription = "";

                                }
                                _context.Offers.AddAsync(new Offer(price, type, location, size, phoneNumber, picUrl, fullDescription, linkToOffer)).GetAwaiter().GetResult();
                                _context.SaveChangesAsync().GetAwaiter().GetResult();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    textContentList.Add(e.ToString());

                    Console.WriteLine("OffersService issue! Call 0893748997 or in the Police.");
                    Console.WriteLine(e.ToString());
                }
            }

        }

        public int GetCountOfCommentsForOffer(int offerId)
        {
            var count = _context.Comments.Count(x => x.OfferId == offerId);
            return count;
        }

        private static async Task<AngleSharp.Dom.IDocument> GetDocument(IBrowsingContext context, string address)
        {
            return await context.OpenAsync(address);
        }

        private Task<bool> IsAnyOffer(YavlenaPlusContext ctx, Offer offer)
        {
            var resultt = ctx.Offers.AnyAsync(x => x.Type == offer.Type && x.PhoneOfOwner == offer.PhoneOfOwner && x.Size == offer.Size && x.Location == offer.Location && x.Price == offer.Price);
            return resultt;
        }
    }
}