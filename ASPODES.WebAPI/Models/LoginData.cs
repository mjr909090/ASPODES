using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Models
{
    public class LoginData
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int PersonId { get; set; }
        public double UserTimeStamp { get; set; }
        public string Token { get; set; }
        public string[] Roles { get; set; }
    }
}
