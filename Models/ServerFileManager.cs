using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace asp_fileserver.Models
{
    public class ServerFileManager
    {
        public List<ServerFile> Data { get; private set; }
        public void Load()
        {
            string json = null;
            string path = Path.Combine(AppContext.BaseDirectory, "files.txt");
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    json = sr.ReadToEnd();
                }

                Data = JsonSerializer.Deserialize<List<ServerFile>>(json);
            }
            else
            {
                Data = new List<ServerFile>();
            }
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize<List<ServerFile>>(Data);
            string path = Path.Combine(AppContext.BaseDirectory, "files.txt");
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }

        public int GetNextId()
        {
            if (Data.Count == 0)
                return 1;

            return Data.Max(f => f.Id) + 1;
        }
    }
}
