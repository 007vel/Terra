using System;
using System.Collections.Generic;

namespace ConnectionLibrary.Interface
{
    public interface IMobile
    {
        void Log(string msg);
        string GetVersion();
        string GetBuildNumber();
        void TerminateApp();
        byte[] ReadOtaFile();
        List<string> GetAllAssetsName();
    }
}
