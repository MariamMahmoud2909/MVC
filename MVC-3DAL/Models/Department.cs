using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3DAL.Models
{
    public class Department :ModelBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public string Description { get; set; }

        // Navigational Property 
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
