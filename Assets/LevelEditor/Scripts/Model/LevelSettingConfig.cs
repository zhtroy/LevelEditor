using System;
using System.Collections.Generic;
using Wooga.Foundation.Json;

namespace CommonLevelEditor
{


    public class LevelSettingConfig:IUpdatable
    {
        public List<string> PreventItemCollection { get; private set; }
        public List<string> SpawnChanceCollection { get; private set; }
        public List<string> TotalChanceCollection { get; private set; }
        public List<string> EnsureItemCollection { get; private set; }

        public List<string> ObjectiveCollection { get; private set; }
        public void Update(JSONNode node)
        {
            PreventItemCollection = new List<string>();
            var preventCol = node.GetCollection("prevent_item_collection");
            foreach (var item in preventCol)
            {
                PreventItemCollection.Add(item.AsString());
            }

            SpawnChanceCollection = new List<string>();
            var chanceCol = node.GetCollection("chance_item_collection");
            foreach (var item in chanceCol)
            {
                SpawnChanceCollection.Add(item.AsString());
            }

            TotalChanceCollection = new List<string>();
            var totalCol = node.GetCollection("total_item_collection");
            foreach (var item in totalCol)
            {
                TotalChanceCollection.Add(item.AsString());
            }

            EnsureItemCollection = new List<string>(); 
            var ensureCol = node.GetCollection("ensure_item_collection");
            foreach (var item in ensureCol)
            {
                EnsureItemCollection.Add(item.AsString());
            }

            ObjectiveCollection = new List<string>();
            var ObjCol = node.GetCollection("objective_collection");
            foreach (var item in ObjCol)
            {
                ObjectiveCollection.Add(item.AsString());
            }
        }
    }
}
