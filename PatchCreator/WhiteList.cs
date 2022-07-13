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
        public static WhiteListFile[] whitelistJsonInfo;
        private ArrayList whitelist;
        public static List<WhiteList> WhitelistList = new List<WhiteList>();
        public string jsonFileName;
        private bool isFileExists;

        public class WhiteListFile
        {
            public string Path { get; set; }
        }

        public WhiteList(string filename)
        {
            this.jsonFileName = $"{Program.directory}\\{filename}";
            whitelist = new ArrayList();
            SetFileExists();
            if (!isFileExists)
                DeleteJsonFile();
            this.AddWhiteListElement(jsonFileName);
            DefaultWLFiles();
        }

        public void DefaultWLFiles()
        {
            AddWhiteListElement("PatchCreator.exe");
            AddWhiteListElement("PatchCreatorDebug.cmd");
            AddWhiteListElement("PatchCreator.pdb");
        }

        private void SetFileExists()
        {
            if (File.Exists(jsonFileName))
                isFileExists = true;
            else
                isFileExists = false;
        }

        private void DeleteJsonFile()
        {
            File.Delete(jsonFileName);
            isFileExists = false;
        }

        private void SaveData()
        {
            File.WriteAllText(jsonFileName, JsonSerializer.Serialize(whitelistJsonInfo));
        }

        public void AddWhiteListElement(string filename)
        {
            filename = filename.Replace(Program.directory + "\\", "").Replace("\u0022", "");
            whitelist.Add(filename);
        }

        public void ConvertAllWLE()
        {
            whitelistJsonInfo = new WhiteListFile[whitelist.Count];
            int i = 0;
            foreach (string file in whitelist)
            {
                whitelistJsonInfo[i] = new WhiteListFile { Path = file };
                if (Program.IsDebug)
                    Console.WriteLine($"WhiteList File Added: {file}");
                i++;
            }
            SaveData();
        }

        public static bool IsContains(String filename)
        {
            foreach(WhiteListFile whitelist in whitelistJsonInfo)
            {
                string f_filename = filename.Replace(Program.directory + "\\", "");
                if(whitelist.Path.Equals(f_filename))
                    return true;
            }
            return false;
        }
    }
}
