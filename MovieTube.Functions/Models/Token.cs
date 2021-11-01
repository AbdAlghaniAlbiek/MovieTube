using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MovieTube.Functions.Models
{
    public class Token
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "expiredIn")]
        public string ExpiredIn { get; set; }

        [JsonProperty(PropertyName = "secretKeyword")]
        public string SecretKeyword { get; set; }
    }
}
