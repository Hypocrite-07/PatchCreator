using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatchCreator
{
    internal class WhiteList
    {
        public static WhiteListFile[] WhitelistJsonInfo;
        private ArrayList whitelist;
        public string JsonFileName;
        private bool isFileExists;

        public class WhiteListFile
        {
            public string Path { get; set; }
        }

        public WhiteList(string filename)
        {
            this.JsonFileName = $"{Program.directory}\\{filename}";
            whitelist = new ArrayList();
            SetFileExists();
            if (!isFileExists)
                DeleteJsonFile();
            this.AddWhiteListElement(JsonFileName);
            DefaultWLFiles();
        }

        public void DefaultWLFiles()
        {
            AddWhiteListElement("PatchCreator.exe");
        }

        private void SetFileExists()
        {
            if (File.Exists(JsonFileName))
                isFileExists = true;
            else
                isFileExists = false;
        }

        private void DeleteJsonFile()
        {
            File.Delete(JsonFileName);
            isFileExists = false;
        }

        private void SaveData()
        {
            File.WriteAllText(JsonFileName, JsonSerializer.Serialize(WhitelistJsonInfo));
        }

        public void AddWhiteListElement(string filename)
        {
            filename = Program.GetFormatedPath(filename).Replace("\u0022", "");
            whitelist.Add(filename);
        }

        public void ConvertAllWLE()
        {
            WhitelistJsonInfo = new WhiteListFile[whitelist.Count];
            int i = 0;
            foreach (string file in whitelist)
            {
                WhitelistJsonInfo[i] = new WhiteListFile { Path = file };
                if (Program.IsDebug)
                    Console.WriteLine($"WhiteList File Added: {file}");
                i++;
            }
            SaveData();
        }

        public static bool IsContains(String filename)
        {
            foreach(WhiteListFile whitelist in WhitelistJsonInfo)
            {
                string f_filename = Program.GetFormatedPath(filename);
                if(whitelist.Path.Equals(f_filename))
                    return true;
            }
            return false;
        }
    }
}
