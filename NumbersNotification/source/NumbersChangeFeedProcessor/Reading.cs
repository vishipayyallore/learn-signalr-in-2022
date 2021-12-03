using Newtonsoft.Json;

namespace NumbersChangeFeedProcessor
{
    public class Reading
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; } = "No Name";

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; } = 9999;
    }
}
