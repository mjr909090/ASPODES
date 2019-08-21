using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ASPODES.WebAPI.Security
{

    public class UserPrincipal:GenericPrincipal
    {
        public CurrentUser UserInfo { get; set; }

        public UserPrincipal(IIdentity identity, string[] roles ):base( identity,  roles)
        { 
        }
    }
}