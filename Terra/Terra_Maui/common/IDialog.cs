using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terra_Maui.common
{
    public interface IDialog
    {

        string getValue();
        void setValue(string val);
        string getTitle();
        Keyboard getkeyBoardType();
    }
}
