﻿using CollectionManager.DataTypes;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.FileCollections;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManager.Modules.FileIO.OsuScoresDb;

namespace CollectionManager.Modules.FileIO
{
    public class OsuFileIo
    {
        public OsuDatabase OsuDatabase;
        public OsuSettingsLoader OsuSettings = new OsuSettingsLoader();
        public CollectionLoader CollectionLoader;
        public MapCacher LoadedMaps => OsuDatabase.LoadedMaps;

        public ScoresDatabaseIo ScoresLoader;
        public IScoreDataStorer ScoresDatabase = new ScoresCacher();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beatmapBase">Object that</param>
        public OsuFileIo(Beatmap beatmapBase)
        {
            OsuDatabase = new OsuDatabase(beatmapBase);
            CollectionLoader = new CollectionLoader(OsuDatabase.LoadedMaps);
            ScoresLoader = new ScoresDatabaseIo(ScoresDatabase);
        }
    }
}