using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;

namespace CollectionOSDBtoCSV
{
    class Program
    {
        public static OsuFileIo OsuFileIo = new OsuFileIo(new BeatmapExtension());

        static void Main(string[] args)
        {
            string input;
            string output;

            if (args.Length != 0)
            {
                input = args[0];
                output = args[1];
            }
            else
            {
                Console.WriteLine("Enter Input Path:");
                input = Console.ReadLine();
                Console.WriteLine("Enter Output Path:");
                output = Console.ReadLine();
            }

            var collections = OsuFileIo.CollectionLoader.LoadOsdbCollections(input);

            string[] lines = new string[collections.AllBeatmaps().Count() + 1];
            lines[0] = "\"" + "CollectionName" + "\",\"" + "StarsNomod" + "\",\"" + "ArtistRoman" + "\",\"" + "ArtistUnicode" + "\",\"" + "DiffName" + "\",\"" + "MapId" + "\",\"" + "MapSetId" + "\",\"" + "Md5" + "\",\"" + "Name" + "\",\"" + "PlayMode" + "\",\"" + "TitleRoman" + "\",\"" + "TitleUnicode" + "\"";
            int i = 1;

            foreach (var collection in collections)
            {
                string CollectionName = collection.Name;

                var beatmaps = collection.AllBeatmaps();

                foreach (var beatmap in beatmaps)
                {
                    var StarsNomod = beatmap.StarsNomod;
                    var ArtistRoman = beatmap.ArtistRoman;
                    var ArtistUnicode = beatmap.ArtistUnicode;
                    var DiffName = beatmap.DiffName;
                    var MapId = beatmap.MapId;
                    var MapSetId = beatmap.MapSetId;
                    var Md5 = beatmap.Md5;
                    var Name = beatmap.Name;
                    var PlayMode = beatmap.PlayMode;
                    var TitleRoman = beatmap.TitleRoman;
                    var TitleUnicode = beatmap.TitleUnicode;

                    lines[i] = "\"" + CollectionName + "\",\"" + StarsNomod + "\",\"" + ArtistRoman + "\",\"" + ArtistUnicode + "\",\"" + DiffName + "\",\"" + MapId + "\",\"" + MapSetId + "\",\"" + Md5 + "\",\"" + Name + "\",\"" + PlayMode + "\",\"" + TitleRoman + "\",\"" + TitleUnicode + "\"";

                    i++;
                }

            }

            using StreamWriter outputfile = new(output);

            foreach (string line in lines)
            {
                outputfile.WriteLine(line);
            }
        }
    }
}