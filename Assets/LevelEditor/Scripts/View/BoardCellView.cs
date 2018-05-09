using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CommonLevelEditor
{

    public class BoardCellView : MonoBehaviour
    {

        public Image image;

        public void SetImage(Sprite s)
        {
            if (s == null)
            {
                return;
            }
            image.sprite = s;
            var c = image.color;
            c.a = 1;
            image.color = c;
        }

        public void ClearImage ()
        {
            image.sprite = null;
            var c = image.color;
            c.a = 0;
            image.color = c;
        }

    }
}
