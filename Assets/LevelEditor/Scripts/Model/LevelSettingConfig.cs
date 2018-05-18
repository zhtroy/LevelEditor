using System;
using System.Collections.Generic;
using Wooga.Foundation.Json;

namespace CommonLevelEditor
{


    public class LevelSettingConfig:IUpdatable
    {
        public List<string> PreventItemCollection { get; private set; }
        public void Update(JSONNode node)
        {
            PreventItemCollection = new List<string>();
            var preventCol = node.GetCollection("prevent_item_collection");
            foreach (var item in preventCol)
            {
                PreventItemCollection.Add(item.AsString());
            }
        }
    }
}
