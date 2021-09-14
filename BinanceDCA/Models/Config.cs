using System.Collections.Generic;

namespace BinanceDCA.Models
{
    public class Config
    {
        public Binance Binance { get; set; }
        public Investing Investing { get; set; }
    }

    public class Binance
    {
        public string APIKey { get; set; }
        public string APISecret { get; set; }
    }

    public class Investing
    {
        public List<Buy> Buys { get; set; }
    }

    public class Buy
    {
        public string BuyTicker { get; set; }
        public string SellTicker { get; set; }
        public decimal AmountToSell { get; set; }
        public string Cron { get; set; }
    }
}