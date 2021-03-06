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
        private UndoRedoList _comList;

        public ScrollRect _scrollRect;
        public Button saveBtn;
        public EnhancedScroller myScroller;
        public LevelEntryView levelCellViewPrefab;
        public InputField levelIDText;
        public Text statusText;
        public float cellHeight= 100f;

        public LevelData CurrentLevel
        {
            get
            {
                return _levelList.CurrentSelectedLevel;
            }
        }

        public static LevelListScrollerController instance = null;
        private void Awake()
        {
            instance = this;
           
        }
        void Start()
        {
            
            LoadLevelList();

            _comList = new UndoRedoList();
            _comList.OnClean += OnListClean;
            _comList.OnDirty += OnListDirty;



            myScroller.Delegate = this;
            _levelList.onDataChange += () => {      
                myScroller.ReloadData();
                _scrollRect.onValueChanged.Invoke(_scrollRect.normalizedPosition);
                

            };

            
        }

        private void OnDestroy()
        {
            _comList.OnClean -= OnListClean;
            _comList.OnDirty -= OnListDirty;
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
                var files = Directory.GetFiles(LevelEditorInfo.Instance.FullConfigurationFolderPath, typename + ".json");
                foreach (var filename in files)
                {

                    var node = LevelEditorUtils.JSONNodeFromFileFullPath(filename);

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
            LevelEntryView cellView = scroller.GetCellView(levelCellViewPrefab) as LevelEntryView;
            cellView.name = _levelList[dataIndex].name;
            cellView.RemoveAllEvents();
            cellView.onSelected += CellViewSelected;
            cellView.SetData( _levelList[dataIndex]);

            return cellView;
        }

        private void JumpToLevelID(int levelId, float time=0f)
        {
            int idx = _levelList.IndexFromLevelId(levelId);
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
                   time,
                   () =>
                   {
                       _levelList.SelectSingleLevel(_levelList[idx]);
                   }
               );
        }

        private void CellViewSelected(EnhancedScrollerCellView cellView)
        {
            if (cellView == null)
            {
               
            }
            else
            {
                // get the selected data index of the cell view
                var selectedData = (cellView as LevelEntryView).Data;
                Debug.Log("cell view selected : " + selectedData.levelNum + " Selected: "   + selectedData.Selected);
                //double click to edit level
                if (selectedData.Selected)
                {
                    GotoEditingMode();
                }
                _levelList.SelectSingleLevel(selectedData);

                
            }
        }

        void GotoEditingMode()
        {
            Show(false);
            
            EditingView.instance.Show(true);

        }
        public void Show(bool show)
        {
            gameObject.SetActive(show);
        }
        void OnListDirty()
        {
            statusText.text = "*";
            //saveBtn.gameObject.SetActive(true);
        }
        void OnListClean()
        {
            statusText.text = "";
         //   saveBtn.gameObject.SetActive(false);
        }

        #region called by button
        public void OnDelete()
        {
            if (_levelList.CurrentSelectedLevel== null)
            {
                return;
            }
            var delCom = new ComDeleteLevel(_levelList, _levelList.CurrentSelectedLevel );
            delCom.Execute();
            _comList.Add(delCom);

        }
        public void OnClone()
        {
            if (_levelList.CurrentSelectedLevel == null)
            {
                return;
            }
            
            int levelId;
            if (int.TryParse(levelIDText.text, out levelId))
            {
                LevelData cloneLevel = _levelList.CurrentSelectedLevel.Clone();
                cloneLevel.levelNum = levelId;
                cloneLevel.name += " (Clone)";
                var cloneCom = new ComAddLevel(_levelList, cloneLevel);
                cloneCom.Execute();
                _comList.Add(cloneCom);
                JumpToLevelID(levelId,0.3f);
            }
        }

        public void OnNew()
        {
            int levelId;
            if (int.TryParse(levelIDText.text, out levelId))
            {
                LevelData newlevel = new LevelData();
                newlevel.levelNum = levelId;

                var newCom = new ComAddLevel(_levelList, newlevel);
                newCom.Execute();
                _comList.Add(newCom);

                JumpToLevelID(levelId,0.3f);
            }

        }

        public void OnMoveLevelUp()
        {
            if (_levelList.CurrentSelectedLevel == null)
            {
                return;
            }

            var upCom = new ComMoveLevelUp(_levelList, _levelList.CurrentSelectedLevel);
            if (upCom.Execute())
            {
                _comList.Add(upCom);
                
            }
        }

        public void OnMoveLevelDown()
        {
            if (_levelList.CurrentSelectedLevel == null)
            {
                return;
            }

            var downCom = new ComMoveLevelDown(_levelList, _levelList.CurrentSelectedLevel);
            if (downCom.Execute())
            {
                _comList.Add(downCom);
                
            }
        }

        public void OnGotoLevel()
        {
            int jumpLevelId;
            if (int.TryParse(levelIDText.text, out jumpLevelId))
            {
                JumpToLevelID(jumpLevelId,0.3f);

            }
        }

        public void OnSave()
        {
            _comList.Save();
            _levelList.SaveLevelsToPath(LevelEditorInfo.Instance.FullConfigurationFolderPath);
            
        }
        #endregion
    }
}
