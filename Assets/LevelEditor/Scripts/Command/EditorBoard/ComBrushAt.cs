using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonLevelEditor
{
    
  
    public class ComBrushAt : ICommand
    {
        EditorBoard _board;
        int _gridX;
        int _gridY;
        BrushData _brushData;

        class ChangedData
        {
           public int x;
           public int y;
           public string layer;
           public string itemName;
        }
        List<ChangedData> _changedDatas = new List<ChangedData>();

        public ComBrushAt(EditorBoard board, BrushData brushData, int gridX,int gridY )
        {
            _board = board;
            _gridX = gridX;
            _gridY = gridY;
            _brushData = brushData;
 
         
        }
        public bool Execute()
        {
            bool canPlaceBrush = true;
      
            //check  if can place brush
            foreach (BrushTile brushTile in _brushData.BrushTiles)
            {
                int tileX = _gridX + (int)(brushTile.Offset.x);
                int tileY = _gridY + (int)(brushTile.Offset.y);

                if (tileX<0 && tileX>=_board.Width)
                {
                    canPlaceBrush = false;
                    break;
                }
                if (tileY<0 && tileY >=_board.Height)
                {
                    canPlaceBrush = false;
                    break;
                }
            }

            if (!canPlaceBrush)
            {
                return false;
            }

            //start to change board model
            

            foreach (BrushTile brushTile in _brushData.BrushTiles)
            {
                int tileX = _gridX + (int)(brushTile.Offset.x);
                int tileY = _gridY + (int)(brushTile.Offset.y);
                foreach (var itemName in brushTile.Items)
                {
                    if (LevelEditorInfo.Instance.DicBoardItem.ContainsKey(itemName))
                    {
                        BoardItem boardItem = LevelEditorInfo.Instance.DicBoardItem[itemName];
                        foreach (var sublayerPair in boardItem.SubLayerChars)
                        {
                            string oldItem= _board.SetItemAt(boardItem.LayerId, tileX, tileY, boardItem.Name);
                            if (oldItem!= boardItem.Name)
                            {
                                _changedDatas.Add(new ChangedData {x=tileX,y=tileY,layer=boardItem.LayerId,itemName = boardItem.Name });
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("brush contains unknown item: " + itemName);
                    }
                    
                }
               
            }
            if (_changedDatas.Count ==0)
            {
                return false;
            }



            return true;    
        }
        public void Undo()
        {
            foreach (var data in _changedDatas)
            {
                _board.SetItemAt(data.layer, data.x, data.y, data.itemName);
            }
         
        }
    }
}
