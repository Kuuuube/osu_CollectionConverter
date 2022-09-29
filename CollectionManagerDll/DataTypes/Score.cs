﻿using System;
using System.IO;
using CollectionManager.Enums;
using CollectionManager.Modules.FileIO;

namespace CollectionManager.DataTypes
{
    public class Score : Replay
    {
        public static Score ReadScore(OsuBinaryReader reader, bool minimalLoad = true, int? version = null)
        {
            return (Score)Read(reader, new Score(), minimalLoad, version);
        }
    }
}