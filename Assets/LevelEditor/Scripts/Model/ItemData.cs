using System;
using System.Collections.Generic;
using Wooga.Foundation.Json;

namespace CommonLevelEditor
{
    public class ItemData : IUpdatable
    {
        public const string NONE_ITEM_NAME = "none_item_name";
        public const string NONE_ITEM_ICON = "none_item_icon";
        public string name { get; private set; }
        public string icon { get; private set; }

        public void Update(JSONNode data)
        {
            name = data.GetString("name", NONE_ITEM_NAME);
            icon = data.GetString("icon", NONE_ITEM_ICON);
        }
    }
}
