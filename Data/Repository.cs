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
                SoldierAttack = new AttackValues();
                VehicleAttack = new AttackValues();
            }

            public class DefenseValues
            {
                public int Front { get; set; }
                public int Rear { get; set; }
            }

            public class AttackValues
            {
                public int Short { get; set; }
                public int Medium { get; set; }
                public int Long { get; set; }
            }

            public string Name { get; set; }
            public int Cost { get; set; }
            public string Type { get; set; }
            public string Subtype { get; set; }
            public DefenseValues Defense { get; set; }
            public AttackValues SoldierAttack { get; set; }
            public AttackValues VehicleAttack { get; set; }
        }

        public class IoFailure : Exception
        {
            public IoFailure(string reason) :
                base(reason)
            {
            }
        }

        Dictionary<string, Troop> troops = new Dictionary<string, Troop>();

        public void Read(string path)
        {
            try
            {
                ReadWithoutErrorHandling(path);
            }
            catch (JsonException e)
            {
                ThrowIoFailure(e);
            }
            catch (SystemException e)
            {
                ThrowIoFailure(e);
            }
        }

        public void AddTroop(Troop troop)
        {
            /// @todo Troop is not necessarily uniquely identifiable from the
            ///       name. Some troops come from different supplements.
            troops[troop.Name] = troop;
        }

        public Troop[] GetTroops()
        {
            return troops.Values.ToArray();
        }

        private static void ThrowIoFailure(Exception e)
        {
            var message = "Failed to read troop file " + "'" + e.Message + "'";
            throw new IoFailure(message);
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
            ReadDefenseToTroop(json, result);
            ReadAttacksToTroop(json, result);
            return result;
        }

        private void ReadDefenseToTroop(JToken json, Troop result)
        {
            int? front = (int?)json["Fdef"];
            int? rear = (int?)json["Rdef"];

            if (front == null || rear == null)
            {
                result.Defense = null;
                return;
            }

            result.Defense.Front = (int)front;
            result.Defense.Rear = (int)rear;
        }

        private void ReadAttacksToTroop(JToken json, Troop result)
        {
            result.SoldierAttack = ReadAttacks(json, "S");
            result.VehicleAttack = ReadAttacks(json, "V");
        }

        private Troop.AttackValues ReadAttacks(JToken json, string typeId)
        {
            var result = new Troop.AttackValues();
            result.Short = (int)json["S" + typeId];
            result.Medium = (int)json["M" + typeId];
            result.Long = (int)json["L" + typeId];
            return result;
        }
    }
}
