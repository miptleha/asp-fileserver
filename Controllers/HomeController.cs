using asp_fileserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace asp_fileserver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var m = new ServerFileManager();
            m.Load();

            return View(m.Data);
        }

        public IActionResult Load(int id)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Upload()
        {
            if (Request.Form.Files.Count > 0)
            {
                string path = Path.Combine(AppContext.BaseDirectory, "files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var uploadedFile = Request.Form.Files[0];
                var fileName = Path.GetFileName(uploadedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }

                var desc = Request.Form["file-desc"][0];
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
