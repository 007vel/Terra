using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terra_Maui
{
    public interface ISmartConfigHelper
    {
        ISmartConfigTask CreatePlatformTask();
    }
}
