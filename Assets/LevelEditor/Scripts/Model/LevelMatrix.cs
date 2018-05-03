using System.Collections.Generic;
using Wooga.Foundation.Json;
using UnityEngine;

namespace CommonLevelEditor
{
    public class LevelMatrix
    {
        readonly char[,] _values;

        public int width { get { return _values.GetLength(0); } }
        public int height { get { return _values.GetLength(1); } }

        public LevelMatrix(int width, int height, char defValue = '-')
        {
            _values = new char[width, height];

            for (int y = 0; y < _values.GetLength(1); y++)
            {
                for (int x = 0; x < _values.GetLength(0); x++)
                {
                    _values[x, y] = defValue;
                }
            }
        }

        public LevelMatrix(IList<JSONNode> rows, int width, int height)
        {
            _values = new char[width, height];

            var gap = height - rows.Count;

            for (int i = 0; i < gap; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _values[j, i] = '-';
                }
            }

            int count = rows.Count;

            for (int y = 0; y < count; y++)
            {
                string str = rows[y].AsString();

                for (int x = 0; x < width; x++)
                {
                    _values[x, y + gap] = x < str.Length ? str[x] : '-';
                }
            }
        }

        public LevelMatrix(LevelMatrix matrix)
        {
            _values = new char[matrix.width, matrix.height];

            for (int y = 0; y < _values.GetLength(1); y++)
            {
                for (int x = 0; x < _values.GetLength(0); x++)
                {
                    _values[x, y] = matrix.Get(x, y);
                }
            }
        }

        public char Get(int x, int y)
        {
            if (x < _values.GetLength(0) && y < _values.GetLength(1))
            {
                return _values[x, y];
            }
            else
            {
                Debug.LogError("Could not get level entry at position " + x + ", " + y);
                return '\0';
            }
        }

        public void Set(int x, int y, char value)
        {
            if (x < _values.GetLength(0) && y < _values.GetLength(1))
            {
                _values[x, y] = value;
            }
            else
            {
                Debug.LogError("Could not set level entry at position " + x + ", " + y);
            }
        }

        public void Set(int x, int y, string value)
        {
            if (value.Length == 1)
            {
                Set(x, y, value[0]);
            }
            else
            {
                Debug.LogError("Could not set level entry because '" + value + "' has a length of " + value.Length);
            }
        }

        public List<string> Serialize()
        {
            var list = new List<string>();

            for (int y = 0; y < _values.GetLength(1); y++)
            {
                var row = "";

                for (int x = 0; x < _values.GetLength(0); x++)
                {
                    row += _values[x, y];
                }

                list.Add(row);
            }


            return list;
        }

        public int GetCountOf(char c)
        {
            int count = 0;

            for (int y = 0; y < _values.GetLength(1); y++)
            {
                for (int x = 0; x < _values.GetLength(0); x++)
                {
                    if (_values[x, y] == c)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public override bool Equals(object obj)
        {
            var other = (LevelMatrix)obj;

            if (other == null
               || other.width != this.width
               || other.height != this.height)
            {
                return false;
            }

            bool result = true;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result &= other.Get(x, y) == this.Get(x, y);
                }
            }

            return result;
        }
        public override int GetHashCode()
        {
            return _values.GetHashCode();
        }
    }
}
