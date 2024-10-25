using System.Text.Json.Serialization;

namespace KironTest.DataModel
{
    public class Event
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
        [JsonPropertyName("bunting")]
        public bool Bunting { get; set; }
    }
}
