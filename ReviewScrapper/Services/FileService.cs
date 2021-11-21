using ReviewScrapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Services
{
    public class FileService : IFileService
    {
        public string GetStringFromFile(string filePath)
        {
            try
            {
                return System.IO.File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
