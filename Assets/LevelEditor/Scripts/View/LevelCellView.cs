using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using EnhancedUI.EnhancedScroller;

namespace CommonLevelEditor
{
    public class LevelCellView : EnhancedScrollerCellView
    {
        #region public
        public Text levelNameText;
        public Image selectionImage;
        public Color selectedColor;
        public Color unSelectedColor;
            
        #endregion

        #region private
        private LevelData _data;

        #endregion

        public event Action<EnhancedScrollerCellView> onSelected;

        #region property
        public int DataIndex {get; private set;}

        #endregion
        
        void OnDestroy()
        {
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
                
            }
        }
        public void SetData(int dataIndex,LevelData data)
        {
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
            }
            
            // link data to view
            DataIndex = dataIndex;
            _data = data;

            //update view UI
            levelNameText.text = data.name; 

            //add handler for selection change
            _data.selectedChanged -= SelectedChanged;
            _data.selectedChanged += SelectedChanged;

            SelectedChanged(data.Selected);
        }

        
        private void SelectedChanged(bool selected)
        {
            selectionImage.color = (selected ? selectedColor : unSelectedColor);
        }

        //called by button click event
        public void OnSelected()
        {
            if ( onSelected!= null)
            {
                onSelected(this);
            }
        }
    }
}

