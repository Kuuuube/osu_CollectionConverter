using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;

namespace CollectionConverter
{
    class Program
    {
        public static OsuFileIo OsuFileIo = new OsuFileIo(new BeatmapExtension());

        static void Main(string[] args)
        {
            string input;
            string output;
            string option;
            int headers;

            if (args.Length != 0)
            {
                input = args[0];
                output = args[1];
                option = args[2];
                headers = int.Parse(args[3]);
            }
            else
            {
                Console.WriteLine("Enter Input Path:");
                input = Console.ReadLine();
                Console.WriteLine("Enter Output Path:");
                output = Console.ReadLine();
                Console.WriteLine("Enter Option:");
                option = Console.ReadLine();
                Console.WriteLine("Number of header rows:");
                headers = int.Parse(Console.ReadLine());
            }

            switch (option)
            {
                case "1":
                {
                    osdb_to_csv(input, output, headers);
                    break;
                }
                case "2":
                {
                    csv_to_osdb(input, output, headers);
                    break;
                }
                default:
                {
                    Console.WriteLine("Invalid option.");
                    break;
                }
            }
        }

        public static void csv_to_osdb(string input, string output, int headers)
        {
            string[] lines;
            if (headers == 0)
            {
                lines = System.IO.File.ReadAllLines(input);
            }
            else
            {
                lines = System.IO.File.ReadAllLines(input).Skip(headers).ToArray();
            }
            
            List<string> CollectionNames = new List<string>();
            
            var collections = new Collections();

            foreach (string line in lines)
            {
                var values = line.Split(",");
                CollectionNames.Add(values[0].Trim('\"'));
            }

            foreach (string CollectionInList in CollectionNames.Distinct())
            {
                var current_collection = new Collection(OsuFileIo.LoadedMaps) { Name = CollectionInList };

                foreach (string line in lines)
                {
                    var values = line.Split(",");

                    var CollectionName = values[0].Trim('\"');

                    if (CollectionName == CollectionInList)
                    {
                        var current_beatmap = new BeatmapExtension();

                        try
                        {
                            current_beatmap.MapId = int.Parse(values[1].Trim('\"'));
                        }
                        catch
                        {
                            current_beatmap.MapId = -1;
                        }
                        
                        try
                        {
                            current_beatmap.MapSetId = int.Parse(values[2].Trim('\"'));
                        }
                        catch
                        {
                            current_beatmap.MapSetId = -1;
                        }
                        
                        try
                        {
                            current_beatmap.Md5 = values[3].Trim('\"');
                        }
                        catch
                        {
                            current_beatmap.Md5 = "";
                        }
                        
                        string gamemode;
                        try
                        {
                            gamemode = values[4].Trim('\"');
                        }
                        catch
                        {
                            gamemode = "Osu";
                        }
                        

                        switch (gamemode)
                        {
                            case "Osu":
                            {
                                current_beatmap.PlayMode = CollectionManager.Enums.PlayMode.Osu;
                                break;
                            }
                            case "Taiko":
                            {
                                current_beatmap.PlayMode = CollectionManager.Enums.PlayMode.Taiko;
                                break;
                            }
                            case "OsuMania":
                            {
                                current_beatmap.PlayMode = CollectionManager.Enums.PlayMode.OsuMania;
                                break;
                            }
                            case "CatchTheBeat":
                            {
                                current_beatmap.PlayMode = CollectionManager.Enums.PlayMode.CatchTheBeat;
                                break;
                            }
                            default:
                            {
                                current_beatmap.PlayMode = CollectionManager.Enums.PlayMode.Osu;
                                break;
                            }
                        }
                        try
                        {
                        current_beatmap.ArtistRoman = values[5].Trim('\"');
                        current_beatmap.ArtistUnicode = values[6].Trim('\"');
                        current_beatmap.TitleRoman = values[7].Trim('\"');
                        current_beatmap.TitleUnicode = values[8].Trim('\"');
                        current_beatmap.DiffName = values[9].Trim('\"');
                        }
                        catch
                        {

                        }

                        try
                        {
                            current_beatmap.StarsNomod = double.Parse(values[10].Trim('\"'));
                        }
                        catch
                        {
                            current_beatmap.StarsNomod = -1d;
                        }
                        
                        current_collection.AddBeatmap(current_beatmap);
                    }
                }
                
                
                collections.Add(current_collection);
                current_collection = null;
            }

            OsuFileIo.CollectionLoader.SaveOsdbCollection(collections, output);
        }

        public static void osdb_to_csv(string input, string output, int headers)
        {
            var collections = OsuFileIo.CollectionLoader.LoadOsdbCollections(input);

            string[] lines = new string[collections.AllBeatmaps().Count() + 1];

            int i;
            if (headers >= 1)
            {
                lines[0] = "\"" + "CollectionName" + "\",\"" + "MapId" + "\",\"" + "MapSetId" + "\",\"" + "Md5" + "\",\"" + "PlayMode" + "\",\"" + "ArtistRoman" + "\",\"" + "ArtistUnicode" + "\",\"" + "TitleRoman" + "\",\"" + "TitleUnicode" + "\",\"" + "DiffName" + "\",\"" + "StarsNomod" + "\"";
                i = 1;
            }
            else
            {
                i = 0;
            }
            
            foreach (var collection in collections)
            {
                string CollectionName = collection.Name;

                var beatmaps = collection.AllBeatmaps();

                foreach (var beatmap in beatmaps)
                {
                    var MapId = beatmap.MapId;
                    var MapSetId = beatmap.MapSetId;
                    var Md5 = beatmap.Md5;
                    var PlayMode = beatmap.PlayMode;
                    var ArtistRoman = beatmap.ArtistRoman;
                    var ArtistUnicode = beatmap.ArtistUnicode;
                    var TitleRoman = beatmap.TitleRoman;
                    var TitleUnicode = beatmap.TitleUnicode;
                    var DiffName = beatmap.DiffName;
                    var StarsNomod = beatmap.StarsNomodParse;

                    lines[i] = "\"" + CollectionName + "\",\"" + MapId + "\",\"" + MapSetId + "\",\"" + Md5 + "\",\"" + PlayMode + "\",\"" + ArtistRoman + "\",\"" + ArtistUnicode + "\",\"" + TitleRoman + "\",\"" + TitleUnicode + "\",\"" + DiffName + "\",\"" + StarsNomod + "\"";

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