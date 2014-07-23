using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data
{
    public class Repository
    {
        public class Troop
        {
            public Troop(string name)
            {
                Name = name;
                Defense = new DefenseValues();
            }

            public class DefenseValues
            {
                public int Front { get; set; }
                public int Rear { get; set; }
            }

            public string Name { get; set; }
            public int Cost { get; set; }
            public string Type { get; set; }
            public string Subtype { get; set; }
            public DefenseValues Defense { get; set; }
        }

        public class IoFailure : Exception
        {
            public IoFailure(string reason) :
                base(reason)
            {
            }
        }

        Dictionary<string, Troop> troops = new Dictionary<string, Troop>();
        private delegate void IoOperation();
        private delegate void ErrorHandler(Exception error);

        public void Write(string path)
        {
            HandleExceptions(() => WriteWithoutErrorHandling(path),
                             (e) => ThrowIoFailure(e, "write"));
        }

        public void Read(string path)
        {
            HandleExceptions(() => ReadWithoutErrorHandling(path),
                             (e) => ThrowIoFailure(e, "read"));
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

        private void HandleExceptions(IoOperation operation,
                                      ErrorHandler errorHandler)
        {
            try
            {
                operation();
            }
            catch (JsonException e)
            {
                errorHandler(e);
            }
            catch (SystemException e)
            {
                errorHandler(e);
            }
        }

        private static void ThrowIoFailure(Exception e, string operation)
        {
            var message = "Failed to " + operation + " troop file " +
                          "'" + e.Message + "'";
            throw new IoFailure(message);
        }

        private void WriteWithoutErrorHandling(string path)
        {
            EnsureDirectoryExistsForFile(path);
            var json = TroopsToJson();
            File.WriteAllText(path, json);
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
            var result = new Troop((string)json["Name"]);
            result.Cost = (int)json["Cost"];
            result.Type = (string)json["Type"];
            result.Subtype = (string)json["Subtype"];
            result.Defense.Front = (int)json["Fdef"];
            result.Defense.Rear = (int)json["Rdef"];
            return result;
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
            var result = new JObject(new JProperty("Name", troop.Name),
                                     new JProperty("Cost", troop.Cost),
                                     new JProperty("Type", troop.Type),
                                     new JProperty("Subtype", troop.Subtype));
            if (troop.Defense != null)
            {
                result.Add(new JProperty("Fdef", troop.Defense.Front));
                result.Add(new JProperty("Rdef", troop.Defense.Rear));
            }
            return result;
        }
    }
}
