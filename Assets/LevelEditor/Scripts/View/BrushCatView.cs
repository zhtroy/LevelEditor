using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;

namespace CommonLevelEditor
{


    public class BrushCatView : MonoBehaviour
    {

        public BrushView prefabBrushView;
        public GameObject upArrow;
        public GameObject downArrow;
        public Image header;

        List<BrushView> _listBrush;
        bool _expand = false;
        public void SetData(BrushCategory data)
        {

            _listBrush = new List<BrushView>();
            header.sprite = Resources.Load<Sprite>(LevelEditorInfo.Instance.WhichGame + "/Sprites/" + data.spriteId);
            foreach (var brushData in data.brushes)
            {  
                BrushView brushView = Instantiate(prefabBrushView);
                brushView.transform.SetParent(transform);
                brushView.transform.localScale = new Vector3(1, 1, 1);
                brushView.SetData(brushData);
                _listBrush.Add(brushView);
            }
            _expand = false;
            OnExpand(_expand);
        }

        public void ToggleExpand()
        {
            _expand = !_expand;
            OnExpand(_expand);
        }
        private void OnExpand(bool value)
        {
       
            foreach (var item in _listBrush)
            {
                item.gameObject.SetActive(value);
             }

            upArrow.SetActive(value);
            downArrow.SetActive(!value);

        }




    }
}