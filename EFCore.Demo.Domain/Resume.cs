using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Demo.Domain
{
    public class Resume
    {
        public int Id { get; set; }
        public string  Description { get; set; }

        public Player Player { get; set; }
        public int PlayerId { get; set; }

    }
}
