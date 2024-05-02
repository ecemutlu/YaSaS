using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrete
{
    public class User:IdentityUser<int>
    {   
        public int Id { get; set; }
        public Building? Building { get; set; }
    }
}
