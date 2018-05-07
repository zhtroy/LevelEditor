using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace CommonLevelEditor
{
    public class BoardTouchListener : MonoBehaviour, IPointerClickHandler
    {

        public int BoardIndex { get; set; }
        public event Action<int> onMouseClick;




        public void OnPointerClick(PointerEventData eventData)
        {
            if (onMouseClick!=null)
            {
                onMouseClick(BoardIndex);
     
            }
        }


    }
}

