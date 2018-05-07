using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonLevelEditor
{
    public class CoordinationHex : ICoordination
    {
        const float SCREEN_Y_OFFSET = -1f;
        const float SCREEN_X_INTERVAL = 0.945f;

        private int _width;
        private int _height;
        public CoordinationHex(int w, int h)
        {
            _width = w;
            _height = h;
        }
        #region ICoordination
        public Vector2 PosFromIndex(int index)
        {
            int gridX = GetGridX(index);
            int gridY = GetGridY(index);
            float screenX = GetScreenX(gridX);
            float screenY = GetScreenY(gridY, gridX);

            return new Vector2(screenX, screenY);
        }
        #endregion

        int GetGridX(int idx)
        {
            int x = idx % _width;

            if (x < 0)
            {
                x += _width;
            }

            return Mathf.Abs(x);
        }
        int GetGridY(int index)
        {
            float y = (float)index / _width;

            return Mathf.FloorToInt(y);
        }

        public float GetScreenX(float x)
        {
            return Mathf.Abs(x) * SCREEN_X_INTERVAL ;
        }

        public float GetScreenY(float y, float x)
        {
            const float offset = -0.5f;
            int gridX = Mathf.FloorToInt(x);
            float rest = (x % 1) * offset;
            bool isEven = gridX % 2 == 0;

            if (!isEven)
            {
                rest = -rest;
                y += offset;
            }

            y += rest;

            return -(y);
        }
    }
}
