using System;

namespace Saving
{
    [Serializable]
    public class GameSaveData
    {
        public uint BestScore = 0;
        public int HighestUnlockedLevel = 0;
        public float SFXVolume = 1f;
        public float MusicVolume = 1f;
    }
}