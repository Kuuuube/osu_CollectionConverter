﻿using CollectionManager.DataTypes;
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
            string input_format;
            string output_format;
            string osudb;
            int headers = 0;

            Collections collection_loaded = new Collections();

            if (args.Length != 0)
            {
                input = args[0];
                output = args[1];
                input_format = args[2];
                output_format = args[3];
                osudb = args[4];
                if (input_format == "3" || output_format == "3")
                {
                    headers = int.Parse(args[5]);
                }
            }
            else
            {
                Console.WriteLine("Enter Input Path:");
                input = Console.ReadLine();
                Console.WriteLine("Enter Output Path:");
                output = Console.ReadLine();
                Console.WriteLine("Enter Input Format:\n1. DB (osu! collection format)\n2. OSDB (Collection Manager format)\n3. CSV (CSV in Collection Converter format)");
                input_format = Console.ReadLine();
                Console.WriteLine("Enter Output Format:\n1. DB (osu! collection format)\n2. OSDB (Collection Manager format)\n3. CSV (CSV in Collection Converter format)");
                output_format = Console.ReadLine();
                Console.WriteLine("Enter osu!.db path or 0 to skip loading osu!.db");
                osudb = Console.ReadLine();
                if (input_format == "3" || output_format == "3")
                {
                    Console.WriteLine("Header row in CSV:\n0. No header row\n1. One header row");
                    headers = int.Parse(Console.ReadLine());
                }
            }

            if (osudb != "0" && osudb != null)
            {
                OsuFileIo.OsuDatabase.Load(osudb);
            }

            switch (input_format)
            {
                case "1":
                {
                    collection_loaded = import_db(input);
                    break;
                }
                case "2":
                {
                    collection_loaded = import_osdb(input);
                    break;
                }
                case "3":
                {
                    collection_loaded = import_csv(input, headers);
                    break;
                }
                default:
                {
                    Console.WriteLine("Invalid input format.");
                    break;
                }
            }

            switch (output_format)
            {
                case "1":
                {
                    export_db(output, collection_loaded);
                    break;
                }
                case "2":
                {
                    export_osdb(output, collection_loaded);
                    break;
                }
                case "3":
                {
                    export_csv(output, headers, collection_loaded);
                    break;
                }
                default:
                {
                    Console.WriteLine("Invalid output format.");
                    break;
                }
            }
        }

        public static Collections import_db(string input)
        {
            return OsuFileIo.CollectionLoader.LoadOsuCollection(input);
        }

        public static Collections import_osdb(string input)
        {
            return OsuFileIo.CollectionLoader.LoadOsdbCollections(input);
        }

        public static Collections import_csv(string input, int headers)
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
                        }
                        catch
                        {
                        }

                        try
                        {
                        current_beatmap.ArtistUnicode = values[6].Trim('\"');
                        }
                        catch
                        {
                        }

                        try
                        {
                        current_beatmap.TitleRoman = values[7].Trim('\"');
                        }
                        catch
                        {
                        }

                        try
                        {
                        current_beatmap.TitleUnicode = values[8].Trim('\"');
                        }
                        catch
                        {
                        }

                        try
                        {
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

            return collections;
        }

        public static void export_db(string output, Collections collection_data)
        {
            OsuFileIo.CollectionLoader.SaveOsuCollection(collection_data, output);
        }

        public static void export_osdb(string output, Collections collection_data)
        {
            OsuFileIo.CollectionLoader.SaveOsdbCollection(collection_data, output);
        }

        public static void export_csv(string output, int headers, Collections collection_data)
        {
            string[] lines = new string[collection_data.AllBeatmaps().Count() + 1];

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
            
            foreach (var collection in collection_data)
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
                    var StarsNomod = beatmap.StarsNomod;

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