using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NotesApp
{
    public static class ConfigManager
    {
        public static JObject LoadStyleDonfig()
        {
            if (File.Exists("StyleConfig.json"))
            {
                string config = File.ReadAllText("StyleConfig.json");
                return JObject.Parse(config);
            }
            return null;
        }
    }
}
