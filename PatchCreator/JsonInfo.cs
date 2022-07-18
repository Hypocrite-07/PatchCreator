using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PatchCreator
{
    public class ProjectInfo
    {
        public String Version { get; set; }
        public jsonFileInfo[] Entries { get; set; } 
    }
    public class jsonFileInfo
    {
        public String Path { get; set; }
        public String Hash { get; set; }
        public long Size { get; set; }
    }
    
    internal class JsonInfo
    {
        private string jsonFileName;
        private string version;
        private bool isFileExists;
        
        public JsonInfo(string jsonFileName, string version)
        {
            this.jsonFileName = $"{Program.directory}\\{jsonFileName}";
            this.version = version;
            SetFileExists();
            if(isFileExists)
                DeleteJsonFile();
        }

        private void SetFileExists()
        {
            if(File.Exists(jsonFileName))
                isFileExists = true;
            else
                isFileExists = false;
        }

        private void DeleteJsonFile()
        {
            File.Delete(jsonFileName);
            isFileExists = false;
        }

        public void JsonPaster(Dictionary<string, string> pairs)
        {
            Console.WriteLine("\nStep 5: Setter JSON File");
            jsonFileInfo[] fileInfos = new jsonFileInfo[pairs.Count];
            jsonFileInfo[] result_fileInfos;
            int i = 0;
            int NN = 0;
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                if (Program.IsDebug)
                    Console.WriteLine($"Key: {pair.Key} / Value: {pair.Value}");
                var formated_path = Program.GetFormatedPath(pair.Key);
                if (WhiteList.IsContains(formated_path))
                {
                    if (Program.IsDebug)
                        Console.WriteLine($"[WL] File \"{formated_path}\" willn't be included in patch info");
                }
                else
                {
                    fileInfos[i] = new jsonFileInfo { Hash = pair.Value, Path = formated_path, Size = new FileInfo(pair.Key).Length };
                    i++;
                }
            }
            result_fileInfos = new jsonFileInfo[i];
            i = 0;
            foreach(jsonFileInfo fileInfo in fileInfos)
            {
                if (fileInfo != null)
                {
                    result_fileInfos[i] = fileInfo;
                    i++;
                }
            }
            //result_fileInfos.Concat(fileInfos);
            var projInfo = new ProjectInfo { Version = version, Entries = result_fileInfos};
            if (Program.IsDebug)
            {
                Console.WriteLine();
                Console.WriteLine(JsonSerializer.Serialize(projInfo));
            }
            SaveData(projInfo);
        }

        private void SaveData(ProjectInfo _projInfo)
        {
            Console.WriteLine("\nStep 6: Fill Json File");
            try
            {
                File.WriteAllText(jsonFileName, JsonSerializer.Serialize(_projInfo));
                Console.WriteLine("\nTask be completed is success.");
            } catch (Exception e)
            {
                Console.WriteLine($"Error excepted: \n{e.Message}\n{e.StackTrace}");
            }
        }

        
    }
}
