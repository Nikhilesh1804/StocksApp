using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService : IStocksService
    {
        
        
        private readonly IStocksRepository _db;

        public StocksService(IStocksRepository dbRep)
        {
            
            
            _db = dbRep;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            //_db.BuyOrders.Add(buyOrder);
            //await _db.SaveChangesAsync();
            await _db.CreateBuyOrder(buyOrder);
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model validation
            //ValidationHelper.ModelValidation(sellOrderRequest);

           
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            
            sellOrder.SellOrderID = Guid.NewGuid();

            
            await _db.CreateSellOrder(sellOrder);
           

            //convert the SellOrder object into SellOrderResponse type
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return _db.GetAllBuyOrders().Result.Select(b => b.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            
            return _db.GetAllSellOrders().Result.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
