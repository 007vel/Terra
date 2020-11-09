﻿using System;
namespace ConnectionLibrary.Interface
{
    public interface IMobile
    {
        void Log(string msg);
        string GetVersion();
        string GetBuildNumber();
    }
}
