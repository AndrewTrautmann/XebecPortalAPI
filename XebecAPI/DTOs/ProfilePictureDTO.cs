using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecAPI.DTOs
{
    public class ProfilePictureDTO
    {
        public int Id { get; set; }
        public string profilePic { get; set; }
        public int AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }
    }
}
