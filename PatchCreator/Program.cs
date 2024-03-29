﻿using System.Collections;
using System.Security.Cryptography;

namespace PatchCreator
{
    class Program
    {
        public static string directory;
        private static bool isDebug = false;
        public static bool IsDebug { get { return isDebug; } }
        private static ArrayList files = new ArrayList();
        private static JsonInfo json;
        public static WhiteList wl;
        private static Dictionary<string, string> dick = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            Start(args);
            Setter();
            EndApp(false);
        }

        public static string GetFormatedPath(string path)
        {
            return path.Replace(directory + "\\", "");
        }

        static void Start(string[] args)
        {
            Console.WriteLine("Patch Creator / by Hypocrite07\n");
            if (args.Length == 1)
            {
                if (args[0] == "-D" || args[0] == "--debug")
                {
                    isDebug = true;
                    Console.WriteLine("Debug Mode is Activated");
                }
                else
                {
                    Console.WriteLine("Args was be entered incorrect.\nExamples args:\n\nDebug Mode: -D or --debug");
                    EndApp(true);
                }
            }
            directory = Directory.GetCurrentDirectory();
            Console.WriteLine("Step 1: Init Dir and Version");
            Console.WriteLine($"Current directory is: \"{directory}\"\n");
            Console.WriteLine("Enter product version(x.x.x): ");
            var version = Console.ReadLine();
            if (version == null)
                version = "1.0.0";
            Console.WriteLine("\nStep 2: Init Json's File");
            wl = new WhiteList("patch-whitelist");
            json = new JsonInfo("patch-info", version);
            Console.WriteLine("\nStep 2.1: Enter Path of WhiteList files/dirs(write \"0\" for exit):");
            while(true)
            {
                var word = Console.ReadLine();
                if (word.StartsWith("0"))
                    break;
                wl.AddWhiteListElement(word);
            }
            wl.ConvertAllWLE();
            Console.WriteLine("\nStep 3: Setter Dirs And Files");
            SetterDirsAndFiles(directory);
        }

        public static void SetterDirsAndFiles(string path)
        {   
            var directories = Directory.GetDirectories(path);

            foreach (string _dir in directories)
            {
                if (WhiteList.IsContains(GetFormatedPath(_dir)))
                    continue;
                if (isDebug)
                    Console.WriteLine($"dir added {_dir}");
                    SetterDirsAndFiles(_dir);
            }
            var _files = Directory.GetFiles(path);
            foreach (string _file in _files)
            {
                if(isDebug)
                    Console.WriteLine($"file added {_file}");
                files.Add(_file);
            }
        }

        static void Setter()
        {
            Console.WriteLine("\nStep 4: HashSetter");
            foreach(var file in files)
            {
                string filename = file as string;
                string hash = Get256HashSumFile(filename);
                dick.Add(filename, hash);
                if (isDebug)
                    Console.WriteLine($"\"{filename}\" hash is \"{hash}\"");
            }
            json.JsonPaster(dick);
        }

        static string Get256HashSumFile(string filename)
        {
            if (filename == null) return null;
            FileStream fs = File.OpenRead(filename);
            byte[] fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);
            fs.Close();
            return Convert.ToHexString(SHA256.Create().ComputeHash(fileData)).ToLower();
        }

        static void EndApp(bool sure)
        {
            if (sure == false)
            {
                Console.WriteLine("\nStep X: Finish App Work.");
                Console.WriteLine("\nEnter any key for exit from app.");
                Console.ReadKey();
            }
            Environment.Exit(0);
        }

    }
}