using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Entities.Common
{
    public abstract class BaseEntity 
    {
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        
    }
}
