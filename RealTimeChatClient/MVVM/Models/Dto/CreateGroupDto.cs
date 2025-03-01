using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.MVVM.Models.Dto
{
    class CreateGroupDto
    {
        public string GroupName { get; set; }
        public bool IsDm { get; set; }
    }
}
