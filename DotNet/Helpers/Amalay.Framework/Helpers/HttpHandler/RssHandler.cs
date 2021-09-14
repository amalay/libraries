using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Amalay.Framework
{
    //public class RssHandler : IHttpHandler
    //{
    //    private string fileName = "RssHandler.cs";       

    //    public bool IsReusable
    //    {            
    //        get 
    //        { 
    //            return false; 
    //        }
    //    }

    //    public void ProcessRequest(HttpContext context)
    //    {
    //        context.Response.ContentType = "text/xml";

    //        using (XmlWriter xmlWriter = XmlWriter.Create(context.Response.OutputStream))
    //        {
    //            xmlWriter.WriteStartDocument();
    //            xmlWriter.WriteElementString("Rss", "Amalay Http Handler RSS Feed!");
    //            xmlWriter.WriteEndDocument();
    //            xmlWriter.Flush();
    //        }
    //    }
    //}
}
