using Microsoft.AspNetCore.Http;
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
            Data = new List<ServerFile>();

            string path = Path.Combine(AppContext.BaseDirectory, "files");
            if (!Directory.Exists(path))
                return;

            foreach (var d in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(d);
                int id;
                if (int.TryParse(di.Name, out id))
                {
                    string desc = null;
                    string name = null;
                    foreach (var f in Directory.GetFiles(d))
                    {
                        string file = Path.GetFileName(f);
                        if (file.ToLower() == "desc.txt")
                        {
                            desc = File.ReadAllText(f, Encoding.UTF8);
                        }
                        else
                        {
                            name = file;
                        }
                    }
                    var sf = new ServerFile() { Id = id, Name = name, Desc = desc };
                    Data.Add(sf);
                }
            }
        }

        internal void DeleteFile(int id)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string dirPath = Path.Combine(path, id.ToString());
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
        }

        public FileContent GetFile(int id)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Stream fileStream = null;
            string fileName = null;
            string dirPath = Path.Combine(path, id.ToString());
            foreach (var f in Directory.GetFiles(dirPath))
            {
                string file = Path.GetFileName(f);
                if (file.ToLower() == "desc.txt")
                {
                    continue; //scip description
                }
                else
                {
                    fileName = file;
                    fileStream = File.OpenRead(f);
                }
            }
            if (fileStream == null || fileName == null)
                throw new Exception("File not found");

            return new FileContent() { FileName = fileName, FileStream = fileStream };
        }

        public void AddFile(IFormFile uploadedFile, string desc)
        {
            var fileName = Path.GetFileName(uploadedFile.FileName);

            string path = Path.Combine(AppContext.BaseDirectory, "files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Load();
            int id = GetNextId();

            string dirPath = Path.Combine(path, id.ToString());
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                uploadedFile.CopyTo(fileStream);
            }

            if (!string.IsNullOrEmpty(desc))
            {
                string descPath = Path.Combine(dirPath, "desc.txt");
                File.WriteAllText(descPath, desc, Encoding.UTF8);
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
