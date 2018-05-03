using System;
using System.Collections.Generic;
using Wooga.Foundation.Json;


namespace CommonLevelEditor
{
    interface IUpdatable
    {
        void Update(JSONNode data);
    }
}
