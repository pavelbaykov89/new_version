using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Infrastructure
{
    public class FileAutoDeleteResult : FileResult
    {
        protected string _fileName;

        protected string _contentType;

        public FileAutoDeleteResult(string fileName, string contentType) : base(contentType)
        {
            _fileName = fileName;

            _contentType = contentType;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.ContentType = _contentType;
            response.WriteFile(_fileName);
            response.Flush();
            File.Delete(_fileName);
        }
    }
}