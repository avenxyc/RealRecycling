using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iesi.Collections.Generic;

namespace Recycling.Domain.Models
{
    public class User : DbEntity
    {
        public virtual String Username { get; set; }
        public virtual String Password { get; set; }
        public virtual String Role { get; set; }
        public virtual String SecurityQuestion { get; set; }
        public virtual String SecurityAnswer { get; set; }
        public virtual string Email { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
    }
}