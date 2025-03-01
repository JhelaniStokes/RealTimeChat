using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.MVVM.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public bool IsDm { get; set; }
    }
}
