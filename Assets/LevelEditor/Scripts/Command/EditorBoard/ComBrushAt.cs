using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    //一般来说，命令只修改model，但由于棋盘用的LevelData做为模型，和view分离不够
    //简便起见，同时修改model和view
    public class ComBrushAt : ICommand
    {
        LevelData _level;
        EditorBoardView _boardView;
        BrushData _brushData;
        int _gridX;
        int _gridY;
        LevelData _snapshotLevel;
        public ComBrushAt(LevelData level, EditorBoardView boardview, BrushData brushData, int gridX, int gridY )
        {
            _level = level;
            _boardView = boardview;
            _brushData = brushData;
            _gridX = gridX;
            _gridY = gridY;
            _snapshotLevel = _level.Clone();
        }
        public void Execute()
        {
            bool dirty = false;
            //model
            foreach (BrushTile brushTile in _brushData.BrushTiles)
            {
                int tileX = _gridX + (int)(brushTile.Offset.x);
                int tileY = _gridY + (int)(brushTile.Offset.y);

                if (tileX<0 && tileX>=_level.width)
                {
                    Undo();
                    
                }
            }


            //view
            
        }
        public void Undo()
        {
            //model
            _level = _snapshotLevel;
            //view
        }
    }
}
