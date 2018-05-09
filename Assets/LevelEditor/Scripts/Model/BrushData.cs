using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wooga.Foundation.Json;
using UnityEngine;

namespace CommonLevelEditor

{

    public class BrushTile : IUpdatable
    {
        public Vector2 Offset { get; private set; }
        public List<string> Items { get; private set; }
        public void Update(JSONNode data)
        {
            int offsetX = data.GetInt("offsetx");
            int offsetY = data.GetInt("offsety");
            Offset = new Vector2(offsetX, offsetY);

            Items = new List<string>();

            var collection = data.GetCollection("items");
            foreach (var item in collection)
            {

                Items.Add(item.AsString());

            }


        }
    }

    
    public class BrushData  : IUpdatable
    {

        const string BRUSHNAME = "brush_name";
        const string BRUSHTILES = "brush_tiles";

        const string EDITORSPRITEID = "sprite_id";
   
     

        public string BrushName { get; private set; }
       
        public List<BrushTile> BrushTiles { get; private set; }
        
        
        public string SpriteId { get; private set; }
        public void Update(JSONNode data)
        {
            BrushName = data.GetString(BRUSHNAME);
            BrushTiles = new List<BrushTile>();
            var collection = data.GetCollection(BRUSHTILES);
            foreach (var item in collection)
            {
                BrushTile tile = new BrushTile();
                tile.Update(item);
                BrushTiles.Add(tile);
            }
            SpriteId = data.GetString("sprite_id");


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
