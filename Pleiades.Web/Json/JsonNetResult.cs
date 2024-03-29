﻿using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Pleiades.Web.Json
{
    public class JsonNetResult : ActionResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
            this.Formatting = Formatting.Indented;  // default setting 
        }

        public JsonNetResult(object data) : base()
        {
            this.Data = data;
        }

        public static JsonNetResult Success()
        {
            return new JsonNetResult(new { Success = true });
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}