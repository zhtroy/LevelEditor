using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    class ColorUtils
    {
        public static List<ColorId> allColors = new List<ColorId> { ColorId.Color0, ColorId.Color1, ColorId.Color2, ColorId.Color3, ColorId.Color4, ColorId.Color5 };
    }
    public enum ColorId
    {
        Color0 = 0,
        Color1,
        Color2,
        Color3,
        Color4,
        Color5,
        None,
        Random
    }

}
