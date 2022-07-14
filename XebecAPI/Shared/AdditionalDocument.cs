using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XebecAPI.Shared.Security;

namespace XebecAPI.Shared
{
    public class AdditionalDocument
    {
        public int Id { get; set; }

        public string documentName { get; set; }

        public string documentBlobLink { get; set; }

        //Foreign Key: AppUser
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
