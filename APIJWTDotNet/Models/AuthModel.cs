using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJWTDotNet.Models
{
    public class AuthModel
    {
        public String Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public List<string> Roles{ get; set; }
        public String Token { get; set; }
        public DateTime Exipred { get; set; }
    }
}
