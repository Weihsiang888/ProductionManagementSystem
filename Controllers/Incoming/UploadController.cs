using DxBlazorApplication7.Data;
using Microsoft.AspNetCore.Mvc;

namespace DxBlazorApplication7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class UploadController : ControllerBase
    {
        protected string ContentRootPath { get; set; }
        public UploadController(IWebHostEnvironment hostingEnvironment)
        {
            ContentRootPath = hostingEnvironment.ContentRootPath;
        }
        [HttpPost("[action]")]
        public ActionResult UploadFile(IFormFile myFile, string Folder)
        {
            try
            {
                var path = Path.Combine(ContentRootPath, "wwwroot\\Uploads\\" + Folder.Replace("_","\\"));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string[] fileExtensions = { ".pdf", ".PDF" };
                var fileName = myFile.FileName.ToLower();
                //string ErpVersion = myFile.FileName.Split('.')[0].Split('_').Last();
                var isValidExtenstion = fileExtensions.Any(ext => {
                    return fileName.LastIndexOf(ext) > -1;
                });
                if (isValidExtenstion)
                {
                    bool _IsExists = System.IO.File.Exists(Path.Combine(path, myFile.FileName));
                    if (_IsExists)
                    {
                        System.IO.File.Delete(Path.Combine(path, myFile.FileName));
                    }
                    using (var fileStream = System.IO.File.Create(Path.Combine(path, myFile.FileName)))
                    {
                        myFile.CopyTo(fileStream);
                    }
                }
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
