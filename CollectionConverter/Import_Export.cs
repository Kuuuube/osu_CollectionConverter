using System.Text;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;

namespace CollectionConverter
{
    class Import_Export
    {
        public static Collections import_db(string input)
        {
            return CollectionConverter.OsuFileIo.CollectionLoader.LoadOsuCollection(input);
        }

        public static Collections import_osdb(string input)
        {
            return CollectionConverter.OsuFileIo.CollectionLoader.LoadOsdbCollections(input);
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

            Collections collections = new Collections();

            foreach (string line in lines)
            {
                string[] values = line.Split(",");
                CollectionNames.Add(values[0].Trim('\"'));
            }

            foreach (string CollectionInList in CollectionNames.Distinct())
            {
                Collection current_collection = new Collection(CollectionConverter.OsuFileIo.LoadedMaps) { Name = CollectionInList };

                foreach (string line in lines)
                {
                    List<char> chars_list = new();
                    char[] line_chars = line.ToCharArray();
                    List<string> values = new();
                    bool found_quote = false;
                    int i = 0;
                    while (i < line_chars.Length)
                    {
                        char current_char = safe_get_char(line_chars, i);
                        char next_char = safe_get_char(line_chars, i + 1);

                        if (current_char == ',' && found_quote == false) {
                            try
                            {
                                values.Add(new string(chars_list.ToArray()));
                                chars_list = new();
                            }
                            catch
                            {
                                values.Add('\0'.ToString());
                                chars_list = new();
                            }
                            i += 1;
                            continue;
                        }
                        if (current_char == '"' && next_char == '"' && found_quote == true) {
                            chars_list.Add(current_char);
                            i += 2;
                            continue;
                        }
                        if (current_char == '"' && found_quote == false) {
                            found_quote = true;
                            i += 1;
                            continue;
                        }
                        if (current_char == '"' && found_quote == true) {
                            found_quote = false;
                            i += 1;
                            continue;
                        }
                        chars_list.Add(current_char);

                        i += 1;
                    }
                    if (i == line_chars.Length) {
                        try
                        {
                            values.Add(new string(chars_list.ToArray()));
                            chars_list = new();
                        }
                        catch
                        {
                            values.Add('\0'.ToString());
                            chars_list = new();
                        }
                    }

                    string CollectionName = values[0].Trim('\"');

                    if (CollectionName == CollectionInList)
                    {
                        BeatmapExtension current_beatmap = new BeatmapExtension();

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
            }

            return collections;
        }

        public static Collections import_folder(string input, int headers)
        {
            Collections merged_collection = new Collections();
            Collections imported_file;
            string[] files = Directory.GetFiles(input);
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".db")
                {
                    imported_file = import_db(file);
                    foreach (Collection collection in imported_file)
                    {
                        merged_collection.Add(collection);
                    }
                }
                if (Path.GetExtension(file) == ".osdb")
                {
                    imported_file = import_osdb(file);
                    foreach (Collection collection in imported_file)
                    {
                        merged_collection.Add(collection);
                    }
                }
                if (Path.GetExtension(file) == ".csv")
                {
                    imported_file = import_csv(file, headers);
                    foreach (Collection collection in imported_file)
                    {
                        merged_collection.Add(collection);
                    }
                }
            }
            return merged_collection;
        }  

        public static void export_db(string output, Collections collection_data)
        {
            CollectionConverter.OsuFileIo.CollectionLoader.SaveOsuCollection(collection_data, output);
        }

        public static void export_osdb(string output, Collections collection_data)
        {
            CollectionConverter.OsuFileIo.CollectionLoader.SaveOsdbCollection(collection_data, output);
        }

        public static void export_csv(string output, Collections collection_data, int headers)
        {
            string[] lines = new string[collection_data.AllBeatmaps().Count() + headers];

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
            
            foreach (Collection collection in collection_data)
            {
                string CollectionName = collection.Name;

                IEnumerable<BeatmapExtension> beatmaps = collection.AllBeatmaps();

                foreach (BeatmapExtension beatmap in beatmaps)
                {
                    int MapId = beatmap.MapId;
                    int MapSetId = beatmap.MapSetId;
                    string Md5 = beatmap.Md5;
                    CollectionManager.Enums.PlayMode PlayMode = beatmap.PlayMode;
                    string ArtistRoman = beatmap.ArtistRoman;
                    string ArtistUnicode = beatmap.ArtistUnicode;
                    string TitleRoman = beatmap.TitleRoman;
                    string TitleUnicode = beatmap.TitleUnicode;
                    string DiffName = beatmap.DiffName;
                    double StarsNomod = beatmap.StarsNomod;

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

        public static void export_folder_db(string output, Collections collection_data)
        {
            foreach (Collection collection in collection_data)
            {
                export_db(output + "\\" + fix_filename(collection.Name) + ".db", new Collections { collection });
            }
        }

        public static void export_folder_osdb(string output, Collections collection_data)
        {
            foreach (Collection collection in collection_data)
            {
                export_osdb(output + "\\" + fix_filename(collection.Name) + ".osdb", new Collections { collection });
            }
        }

        public static void export_folder_csv(string output, Collections collection_data, int headers)
        {
            foreach (Collection collection in collection_data)
            {
                export_csv(output + "\\" + fix_filename(collection.Name) + ".csv", new Collections { collection }, headers);
            }
        }

        //replaces illegal characters for underscores
        public static string fix_filename(string filename)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(invalidChar.ToString(), "_");
            }
            return filename;
        }

        public static char safe_get_char(char[] array, int index) {
            if (index >= array.Length) {
                return '\0';
            } else {
                return array[index];
            }
        }
    }
}