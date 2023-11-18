using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using Json.Schema;
using Microsoft.AspNetCore.Mvc;
namespace FileUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaValidationController
    {


        public SchemaValidationController()
        {
            var files = Directory.GetFiles(@"C:\source\control_set\applications\old-tester\static\futures\jsonSchemas", "*.schema.json");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var schema = JsonSchema.FromFile(file);
                SchemaRegistry.Global.Register(new Uri($"C:/source/control_set/applications/old-tester/static/futures/jsonSchemas/{fileName}"), schema);
            }
        }

        [HttpGet]
        public ActionResult<bool> Get()
        {
            //var _schema = SchemaRegistry.Global.Get(new System.Uri("http://localhost/root.schema.json"));


            //var schema = new JsonSchemaBuilder().Ref(new System.Uri("http://localhost/root.schema.json"))
            //.Build();

            var schema = JsonSchema.FromFile(@"C:\source\control_set\applications\old-tester\static\futures\jsonSchemas\root.schema.json");

            var doc = JsonNode.Parse(File.ReadAllText(@"C:\source\control_set\applications\old-tester\static\futures\jsonSchemas\root.schema.json"));

            var result = schema.Evaluate(doc);
            return false;
        }
    }
}

