using System;

namespace Saving
{
    [Serializable]
    public class GameSaveData
    {
        public uint BestScore = 0;
        public int HighestUnlockedLevel = 1;
    }
}