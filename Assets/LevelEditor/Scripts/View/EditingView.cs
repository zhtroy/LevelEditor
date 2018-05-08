using UnityEngine;
using System.Collections;

namespace CommonLevelEditor
{
    public class EditingView : MonoBehaviour
    {
        public static EditingView instance = null;
        private void Awake()
        {
            instance = this;
            gameObject.SetActive(false);
        }

        public void Show(bool show)
        {
            gameObject.SetActive(show);
        }
    }

    

}
