namespace Parser
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Car
    {
        public bool IsValid { get; set; }
        [JsonProperty("certificate")]
        public string Certificate { get; set; }
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        public int Year { get; set; }
        [JsonProperty("lot")]
        public string Lot { get; set; }
        [JsonProperty("category")]
        public string Category{ get; set; }
        [JsonProperty("damage")]
        public string Damage { get; set; }
        [JsonProperty("engineType")]
        public string EngineType { get; set; }
        [JsonProperty("bids")]
        public HashSet<string> Bids { get; set; }
        [JsonProperty("images")]
        public HashSet<string> Images { get; set; }
    }
}