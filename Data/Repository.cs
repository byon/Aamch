using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Data
{
    public class Repository
    {
        public class Troop
        {
            public Troop(string name = "")
            {
                Name = name;
            }

            public string Name { get; set; }
        }

        public class IoFailure : Exception
        {
            public IoFailure(string reason)
            {
            }
        }

        Dictionary<string, Troop> troops = new Dictionary<string, Troop>();
        private delegate void IoOperation();
        private delegate void ErrorHandler(Exception error);

        public void Write(string path)
        {
            HandleIoExceptions(() => WriteWithoutErrorHandling(path),
                               (e) => ThrowWriteFailure(e));
        }

        public void Read(string path)
        {
            HandleIoExceptions(() => ReadWithoutErrorHandling(path),
                               (e) => { });
        }

        public void AddTroop(Troop troop)
        {
            /// @todo Troop is not necessarily uniquely identifiable from the
            ///       name. Some troops come from different supplements.
            troops[troop.Name] = troop;
        }

        public bool HasTroop(string name)
        {
            return troops.ContainsKey(name);
        }

        public Troop[] GetTroops()
        {
            return troops.Values.ToArray();
        }

        private void HandleIoExceptions(IoOperation operation,
                                        ErrorHandler errorHandler)
        {
            try
            {
                operation();
            }
            catch (SystemException e)
            {
                errorHandler(e);
            }
        }

        private static void ThrowWriteFailure(Exception e)
        {
            var message = "Failed to write troop file " + e.Message;
            throw new IoFailure(message);
        }

        private void WriteWithoutErrorHandling(string path)
        {
            EnsureDirectoryExistsForFile(path);
            File.WriteAllText(path, TroopsToJson());
        }

        private void ReadWithoutErrorHandling(string path)
        {
            StoreTroops(TroopsFromJson(File.ReadAllText(path)));
        }

        private void StoreTroops(Troop[] troops)
        {
            this.troops = new Dictionary<string, Troop>();
            foreach (var troop in troops)
            {
                AddTroop(troop);
            }
        }

        private Troop[] TroopsFromJson(string json)
        {
            var troopArray = JArray.Parse(json);
            var troopList = troopArray.Select(TroopFromJson);
            return troopList.ToArray();
        }

        private Troop TroopFromJson(JToken json)
        {
            return new Troop((string)json["Name"]);
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

        private JObject TroopToJson(KeyValuePair<string, Troop> pair)
        {
            var troop = pair.Value;
            return new JObject(new JProperty("Name", troop.Name));
        }
    }
}
