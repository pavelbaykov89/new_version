using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace SLK.Services.FileStorage
{
    public interface IFilesRepository
    {
        /// <summary>
        /// Строит ссылку на файл
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        string BuildUrl(string id);

        /// <summary>
        /// Извлекает идентификатор файла из ссылки
        /// </summary>
        /// <param name="url">Ссылка на файл</param>
        /// <returns>Идентификатор файла</returns>
        string ExtractID(string url);

        /// <summary>
        /// Создает новый файл
        /// </summary>
        /// <returns>Идентификатор файла</returns>
        string Create(Stream fileStream, string name, string[] allowedExtensions = null);

        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        void Remove(string id);
    }

    class FilesRepository : IFilesRepository
    {
        readonly string FilesFolderPath;
        readonly string FilesStorageBaseUrl;

        public FilesRepository()
        {
            FilesFolderPath = ConfigurationManager.AppSettings["FilesFolderPath"];
            //this can be useful if you want CDN or something like
            FilesStorageBaseUrl = ConfigurationManager.AppSettings["FilesStorageBaseUrl"] ?? "";
        }

        public string BuildUrl(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            return FilesStorageBaseUrl + BuildVirtualPath(id);
        }

        public string ExtractID(string url)
        {
            //в текущей реализации идентификатором файла является название физического файла без учета папки
            return VirtualPathUtility.GetFileName(url.Replace(FilesStorageBaseUrl, string.Empty));
        }

        public string Create(Stream stream, string name, string[] allowedExtensions = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            if (name.Contains("__"))
                throw new ArgumentException("File name can't contains \"__\"", name);

            if (name.Contains(" "))
                name = name.Replace(" ", "-");

            if (!Regex.IsMatch(name, @"^[a-z0-9\-_\.]+$", RegexOptions.IgnoreCase))
                throw new ArgumentException("File name contains disallowed symbols", name);

            var extension = Path.GetExtension(name);

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException("File has no extension", name);

            if (allowedExtensions != null)
            {
                var extensionCheck = extension.Replace(".", "").ToLowerInvariant();

                if (!allowedExtensions.Contains(extensionCheck))
                    throw new ArgumentException("File with this extension not allowed", name);
            }

            string id = null;
            string physicalPath = null;
            do
            {
                var candidateID = string.Format("{0}__{1}", Guid.NewGuid(), name);
                physicalPath = GetPhysicalPath(candidateID);
                if (!File.Exists(physicalPath))
                    id = candidateID;
            }
            while (id == null);

            var directoryPath = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var mStream = new MemoryStream())
            using (var fStream = new FileStream(physicalPath, FileMode.CreateNew, FileAccess.Write))
            {
                stream.Position = 0;
                stream.CopyTo(mStream);

                var bytes = mStream.ToArray();
                fStream.Write(bytes, 0, bytes.Length);

                stream.Dispose();
            }

            return id;
        }

        public void Remove(string id)
        {
            var physicalPath = GetPhysicalPath(id);
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);
        }

        private string BuildVirtualPath(string id)
        {
            return VirtualPathUtility.Combine(FilesFolderPath, id);
        }

        private string GetPhysicalPath(string id)
        {
            return HostingEnvironment.MapPath(BuildVirtualPath(id));
        }
    }
}
