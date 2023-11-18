using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FileUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private IHostingEnvironment hostingEnv;
        private string folderPath = @"C:\temp\file_uploads";
        public FileController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [Route("Upload/{chunkBucket}")]

        [HttpPost]
        public IActionResult Upload(IList<IFormFile> UploadFiles, [FromRoute] string chunkBucket, [FromForm] string metadata)
        {
            Metadata meta = null;
            if (UploadFiles != null)
            {
                if (!string.IsNullOrEmpty(metadata))
                {
                    meta = JsonConvert.DeserializeObject<Metadata>(metadata);
                }

                try
                {
                    foreach (var file in UploadFiles)
                    {

                        var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        // filename = hostingEnv.WebRootPath + $@"\{filename}";
                        if (!System.IO.File.Exists(filename))
                        {
                            Directory.CreateDirectory(folderPath);
                            using (var fs = System.IO.File.Create($"{folderPath}\\{filename}"))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }

                            // Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.StatusCode = 200;
                            //  Response.
                            var content = new { id = filename, fileContextId = meta != null ? meta.FileContextId : null, sha256 = "23498SAFDAASFASG" };
                            //   return Content(Guid.NewGuid().ToString());
                            return new JsonResult(content);
                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 204;

                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                        }
                    }
                }
                catch (Exception e)
                {
                    Response.Clear();
                    Response.ContentType = "application/json; charset=utf-8";
                    Response.StatusCode = 204;
                    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "No Content";
                    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                }
            }


            return Content("");
        }


        [Route("RemoveWithMetaData/{chunkBucket}")]
        [HttpPost]
        public IActionResult RemoveFileWithMetaData([FromForm] string metadata, CancellationToken cancellationToken)
        {
            try
            {
                RemoveMetadata meta = null;
                if (!string.IsNullOrEmpty(metadata))
                {
                    meta = JsonConvert.DeserializeObject<RemoveMetadata>(metadata);
                }


                var filePath = $"{folderPath}\\{meta.Id}";
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

public class Metadata
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string FileContextId { get; set; }
    public string ParameterPath { get; set; }
    public string ParameterKey { get; set; }
}

public class RemoveMetadata
{
    public string FileContextId { get; set; }
    public string Id { get; set; }
    public string Sha256 { get; set; }
}
