﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

namespace CommonLevelEditor
{
    public class LevelListScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
 
        private LevelDataList _levelList;
        private UndoRedoList _comList = new UndoRedoList();
     

        public Button saveBtn;
        public EnhancedScroller myScroller;
        public LevelCellView levelCellViewPrefab;
        public InputField levelIDText;
        public float cellHeight= 100f;
        void Start()
        {
            LoadLevelList();
            
            


            myScroller.Delegate = this;
            _levelList.OnDataChange += () => { myScroller.ReloadData(); };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) )
            {
                _comList.Undo();
            }
            if (Input.GetKeyUp(KeyCode.Y))
            {
                _comList.Redo();
            }
        }

       

        
        void LoadLevelList()
        {
            _levelList = new LevelDataList();

            foreach (var typename in LevelEditorInfo.Instance.LevelNumToLevelType.Values)
            {
                var files = Directory.GetFiles(LevelEditorInfo.Instance.FullConfigurationFolderPath, typename + "*.json");
                foreach (var filename in files)
                {

                    var node = LevelEditorUtils.JSONNodeFromFile(filename);

                    _levelList.Update(node);

                }
            }

        }
        public int GetNumberOfCells(EnhancedScroller scroller) 
        {
             return _levelList.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) 
        {

             return cellHeight; 
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            LevelCellView cellView = scroller.GetCellView(levelCellViewPrefab) as LevelCellView;
            cellView.name = _levelList[dataIndex].name;
            cellView.onSelected += CellViewSelected;
            cellView.SetData( _levelList[dataIndex]);

            return cellView;
        }


        private void CellViewSelected(EnhancedScrollerCellView cellView)
        {
            if (cellView == null)
            {
               
            }
            else
            {
                // get the selected data index of the cell view
                var selectedData = (cellView as LevelCellView).Data;

                _levelList.SelectSingleLevel(selectedData);
            }
        }


        #region called by button
        public void OnDelete()
        {
            if (_levelList.SelectedLevel== null)
            {
                return;
            }
            var delCom = new ComDeleteLevel(_levelList, _levelList.SelectedLevel );
            delCom.Execute();
            _comList.Add(delCom);

        }
        public void OnClone()
        {

        }

        public void OnNew()
        {

        }

        public void OnGotoLevel()
        {
            int jumpLevelId;
            if (int.TryParse(levelIDText.text, out jumpLevelId))
            {
                int idx =_levelList.IndexFromLevelId(jumpLevelId);
                if (idx == -1)
                {
                    Debug.Log("levelId dont exist");
                    return;
                }
                // jump to the index
                myScroller.JumpToDataIndex(idx,
                    0.5f,
                    0.5f,
                    true,
                    EnhancedScroller.TweenType.linear,
                    0.3f,
                    () =>
                    {
                        _levelList.SelectSingleLevel(_levelList[idx]);
                    }
                );
            }
        }
        #endregion
    }
}
