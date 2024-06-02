using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailPreparationMachine
{
    public interface ILoader
    {
        void PickUpDetail(Detail detail);
    }
}
