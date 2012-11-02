﻿using System;
using System.IO;

namespace apid.web.Areas.Admin.FileHandler
{
    public class FilesStatus
    {
        public const string HandlerPath = "/Areas/SuperUser/FileHandler/";

        public string group { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string error { get; set; }

        public string GetFileAccessPath(string fileName)
        {
            return HandlerPath + "Handler.ashx?f=" + fileName;
        }

        public string GetFileHandlerPath()
        {
            return HandlerPath + "Handler.ashx";
        }

        public FilesStatus() { }

        public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, (int)fileInfo.Length, fileInfo.FullName, false); }

        public FilesStatus(string fileName, int fileLength, string fullPath, bool webPath) { SetValues(fileName, fileLength, fullPath, webPath); }

        private void SetValues(string fileName, int fileLength, string fullPath, bool webPath)
        {
            name = fileName;
            type = "image/png";
            size = fileLength;
            progress = "1.0";
            if (webPath) url = fullPath; else url = HandlerPath + "Handler.ashx?f=" + fileName;
            delete_url = HandlerPath + "Handler.ashx?f=" + fileName;
            delete_type = "DELETE";

            var ext = Path.GetExtension(fullPath);
            
            //var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
            //if (fileSize > 3 || !IsImage(ext)) 
            thumbnail_url = "/Areas/SuperUser/Content/images/generalFile.png";
            //else thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath);
        }

        private bool IsImage(string ext)
        {
            return ext == ".gif" || ext == ".jpg" || ext == ".png";
        }

        private static string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(File.ReadAllBytes(fileName));
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}