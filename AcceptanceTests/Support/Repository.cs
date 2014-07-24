using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AcceptanceTests
{
    public class Repository
    {
        public class IoFailure : Exception
        {
            public IoFailure(string reason) :
                base(reason)
            {
            }
        }

        List<Dictionary<string, string>> troops =
            new List<Dictionary<string, string>>();

        public void AddTroop(Dictionary<string, string> troop)
        {
            troops.Add(troop);
        }

        public void Write(string path)
        {
            EnsureDirectoryExistsForFile(path);
            var json = TroopsToJson();
            File.WriteAllText(path, json);
        }

        private void EnsureDirectoryExistsForFile(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        private string TroopsToJson()
        {
            var jsonTroops = from t in troops select TroopToJson(t);
            return new JArray(jsonTroops).ToString();
        }

        private JObject TroopToJson(Dictionary<string, string> troop)
        {
            var properties = troop.Select(p => new JProperty(p.Key, p.Value));
            return new JObject(properties.ToArray());
        }
    }
}
