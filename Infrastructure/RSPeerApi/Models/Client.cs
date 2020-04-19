using System;
using Newtonsoft.Json;

namespace Infrastructure.RSPeerApi.Models
{
    [Serializable]
    public class Client
    {
        [JsonProperty(PropertyName = "RsUsername")]
        public string Username { get; set; } = "";

        [JsonProperty(PropertyName = "RsPassword")]
        public string Password { get; set; } = "";
        
        public int World { get; set; } = -1;
        
        public string ScriptName { get; set; }

        public bool IsRepoScript { get; set; }

        public string ScriptArgs { get; set; }
        
        public bool UseProxy { get; set; }
        
        public int ProxyPort { get; set; }

        public string ProxyIp { get; set; }

        public string ProxyUser { get; set; }

        public string ProxyPass { get; set; }

        public Config Config { get; set; } = new Config();
    }
    
    [Serializable]
    public class Config
    {
        public bool LowCpuMode { get; set; } = false;

        public bool SuperLowCpuMode { get; set; } = false;

        public int EngineTickDelay { get; set; } = 0;

        public bool DisableModelRendering { get; set; }

        public bool DisableSceneRendering { get; set; }
    }
}