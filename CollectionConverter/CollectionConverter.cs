using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;

namespace CollectionConverter
{
    class CollectionConverter
    {
        public static OsuFileIo OsuFileIo = new OsuFileIo(new BeatmapExtension());

        static void Main(string[] args)
        {
            string input;
            string output;
            string input_format;
            string output_format;
            string osudb = "0";
            int headers = 0;

            Collections collection_loaded = new Collections();

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
                Console.WriteLine("Enter Input Format:\n1. DB (osu! collection format)\n2. OSDB (Collection Manager format)\n3. CSV (CSV in Collection Converter format)\n4. Folder (All collections in the folder will be parsed based on extension)");
                input_format = Console.ReadLine();
                Console.WriteLine("Enter Output Format:\n1. DB (osu! collection format)\n2. OSDB (Collection Manager format)\n3. CSV (CSV in Collection Converter format)\n41. Folder using DB (All collections individually exported in DB format)\n42. Folder using OSDB (All collections individually exported in OSDB format)\n43. Folder using CSV (All collections individually exported in CSV format)");
                output_format = Console.ReadLine();
                Console.WriteLine("Enter osu!.db path or 0 to skip loading osu!.db");
                osudb = Console.ReadLine();
                if (input_format == "3" || output_format == "3" || input_format == "4" || output_format == "43")
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
                case "41":
                {
                    Import_Export.export_folder_db(output, collection_loaded);
                    break;
                }
                case "42":
                {
                    Import_Export.export_folder_osdb(output, collection_loaded);
                    break;
                }
                case "43":
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