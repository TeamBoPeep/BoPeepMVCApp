using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoPeepMVC.Models.Interfaces
{
    public interface IActivityManager
    {
        public Task<string> GetHello();
    }
}
