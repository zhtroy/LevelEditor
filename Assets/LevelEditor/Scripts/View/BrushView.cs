using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CommonLevelEditor
{
    public class BrushView : MonoBehaviour
    {
        public Image image;


        private BrushData _data;
        public void SetData(BrushData data)
        {
            _data = data;
            image.sprite = Resources.Load<Sprite>(LevelEditorInfo.Instance.WhichGame + "/Sprites/" + data.SpriteId);
        }


        public void OnClick()
        {
            SendMessageUpwards("OnCurrentBrush", _data);
        }

    }
}
