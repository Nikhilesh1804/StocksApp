using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockMarketDbContext _db;

        public StocksRepository(StockMarketDbContext db)
        {
            _db = db;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            //add sell order object to sell orders list
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetAllBuyOrders()
        {
            List<BuyOrder> buyOrders = await _db.BuyOrders
    .OrderByDescending(temp => temp.DateAndTimeOfOrder)
    .ToListAsync();

            return buyOrders;
        }

        public async Task<List<SellOrder>> GetAllSellOrders()
        {
            List<SellOrder> sellOrders = await _db.SellOrders
    .OrderByDescending(temp => temp.DateAndTimeOfOrder)
    .ToListAsync();

            return sellOrders;
        }
    }
}