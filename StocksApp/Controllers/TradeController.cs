using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        public TradeController(IFinnhubService finnhubService, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _stocksService = stocksService;
        }


        [Route("/")]
        [Route("/Trade/Index")]
        public IActionResult Index()
        {
            ViewBag.FinnhubToken = "cc676uaad3i9rj8tb1s0";
            Dictionary<string, object>? companyProfileDictionary = new Dictionary<string, object>();
            var resultCompanyProfile = _finnhubService.GetCompanyProfile("AAPL");

            companyProfileDictionary = resultCompanyProfile.Result;


            Dictionary<string, object>? stockQuoteDictionary = new Dictionary<string, object>();
            var resultStockQuote = _finnhubService.GetStockPriceQuote("AAPL");

            stockQuoteDictionary = resultStockQuote.Result;


            StockTrade stockTrade = new StockTrade() { StockSymbol = "AAPL" };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]), StockName = Convert.ToString(companyProfileDictionary["name"]), Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = "cc676uaad3i9rj8tb1s0";




            return View(stockTrade);
        }

        [Route("/Trade/Orders")]
        public   IActionResult Orders()
        {
            List<BuyOrderResponse> buyOrderResponses =  _stocksService.GetBuyOrders().Result;
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders().Result;
            Orders orders = new Orders();
            orders.BuyOrders = buyOrderResponses;
            orders.SellOrders = sellOrderResponses;
            return View(orders);
        }

        [Route("/Trade/BuyOrder")]
        [HttpPost]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest).Result;
            return RedirectToAction(nameof(Orders));
        }


        [Route("/Trade/SellOrder")]
        [HttpPost]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
        {
            
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            
            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest).Result;

            return RedirectToAction(nameof(Orders));
        }


        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            //Get list of orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            //orders.AddRange(await _stocksService.GetBuyOrders());
            //orders.AddRange(await _stocksService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            

            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }


    }
}
