using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Shared.Security;

namespace XebecAPI.Shared
{
    public class ProfilePicture
    {
        public int Id { get; set; }
        public string profilePic { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
