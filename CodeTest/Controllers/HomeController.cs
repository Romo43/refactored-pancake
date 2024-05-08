using System;
using System.Linq;
using System.Web.Mvc;
using Quote.Contracts;
using Quote.Models;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using PruebaIngreso.Models;

namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;

        public HomeController(IQuoteEngine quote)
        {
            this.quote = quote;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }

        public ActionResult Test3()
        {
            var serviceCodes = new List<string> { "E-U10-UNILATIN", "E-U10-DSCVCOVE", "E-E10-PF2SHOW" };

            var apiResponses = new List<ApiResponse>();

            // Enable TLS 1.1 and TLS 1.2 security protocols
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            foreach (var code in serviceCodes)
            {
                // API URL with the service code
                string apiUrl = $"https://refactored-pancake.free.beeceptor.com/margin/{code}";

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string response = client.DownloadString(apiUrl);
                        dynamic responseData = JsonConvert.DeserializeObject(response);
                        double margin = responseData.margin;

                        apiResponses.Add(new ApiResponse { Code = code, Margin = margin });
                    }
                }
                catch (WebException ex)
                {
                    // If an exception occurs, assign 0.0 to the margin
                    // and add the response to the list
                    apiResponses.Add(new ApiResponse { Code = code, Margin = 0.0 });
                }
            }

            return View(apiResponses);
        }

        public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            return View(result.TourQuotes);
        }
    }
}