using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assets.Tenenet.Player
{
    public class Score
    {
        [JsonProperty("metric_id")]
        public string MetricId { get; set; }

        [JsonProperty("metric_name")]
        public string MetricName { get; set; }

        [JsonProperty("metric_type")]
        public string MetricType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Message
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("score")]
        public List<Score> Score { get; set; }
    }

    public class Root
    {
        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

namespace Assets.Tenenet.Rank
{
    public class Rank
    {
        public string alias { get; set; }
        public int rank { get; set; }
    }

    public class Message
    {
        public List<Rank> data { get; set; }
    }

    public class Root
    {
        public Message message { get; set; }
        public string status { get; set; }
    }
}