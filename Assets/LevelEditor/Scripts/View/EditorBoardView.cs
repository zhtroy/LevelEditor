using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CommonLevelEditor
{

    public class EditorBoardView : MonoBehaviour
    {
        #region variable
        private EditorBoard _board;
        private List<BoardTouchListener> _listListeners = new List<BoardTouchListener>();
        private ICoordination _coord;

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
        public Image brushIcon;

        private void OnDestroy()
        {
            CleanCellCollidersCallBacks();
            _board.onDataChanged -= RefreshSingleCell;
        }
        // Use this for initialization
        void Start()
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
           
            _board = new EditorBoard(width, height);
            _board.onDataChanged += RefreshSingleCell;
            
            InitCellViews(_board);
            InitBoardCellColliders();

        }


        private void RefreshSingleCell(string layername, int idx, string data)
        {

        }

        void InitCellViews(EditorBoard board)
        {
            //use fixed for now
            List<string> layerList = new List<string>{ ConfigLayerId.FIELDS, ConfigLayerId.COLORS, ConfigLayerId.COVERS };
            foreach (var item in layerList)
            {
                GameObject layer = Instantiate(prefabLayerContainer);
                layer.name = item;
                layer.transform.SetParent(transform);

                RectTransform trans = layer.transform as RectTransform;
                trans.anchoredPosition = Vector2.zero;

                _layers[item] = new List<BoardCellView>();
                for (int i = 0; i < board.CellNum; i++)
                {
                    BoardCellView cell = Instantiate(prefabCellView);
                    cell.name += i;
                    cell.transform.SetParent(layer.transform);
                    cell.transform.localPosition = _coord.PosFromIndex(i) * cellCoordScale;
                    _layers[item].Add(cell);

                }
            }
            RefreshBoardView();
        }

        void RefreshBoardView()
        {

        }
        void InitBoardCellColliders()
        {
            GameObject colliderRoot = Instantiate(prefabLayerContainer);
            colliderRoot.name = "CollierRoot";
            colliderRoot.transform.SetParent(transform);
            RectTransform trans = colliderRoot.transform as RectTransform;
            trans.anchoredPosition = Vector2.zero;
                 
          
            for (int i = 0; i < _board.CellNum; i++)
            {
                BoardTouchListener listener = Instantiate(prefabHexCollider);

                listener.transform.SetParent(colliderRoot.transform);

               
                listener.transform.localPosition = _coord.PosFromIndex(i)* cellCoordScale;

                listener.name += i.ToString();

                listener.BoardIndex = i;

                listener.onLeftMousePress += OnClickedIndex;

                _listListeners.Add(listener);
            }
            




        }

       
        void CleanCellCollidersCallBacks()
        {
            foreach (var item in _listListeners)
            {
                item.onLeftMousePress -= OnClickedIndex;

            }
        }

        void OnClickedIndex (int idx)
        {
            Debug.Log("I'm clicked " + idx);

        }

        #region button calls
        public void OnSave()
        {

        }
        public void OnBrush()
        {

        }
        public void OnLastBrush()
        {

        }

        public void OnNextBrush()
        {

        }

       
        #endregion


    }
}
