using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Interfaces
{
    public interface IFileService
    {
        public string GetStringFromFile(string filePath);
    }
}
