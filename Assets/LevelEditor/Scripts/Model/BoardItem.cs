using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wooga.Foundation.Json;


namespace CommonLevelEditor
{
    public  class BoardItem :IUpdatable
    {
        public string Name { get;private set; }
        public string LayerId { get; private set; }
        
        public Dictionary<string, string> SubLayerChars { get; private set; }


        public string SpriteId { get; private set; }

        public void Update(JSONNode data)
        {
            Name = data.GetString("name");
            LayerId = data.GetString("layer_id");
            SpriteId = data.GetString("sprite_id");

            SubLayerChars = new Dictionary<string, string>();

            var dic = data.GetDictionary("sublayers");
            foreach (var item in dic)
            {
                SubLayerChars.Add(item.Key, item.Value.AsString());

            }



        }
    }
}
