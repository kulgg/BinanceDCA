using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects;
using BinanceDCA.Models;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;

namespace BinanceDCA
{
    public class App
    {
        private readonly Config _config;
        private readonly ILogger<App> _log;

        public App(Config config, ILogger<App> log)
        {
            _config = config;
            _log = log;
        }

        public async Task Execute()
        {
            List<Buy> coinsToBuy = _config.Investing.Buys;
            
            var client = new BinanceClient(new BinanceClientOptions(){
                ApiCredentials = new ApiCredentials(_config.Binance.APIKey, _config.Binance.APISecret)
            });
            
            _log.LogInformation("Requesting Market Prices");
            
            var callResult = await client.Spot.Market.GetPricesAsync();
            
            if(!callResult.Success)
            {
                _log.LogError(callResult.Error?.Message);
            }
            else
            {
                foreach (var coin in coinsToBuy)
                {
                    var symbol = $"{coin.BuyTicker}{coin.SellTicker}";
                    var coinData = callResult.Data.FirstOrDefault(x => x.Symbol == symbol);
                    
                    if (coinData != null)
                    {
                        _log.LogInformation("{0} price: {1}", symbol, coinData.Price);
                    
                        var placeOrderResult = await client.Spot.Order.PlaceOrderAsync(symbol, OrderSide.Buy, OrderType.Market, quoteOrderQuantity: coin.AmountToSell);
                        
                        if (!placeOrderResult.Success)
                        {
                            _log.LogError(placeOrderResult.Error?.Message);
                        }
                        else
                        {
                            _log.LogInformation("{0}: Bought {1} {2} at {3} {4} average", placeOrderResult.Data.Status, placeOrderResult.Data.QuantityFilled, coin.BuyTicker, placeOrderResult.Data.AverageFillPrice, coin.SellTicker);
                        }
                    }
                }
            }
        }
    }
}