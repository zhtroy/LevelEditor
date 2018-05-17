using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CommonLevelEditor
{
    //view and controller
    public class EditorBoardView : MonoBehaviour
    {
        //为保持写出json格式一致，使用一个EditorBoard做为model，是LevelData的一个中间层
        #region variable
        private EditorBoard _board;
        private List<BoardTouchListener> _listListeners = new List<BoardTouchListener>();
        private ICoordination _coord;
        private UndoRedoList _comList = new UndoRedoList();

        private Dictionary<string, List<BoardCellView>> _layers = new Dictionary<string, List<BoardCellView>>();
        #endregion

        public enum BoardType
        {
            Hex
        }

        public int cellCoordScale;
        public BoardType boardType = BoardType.Hex;
        public BoardTouchListener prefabHexCollider;
        public BoardCellView prefabCellView;
        public GameObject prefabLayerContainer;
        public Text statusText;
        public BrushListController brushList;
        List<GameObject> _viewLayers = new List<GameObject>();
        List<GameObject> _colliderLayers = new List<GameObject>();

        private void OnDestroy()
        {
            CleanCellCollidersCallBacks();
            if (_board!=null)
            {
                _board.onDataChanged -= RefreshSingleCell;
            }
            

            _comList.OnClean -= _comList_OnClean;
            _comList.OnDirty -= _comList_OnDirty;
        }
        // Use this for initialization
        public void Init()
        {
            int width = LevelEditorInfo.Instance.BoardWidth;
            int height = LevelEditorInfo.Instance.BoardHeight;

            switch (boardType)
            {
                case BoardType.Hex:
                    _coord = new CoordinationHex(width,height);
                    break;
                default:
                    break;
            }

            
            _board = new EditorBoard(width, height, LevelListScrollerController.instance.CurrentLevel);
            _board.onDataChanged += RefreshSingleCell;


            
            InitCellViews(_board);
            InitBoardCellColliders();


            _comList = new UndoRedoList();
            _comList.OnClean += _comList_OnClean;
            _comList.OnDirty += _comList_OnDirty;
        }

        private void _comList_OnDirty()
        {
            statusText.text = "*";
        }

        private void _comList_OnClean()
        {
            statusText.text = "";
        }

        private void RefreshSingleCell(string layername, int idx, string itemname)
        {
            if (_layers.ContainsKey(layername))
            {
                if (string.IsNullOrEmpty(itemname))
                {
                    _layers[layername][idx].ClearImage();

                }
                else
                {
                    var sprite = LevelEditorInfo.Instance.GetItemSpriteByName(itemname);
                    _layers[layername][idx].SetImage(sprite);
                }
               
            } 
        }

        void InitCellViews(EditorBoard board)
        {
            //destroy old layers
            foreach (var layer in _viewLayers)
            {
                DestroyImmediate(layer.gameObject);
            }
            _viewLayers.Clear();

            foreach (var item in board.Layers.Keys)
            {
                GameObject layer = Instantiate(prefabLayerContainer);
                _viewLayers.Add(layer);

                layer.name = item;
                layer.transform.SetParent(transform);

                RectTransform trans = layer.transform as RectTransform;
                trans.anchoredPosition = Vector2.zero;
                trans.localScale = new Vector3(1, 1, 1);

                _layers[item] = new List<BoardCellView>();
                for (int i = 0; i < board.CellNum; i++)
                {
                    BoardCellView cell = Instantiate(prefabCellView);
                    cell.name = item +"CellView"+ i;
                    cell.transform.SetParent(layer.transform);
                    cell.transform.localPosition = _coord.PosFromIndex(i) * cellCoordScale;
                    cell.transform.localScale = new Vector3(1, 1, 1);
                    _layers[item].Add(cell);

                }
            }
            RefreshBoardView();
        }

        void RefreshBoardView()
        {
            foreach (var layer in _board.Layers)
            {
                for(int i=0;i<layer.Value.Count;i++)
                {
                    string itemname = layer.Value[i];

                    if (string.IsNullOrEmpty(itemname))
                    {
                        _layers[layer.Key][i].ClearImage();
                       
                    }
                    else
                    {
                        var sprite = LevelEditorInfo.Instance.GetItemSpriteByName(itemname);
                        _layers[layer.Key][i].SetImage(sprite);
                    }

                }
            }
        }
        void InitBoardCellColliders()
        {

            //destroy old layers
            foreach (var layer in _colliderLayers)
            {
                DestroyImmediate(layer.gameObject);
            }
            _colliderLayers.Clear();


            GameObject colliderRoot = Instantiate(prefabLayerContainer);
            _colliderLayers.Add(colliderRoot);

            colliderRoot.name = "CollierRoot";
            colliderRoot.transform.SetParent(transform);
            RectTransform trans = colliderRoot.transform as RectTransform;
            trans.anchoredPosition = Vector2.zero;
            trans.localScale = new Vector3(1, 1, 1);
                 
          
            for (int i = 0; i < _board.CellNum; i++)
            {
                BoardTouchListener listener = Instantiate(prefabHexCollider);

                listener.transform.SetParent(colliderRoot.transform);
                listener.transform.localScale = new Vector3(1, 1, 1);
               
                listener.transform.localPosition = _coord.PosFromIndex(i)* cellCoordScale;
                

                listener.name += i.ToString();

                listener.BoardIndex = i;

                listener.onLeftMouseBrushAction += OnBrushIndex;

                _listListeners.Add(listener);
            }
            




        }

       
        void CleanCellCollidersCallBacks()
        {
            foreach (var item in _listListeners)
            {
                item.onLeftMouseBrushAction -= OnBrushIndex;

            }
        }

        void OnBrushIndex (int idx)
        {
            Debug.Log("I'm brushed " + idx);
            BrushData brushData = brushList.CurrentBrush;
            if (brushData == null)
            {
                Debug.LogWarning("没有选择画刷");
                return;
            }

            int gridX = _coord.GetGridX(idx);
            int gridY = _coord.GetGridY(idx);
            ICommand brushCom = new ComBrushAt(_board, brushData, gridX,gridY);
            if (brushCom.Execute())
            {
                _comList.Add(brushCom);
            }
            else
            {
                Debug.LogWarning("can't place brush at this position: (" + gridX + "," + gridY + ")");
            }
            
            

        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnUndo();
            }
            if (Input.GetKeyUp(KeyCode.Y))
            {
                OnRedo();
            }
        }
        #region button calls
        public void OnSave()
        {
            _comList.Save();
            _board.UpdateRelatedLevelData();
            LevelListScrollerController.instance.OnSave();
        }
        public void OnUndo()
        {
            _comList.Undo();
        }

        public void OnRedo()
        {
            _comList.Redo();
        }


        public void OnExitToLevelList()
        {
            LevelListScrollerController.instance.Show(true);
            EditingView.instance.Show(false);

        }

       
        #endregion


    }
}
