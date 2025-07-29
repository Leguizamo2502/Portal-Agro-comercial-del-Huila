using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Entity.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int PersonId { get; set; }
    }
}
