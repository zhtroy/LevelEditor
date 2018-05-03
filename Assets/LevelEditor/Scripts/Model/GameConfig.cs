using System;
using System.Collections.Generic;
using Wooga.Foundation.Json;

namespace CommonLevelEditor
{
    public class GameConfig : IUpdatable
    {

        public List<string> availableSpawningSets
        {
            get; private set;
        }

        public void Update(JSONNode data)
        {
            var spawningsetnode = data.GetCollection("spawingSets");

            if (availableSpawningSets == null)
            {
                availableSpawningSets = new List<string>();
            }
            else{
                availableSpawningSets.Clear();
            }

            foreach (var item in spawningsetnode)
            {
                availableSpawningSets.Add(item.GetString("name"));
            }

        }

    }
}
