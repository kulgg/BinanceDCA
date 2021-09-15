![build](https://github.com/JlKmn/BinanceDCA/actions/workflows/dotnet.yml/badge.svg)
# BinanceDCA
This bot allows you to automatically DCA (Dollar-cost average) into your preferred cryptocurrencies on Binance. Buy frequency and amount to purchase of each cryptocurrency can be defined in the configuration. Simple & fast setup.

## Getting Started
### Binance API Keys
1. [Generate a new API Key on Binance](https://www.binance.com/en/my/settings/api-management)
2. Check "Enable Reading" and "Enable Spot & Margin Trading".

### Setup
[.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) needed
```
git clone https://github.com/JlKmn/BinanceDCA.git
```

### Config
To use this bot to execute your custom DCA strategy, you only have to:
1. Add your Binance API Key & Secret to `appsettings.json`
2. Place your desired buys inside the "Buys" list in `appsettings.json`. For example:
```
"Buys": [
            { "BuyTicker": "BTC", "SellTicker": "USDT", "AmountToSell": 10, "Cron": "0 0 8 ? * FRI *" },
            { "BuyTicker": "HBAR", "SellTicker": "USDT", "AmountToSell": 10, "Cron": "0 0 8 ? * FRI *" }
        ]
```
The config of the first buy translates to: "Buy 10 USD of Bitcoin every Friday at 8am".

You can add a custom buy frequency for every cryptocurrency you want to DCA into by using the Cron field in the config.

Don't know what Cron expressions are? [Cron Expression Generator](https://www.freeformatter.com/cron-expression-generator-quartz.html) might be useful for generating them.
