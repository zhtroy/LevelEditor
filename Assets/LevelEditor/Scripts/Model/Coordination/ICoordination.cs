﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLevelEditor
{
    interface ICoordination
    {
        Vector2 PosFromIndex(int index);
        int GetGridX(int idx);
        int GetGridY(int idx);



    }
}
