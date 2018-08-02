using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

//UPLOAD Controller

namespace PhotoAlbum.Controllers
{
    public class UploadController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IHostingEnvironment he;

        public UploadController(ApplicationDbContext dbContext, IHostingEnvironment e)
        {
            he = e;
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(List<IFormFile> files)
        {
            int numberOfPictures = files.Count();

            if (numberOfPictures != 0)
            {
                DateTime uploadTime = DateTime.Now;
                string stackName = uploadTime.ToString();

                Stack newStack = new Stack();
                newStack.Name = stackName;
                context.Stacks.Add(newStack);
                context.SaveChanges();

                foreach (var file in files)
                {
                    var fileName = Path.Combine(he.WebRootPath, Path.GetFileName(file.FileName));
                    file.CopyTo(new FileStream(fileName, FileMode.Create));
                    string pathName = "/" + Path.GetFileName(file.FileName).ToString();
                    FileInfo fileInfo = new FileInfo(pathName);
                    DateTime dateTime = fileInfo.CreationTime;

                    Picture newPicture = new Picture();
                    newPicture.Name = pathName;
                    newPicture.StackID = newStack.ID;
                    newPicture.Date = dateTime;
                    context.Pictures.Add(newPicture);
                }
                context.SaveChanges();

                ViewData["Number Of Pictures"] = numberOfPictures.ToString();

                return View();
            }
            return Redirect("/Upload");
        }
    }
}
