using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Maurice.Core.Services
{
    public interface IFileService
    {
        public Task<IDictionary<string, string>> ProcessXmlFileAsync(StorageFile file);
    }
}
