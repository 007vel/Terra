using System;
using System.Collections.Generic;
using Entities;
using Xamarin.Forms;

namespace Terra.Core.common
{
    public interface IDialog
    {
        
        string getValue();
        void setValue(string val);
        string getTitle();
        Keyboard getkeyBoardType();
    }
}
