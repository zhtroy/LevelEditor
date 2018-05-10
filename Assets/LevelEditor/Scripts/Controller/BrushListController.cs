using UnityEngine;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Wooga.Foundation.Json;
using UnityEngine.UI;
namespace CommonLevelEditor
{

    // view and model
    public class BrushListController : MonoBehaviour
    {
        
        private List<BrushCategory> _list;

        public BrushData CurrentBrush { get; private set; }
        public Transform brushRoot;
        public BrushCatView prefabBrushCatView;
        public Image currentBrushImage;

        private void Start()
        {
            JSONNode node = LevelEditorUtils.JSONNodeFromFileResourcesPath(
                LevelEditorInfo.Instance.EditorConfigPath + LevelEditorInfo.FILE_BRUSH);
            UpdateData(node);



            foreach (var data in _list)
            {
                BrushCatView cat = Instantiate(prefabBrushCatView);
                cat.transform.SetParent(brushRoot);
                cat.transform.localScale = new Vector3(1, 1, 1);
                cat.SetData(data);
            }
        }

        void UpdateData(JSONNode data)
        {
            var col = data.GetCollection("category");
           
            _list = new List<BrushCategory>();

            foreach (var node in col)
            {
                BrushCategory cat = new BrushCategory();
                cat.Update(node);
                _list.Add(cat);
            }

        }


        void OnCurrentBrush(BrushData data)
        {
            CurrentBrush = data;
            currentBrushImage.sprite = Resources.Load<Sprite>(LevelEditorInfo.Instance.WhichGame + "/Sprites/" + CurrentBrush.SpriteId);

        }









    }
}

