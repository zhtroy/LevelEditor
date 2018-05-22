using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using EnhancedUI.EnhancedScroller;

namespace CommonLevelEditor
{
    public class LevelEntryView : EnhancedScrollerCellView
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

        List<Action<EnhancedScrollerCellView>> delegates = new List<Action<EnhancedScrollerCellView>>();
        private event Action<EnhancedScrollerCellView> _onSelected;

        public event Action<EnhancedScrollerCellView> onSelected
        {
            add
            {
                _onSelected += value;
                delegates.Add(value);
            }

            remove
            {
                _onSelected -= value;
                delegates.Remove(value);
            }
        }


        #region property
        public LevelData Data
        {
            get
            {
                return _data;
            }
        }

        #endregion
        
        void OnDestroy()
        {
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
                
            }
        }
        public void SetData(LevelData data)
        {
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
            }
            
            // link data to view
            _data = data;

            //update view UI
            levelNameText.text = "  "+data.levelNum +"    " + data.name; 

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
            if ( _onSelected!= null)
            {
               // Debug.LogWarning("selected: " + Data.levelNum + " " +Data.name);
                _onSelected(this);
            }
        }

        public void RemoveAllEvents()
        {
            foreach (var eh in delegates)
            {
                _onSelected -= eh;
            }
            delegates.Clear();
        }
    }
}

