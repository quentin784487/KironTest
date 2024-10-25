using System.Text.Json.Serialization;

namespace KironTest.DataModel
{
    public class PagedResult
    {
        [JsonPropertyName("result")]
        public List<CryptoCurrency> Result { get; set; }
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
    }

    public class CryptoCurrency
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("rank")]
        public long Rank { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("priceBtc")]
        public double PriceBtc { get; set; }
        [JsonPropertyName("volume")]
        public double Volume { get; set; }
        [JsonPropertyName("marketCap")]
        public double MarketCap { get; set; }
        [JsonPropertyName("availableSupply")]
        public long AvailableSupply { get; set; }
        [JsonPropertyName("totalSupply")]
        public long TotalSupply { get; set; }
        [JsonPropertyName("fullyDilutedValuation")]
        public double FullyDilutedValuation { get; set; }
        [JsonPropertyName("priceChange1h")]
        public double PriceChange1h { get; set; }
        [JsonPropertyName("priceChange1d")]
        public double PriceChange1d { get; set; }
        [JsonPropertyName("priceChange1w")]
        public double PriceChange1w { get; set; }
        [JsonPropertyName("redditUrl")]
        public string RedditUrl { get; set; }
        [JsonPropertyName("twitterUrl")]
        public string TwitterUrl { get; set; }
        [JsonPropertyName("explorers")]
        public List<string> Explorers { get; set; }        
    }    

    public class Meta
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        [JsonPropertyName("itemCount")]
        public int ItemCount { get; set; }
        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }
        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}
