using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wooga.Foundation.Json;

namespace CommonLevelEditor

{
    public class BrushData  : IUpdatable
    {
        const string BRUSHNAME = "brush_name";
        const string LAYERID = "layer_id";
        const string TYPEID = "type_id";
        const string HITPOINTS = "hitpoints";
        const string COLOR = "color";
        const string SPRITEID = "sprite_id";
        const string OPTIONS = "options";
        const string EDITORSPRITEID = "editor_sprite_id";
     

        public string BrushName { get; private set; }
        public string LayerId { get; private set; }
        public string TypeId { get; private set; }
        public string Hitpoints { get; private set; }
        public string Color { get; private set; }
        public string SpriteId { get; private set; }
        public string Options { get; private set; }
        public string Editor_spriteId { get; private set; }
        public void Update(JSONNode data)
        {
            BrushName = data.GetString(BRUSHNAME);
            LayerId = data.GetString(LAYERID);
            Hitpoints = data.GetString(HITPOINTS);
            Color = data.GetString(COLOR);
            SpriteId = data.GetString(SPRITEID);
            Options = data.GetString(OPTIONS);
            Editor_spriteId = data.GetString(EDITORSPRITEID);


        }
    }

    public class BrushCategory : IUpdatable
    {
        const string NAME = "category_name";
        const string SPRITE_ID = "sprite_id";
        const string BRUSHES = "brushes";

        public string name { get; private set; }
        public string spriteId { get; private set; }
        public List<BrushData> brushes { get; private set; }


        public void Update(JSONNode data)
        {
            name = data.GetString(NAME);
            spriteId = data.GetString(SPRITE_ID);

            var collection = data.GetCollection(BRUSHES);

            brushes = new List<BrushData>();
            foreach (JSONNode item in collection)
            {
                BrushData b = new BrushData();
                b.Update(item);
                brushes.Add(b);
            }
        }

        public event Action<bool> selectedChanged;

        private bool _selected =false;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                // if the value has changed
                if (_selected != value)
                {
                    // update the state and call the selection handler if it exists
                    _selected = value;
                    if (selectedChanged != null) selectedChanged(_selected);
                }
            }
        }

    }
}
