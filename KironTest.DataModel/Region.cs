using System.Text.Json.Serialization;

namespace KironTest.DataModel
{
    public class Region
    {
        public string Name { get; set; }
        [JsonPropertyName("division")]
        public string Division { get; set; }
        [JsonPropertyName("events")]
        public List<Event> Events { get; set; } = new List<Event>();
    }
}
