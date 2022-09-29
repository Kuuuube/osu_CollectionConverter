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
            lines[0] = "\"" + "Collection" + "\",\"" + "StarsNomod" + "\",\"" + "Artist" + "\",\"" + "ArtistRoman" + "\",\"" + "ArtistUnicode" + "\",\"" + "DiffName" + "\",\"" + "MapId" + "\",\"" + "MapLink" + "\",\"" + "MapSetId" + "\",\"" + "MapSetLink" + "\",\"" + "Md5" + "\",\"" + "Name" + "\",\"" + "PlayMode" + "\",\"" + "Title" + "\",\"" + "TitleRoman" + "\",\"" + "TitleUnicode" + "\"";
            int i = 1;

            foreach (var collection in collections)
            {
                string CollectionName = collection.Name;

                var beatmaps = collection.AllBeatmaps();

                foreach (var beatmap in beatmaps)
                {
                    var StarsNomod = beatmap.StarsNomod;
                    var Artist = beatmap.Artist;
                    var ArtistRoman = beatmap.ArtistRoman;
                    var ArtistUnicode = beatmap.ArtistUnicode;
                    var DiffName = beatmap.DiffName;
                    var MapId = beatmap.MapId;
                    var MapLink = beatmap.MapLink;
                    var MapSetId = beatmap.MapSetId;
                    var MapSetLink = beatmap.MapSetLink;
                    var Md5 = beatmap.Md5;
                    var Name = beatmap.Name;
                    var PlayMode = beatmap.PlayMode;
                    var Title = beatmap.Title;
                    var TitleRoman = beatmap.TitleRoman;
                    var TitleUnicode = beatmap.TitleUnicode;

                    lines[i] = "\"" + CollectionName + "\",\"" + StarsNomod + "\",\"" + Artist + "\",\"" + ArtistRoman + "\",\"" + ArtistUnicode + "\",\"" + DiffName + "\",\"" + MapId + "\",\"" + MapLink + "\",\"" + MapSetId + "\",\"" + MapSetLink + "\",\"" + Md5 + "\",\"" + Name + "\",\"" + PlayMode + "\",\"" + Title + "\",\"" + TitleRoman + "\",\"" + TitleUnicode + "\"";

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