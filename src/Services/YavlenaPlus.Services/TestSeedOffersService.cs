using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace YavlenaPlus.Services
{
    class TestSeedOffersService
    {
        /// Install "AngleSharp" and "System.Text.Encoding.CodePages" through NuGet
        /// 
        //Console.OutputEncoding = Encoding.UTF8;

        //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            
        public object Page1 { get => GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=1"); }
        public object Page2 { get => GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=2"); }
        public object Page3 { get => GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=3"); }
        public object Page4 { get => GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=4"); }

        //var page1 = GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=1");
        //var page2 = GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=2");
        //var page3 = GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=3");
        //var page4 = GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=4");
        //var page5 = GetData("https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6zbk&f1=5");

        List<string> GetData(string address)
        {
            var config = Configuration.Default.WithDefaultLoader();
            //var address = "https://www.imot.bg/pcgi/imot.cgi?act=3&slink=4m6v0x&f1=1";
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync(address).GetAwaiter().GetResult();
            var cellSelector = "tr.vevent td:nth-child(3)";
            var cells = document.QuerySelectorAll(cellSelector);
            var price = document.QuerySelector(".price").TextContent;
            var titles = cells.Select(m => m.TextContent);
            return GetRawListOfOffers(document);
        }

        List<string> GetRawListOfOffers(IDocument document)
        {

            // var prices = document.GetElementsByClassName("price");
            var prices = document.GetElementsByClassName("price").Select(x => x.TextContent).ToList();


            ///////regex try one

            //list with raw info about offers 
            List<string> rawOfferList = new List<string>();

            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;

            //this regex and foreach add to raw list size in (kB.M.), very short description and finally phone number of owner.
            var sizeShortDescriptionPhonePattern = @"(\d{2,5} кв.м,)(.*)(\d{10})";
            var counter = 0;
            foreach (Match m in Regex.Matches(document.DocumentElement.InnerHtml, sizeShortDescriptionPhonePattern, options))
            {
                Console.WriteLine($"NumberOfOffer {counter}");
                Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
                Console.WriteLine("---------------------------------------------------------------------------------------");
                rawOfferList.Add($"{prices[counter]}***{m.Value}");
                counter++;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //this regex and foreach add to raw list type of property (2-bedroom or 3-bedroom), and location in format {city}, {neighborhood};
            string locationType = @"""(Обява продава).*""";
            counter = 0;
            foreach (Match m in Regex.Matches(document.DocumentElement.InnerHtml, locationType, options))
            {
                Console.WriteLine($"NumberOfOffer {counter}");
                Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------------------------------------------------");
                rawOfferList[counter] += $" *** {m.Value}";
                counter++;
                //rawOfferList[counter] += $" *** {m.Value}";
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //this regex and foreach add to raw list link to one small img (main img of the offer). It will be helpful for list representation of many offers.
            var regexForSrc = @"(<img src=""\/\/imot.focus.bg\/photosimotbg\/\d{1}\/\d{2,5}\S*)()";
            counter = 0;
            foreach (Match m in Regex.Matches(document.DocumentElement.InnerHtml, regexForSrc, options))
            {
                Console.WriteLine($"NumberOfOffer {counter}");
                Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------------------------------------------------");
                rawOfferList[counter] += $" *** {m.Value}";
                counter++;
                //rawOfferList[counter] += $" *** {m.Value}";
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //this regex and foreach add to raw list actually link to full info of the offer (full description and all of the pictures);
            var regexForLink = @"(<a href="")(\/\/www.imot.bg\S*) (class=""lnk1"">)";
            counter = 0;
            foreach (Match m in Regex.Matches(document.DocumentElement.InnerHtml, regexForLink, options))
            {
                Console.WriteLine($"NumberOfOffer {counter}");
                Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------------------------------------------------");
                rawOfferList[counter] += $" *** {m.Value}";
                counter++;
                //rawOfferList[counter] += $" *** {m.Value}";
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            return rawOfferList;

        }
    }
}

