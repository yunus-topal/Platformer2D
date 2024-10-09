using System.Collections.Generic;

namespace General {
    [System.Serializable]
    public class GameData
    {
        public int playerGold;
        public List<string> upgrades;
        public string checkpoint;
        public List<string> bossesKilled;

    }
}