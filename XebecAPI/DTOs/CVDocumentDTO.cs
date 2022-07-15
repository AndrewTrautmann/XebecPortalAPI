using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XebecAPI.Shared.Security;

namespace XebecAPI.DTOs
{
    public class CVDocumentDTO
    {
        public int Id { get; set; }

        public string documentName { get; set; }

        public string documentBlobLink { get; set; }

        public int AppUserId { get; set; }

        public AppUserDTO AppUser { get; set; }
    }
}
