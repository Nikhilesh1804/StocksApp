using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Implemented repository pattern. 
    /// </summary>
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        Task<List<BuyOrder>> GetAllBuyOrders();
        Task<List<SellOrder>> GetAllSellOrders();
    }
}