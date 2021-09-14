using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Enums;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BinanceDCA
{
    public class Binance : IJob
    {
        private readonly BinanceClient _client;
        private readonly ILogger<Binance> _log;

        public Binance(BinanceClient client, ILogger<Binance> log)
        {
            _client = client;
            _log = log;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            string buyTicker = dataMap.GetString("BuyTicker");
            string sellTicker = dataMap.GetString("SellTicker");
            var amountToSell = (decimal) dataMap["AmountToSell"];
            
            _log.LogInformation("Executing {0} DCA", buyTicker);

            var callResult = await _client.Spot.Market.GetPricesAsync();
            
            if(!callResult.Success)
            {
                _log.LogError(callResult.Error?.Message);
            }
            else
            {
                var symbol = $"{buyTicker}{sellTicker}";
                var coinData = callResult.Data.FirstOrDefault(x => x.Symbol == symbol);
                
                if (coinData != null)
                {
                    _log.LogInformation("{0} price: {1}", symbol, coinData.Price);
                    
                    var placeOrderResult = await _client.Spot.Order.PlaceOrderAsync(symbol, OrderSide.Buy, OrderType.Market, quoteOrderQuantity: amountToSell);
                    
                    if (!placeOrderResult.Success)
                    {
                        _log.LogError(placeOrderResult.Error?.Message);
                    }
                    else
                    {
                        _log.LogInformation("{0}: Bought {1} {2} (@ {3} {4} average)", placeOrderResult.Data.Status, placeOrderResult.Data.QuantityFilled, buyTicker, placeOrderResult.Data.AverageFillPrice, sellTicker);
                    }
                }
                else
                {
                    _log.LogInformation("Bad Ticker: {0}", symbol);
                }
            }
        }
    }
}