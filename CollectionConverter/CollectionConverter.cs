using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;

namespace CollectionConverter
{
    class CollectionConverter
    {
        public static OsuFileIo OsuFileIo = new(new BeatmapExtension());

        static void Main(string[] args)
        {
            string? input;
            string? output;
            string? input_format;
            string? output_format;
            string? osudb = "0";
            int headers = 0;

            Collections collection_loaded = [];

            if (args.Length != 0)
            {
                input = args[0];
                output = args[1];
                input_format = args[2];
                output_format = args[3];
                try
                {
                    osudb = args[4];
                }
                catch
                {
                    Console.WriteLine("osu!.db path not defined. Using default: " + osudb);
                }
                
                if (input_format == "3" || output_format == "3" || input_format == "4" || output_format == "43")
                {
                    try
                    {
                        headers = int.Parse(args[5]);
                    }
                    catch
                    {
                        Console.WriteLine("Header row not defined. Using default: " + headers);
                    }
                }
            }
            else
            {
                Console.WriteLine("Enter Input Path:");
                input = Console.ReadLine();
                Console.WriteLine("Enter Output Path:");
                output = Console.ReadLine();
                Console.WriteLine("Enter Input Format:\n"
                    + "1. DB (osu! collection format)\n"
                    + "2. OSDB (Collection Manager format)\n"
                    + "3. CSV (CSV in Collection Converter format)\n"
                    + "4. REALM (osu! lazer collection format)\n"
                    + "5. Folder (All collections in the folder will be parsed based on extension)");
                input_format = Console.ReadLine();
                Console.WriteLine("Enter Output Format:\n"
                    + "1. DB (osu! collection format)\n"
                    + "2. OSDB (Collection Manager format)\n"
                    + "3. CSV (CSV in Collection Converter format)\n"
                    + "4. REALM (osu! lazer collection format)\n"
                    + "51. Folder using DB (All collections individually exported in DB format)\n"
                    + "52. Folder using OSDB (All collections individually exported in OSDB format)\n"
                    + "53. Folder using CSV (All collections individually exported in CSV format)");
                output_format = Console.ReadLine();
                Console.WriteLine("Enter osu!.db path, client.realm path (lazer) or 0 to skip loading osu! client database");
                osudb = Console.ReadLine();
                if (input_format == "3" || output_format == "3" || input_format == "5" || output_format == "53")
                {
                    Console.WriteLine("Header row in CSV:\n"
                        + "0. No header row\n"
                        + "1. One header row");
                    var header_input = Console.ReadLine();
                    if (header_input != null && header_input.Length > 0) {
                        headers = int.Parse(header_input);
                    }
                }
            }

            if (input == null || output == null) {
                return;
            }

            if (osudb != "0" && osudb != null)
            {
                OsuFileIo.OsuDatabase.Load(osudb);
            }

            switch (input_format)
            {
                case "1":
                {
                    collection_loaded = Import_Export.import_db(input);
                    break;
                }
                case "2":
                {
                    collection_loaded = Import_Export.import_osdb(input);
                    break;
                }
                case "3":
                {
                    collection_loaded = Import_Export.import_csv(input, headers);
                    break;
                }
                case "4":
                {
                    collection_loaded = Import_Export.import_lazer_db(input);
                    break;
                }
                case "5":
                {
                    collection_loaded = Import_Export.import_folder(input, headers);
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
                    Import_Export.export_db(output, collection_loaded);
                    break;
                }
                case "2":
                {
                    Import_Export.export_osdb(output, collection_loaded);
                    break;
                }
                case "3":
                {
                    Import_Export.export_csv(output, collection_loaded, headers);
                    break;
                }
                case "4":
                {
                    Import_Export.export_lazer_db(output, collection_loaded);
                    break;
                }
                case "51":
                {
                    Import_Export.export_folder_db(output, collection_loaded);
                    break;
                }
                case "52":
                {
                    Import_Export.export_folder_osdb(output, collection_loaded);
                    break;
                }
                case "53":
                {
                    Import_Export.export_folder_csv(output, collection_loaded, headers);
                    break;
                }
                default:
                {
                    Console.WriteLine("Invalid output format.");
                    break;
                }
            }
        }
    }
}