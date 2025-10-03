using Maurice.Data.Models;
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
        public Task<Comprobante> ProcessXmlFileAsync(StorageFile file);
    }
}
