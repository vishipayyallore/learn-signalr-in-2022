using Newtonsoft.Json;

namespace NumbersNotification.Core.Domain
{

    public class Reading
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; } = "No Name";

        [JsonProperty(PropertyName = "createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; } = 9999;
    }

}
