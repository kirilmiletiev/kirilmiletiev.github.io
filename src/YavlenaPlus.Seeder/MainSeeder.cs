using AngleSharp;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YavlenaPlus.Data;
using YavlenaPlus.Data.Models;

namespace YavlenaPlus.Seeder
{
    public class MainSeeder
    {
        private readonly YavlenaPlusContext _context;

        public MainSeeder(YavlenaPlusContext yavlenaPlusContext)
        {
            this._context = yavlenaPlusContext;
            Seed();
        }

        private async void Seed()
        {
            RegexOptions options = RegexOptions.Multiline;
            var config = Configuration.Default.WithDefaultLoader();
            Console.OutputEncoding = Encoding.UTF8;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var context = BrowsingContext.New(config);

            List<string> textContentList = new List<string>();

            //while (true)
            //{                               ----------------------------------------
            //                 <<<<--------- All  .Seed() {{{[[({})]]}{[({})]]}{[({})]]}}} code  here Async -------->>>>
            //}    
            //----------------------------------------

            while (this._context.Offers.Any())
            {
                var span = TimeSpan.FromSeconds(5);

                try
                {
                    for (int i = 1; i <= 1; i++)
                    {
                        // System.Threading.Thread.Sleep(5000);///must be  much more, but for the testing purpuse only! ASYNC! There must be 2-3 of them. + while(true)
                        var address = $"https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4ron5i&f1=1";
                        var document = context.OpenAsync(address).GetAwaiter().GetResult();
                        for (int j = 11; j < 50; j++)
                        {
                            //  System.Threading.Thread.Sleep(10000); ///must be  much more, but for the testing purpuse only! ASYNC! There must be 2-3 of them. while(true)
                            var selector = $"body > div:nth-child(2) > table:nth-child(4) > tbody > tr:nth-child(1) > td:nth-child(1) > table:nth-child({j}) > tbody";
                            var picAndDescriptionSelector = $"body > div:nth-child(2) > table:nth-child(4) > tbody > tr:nth-child(1) > td:nth-child(1) > table:nth-child({j}) > tbody > tr:nth-child(2) > td:nth-child(1) > table > tbody > tr > td > a";
                            var item = document.QuerySelectorAll(selector)[0];
                            var picAndFullDescr = document.QuerySelectorAll(picAndDescriptionSelector)[0];

                            var price = int.Parse(Regex.Match(item.TextContent, @"(( ?[0-9 ]{1,}) (EUR|ЛЕВ))", options).Value.Replace("EUR", "").ToString().Replace(" ", ""));
                            var type = Regex.Match(item.TextContent, @"Продава (\d-СТАЕН)|(Продава КЪЩА)|(Продава МНОГОСТАЕН)|(Продава МЕЗОНЕТ)|(Продава АТЕЛИЕ, ТАВАН)|(Продава ОФИС)|(Продава ОФИС)|(Продава ПАРЦЕЛ)|(Продава ГАРАЖ)|(Продава МАГАЗИН)|(Продава ЕТАЖ ОТ КЪЩА)", options).Value.Replace("Продава ", "");
                            var location = Regex.Match(item.TextContent, @"град ([А-Я][а-я]+), (.){3,}", options).Value.Replace("град ", "");
                            var size = int.Parse(Regex.Match(item.TextContent, @"  \d{1,7} кв.м", options).Value.Replace("  ", "").Replace(" кв.м", ""));
                            var phoneNumber = Regex.Match(item.TextContent, @"тел.: ([\d \/-]+)", options).Value.Replace("тел.: ", "").Replace(" ", "").Replace("-", "").Replace("/", "").Replace("\\", "");
                            var picUrl = picAndFullDescr.InnerHtml.Split('\'')[1];
                            var linkToOffer = document.QuerySelectorAll(picAndDescriptionSelector).OfType<IHtmlAnchorElement>().Select(x => x.Href).ToList()[0];

                            //for full description
                            var fullDescription = context.OpenAsync(linkToOffer).GetAwaiter().GetResult().GetElementById("description_div").TextContent;
                            //AddOffer(price, type, location, size, phoneNumber, picUrl, fullDescription, linkToOffer);
                            if (!_context.Offers.Any(x => x.PhoneOfOwner == phoneNumber && x.Location == location && x.Price == price && x.Type == type))
                            {
                                _context.Offers.Add(new Offer(price, type, location, size, phoneNumber, picUrl, fullDescription, linkToOffer));
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("OffersService issue! Call 0893748997 or in the Police.");
                    Console.WriteLine(e);
                }
            }
            await Task.Run(() => Seed());
        }

    }
}
