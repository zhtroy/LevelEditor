using UnityEngine;
using System.Collections;

namespace CommonLevelEditor
{
    public class EditingView : MonoBehaviour
    {
        public static EditingView instance = null;

        public EditorBoardView boardView;
        private void Awake()
        {
            instance = this;
            gameObject.SetActive(false);
            
        }

        public void Show(bool show)
        {
            gameObject.SetActive(show);
            if (show)
            {
                boardView.Init();
            }
           
        }
    }

    

}
