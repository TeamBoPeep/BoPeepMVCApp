using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoPeepMVC.Models.Interfaces
{
    public interface ITagManager
    {
        public Task<IEnumerable<Tag>> GetTags();
    }
}
