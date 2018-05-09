using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace CommonLevelEditor
{
    public class BoardTouchListener : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {

        public int BoardIndex { get; set; }
        public event Action<int> onLeftMousePress;
        public event Action<int> onRightMousePress;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)

            {
                if (onLeftMousePress != null)
                {
                    onLeftMousePress(BoardIndex);

                }

            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (onRightMousePress != null)
                {
                    onRightMousePress(BoardIndex);

                }
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
      

            if (Input.GetMouseButton(0))
 
            {
                if (onLeftMousePress != null)
                {
                    onLeftMousePress(BoardIndex);

                }

            }
            else if (Input.GetMouseButton(1))
            {
                if (onRightMousePress != null)
                {
                    onRightMousePress(BoardIndex);

                }
            }
        }


    }
}

