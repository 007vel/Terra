using System;
using ConnectionLibrary.Interface;
using Terra.Droid.Helper;
using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Provider;
using Android.Util;
using Java.IO;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Globalization;

[assembly: Xamarin.Forms.Dependency(typeof(MobileHelper))]
namespace Terra.Droid.Helper
{
    public class MobileHelper : IMobile
    {
        File logFile = null;
        public string GetBuildNumber()
        {
            PackageInfo packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0);
            return packageInfo.VersionCode.ToString();
        }

        public string GetVersion()
        {
            PackageInfo packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0);
            return packageInfo.VersionName;
        }

        public void Log(string logMessage)
        {
            
            if(logFile==null)
            {
                logFile = GetLogFile();
            }
            FileWriter fileWriter = new FileWriter(logFile, true);
            BufferedWriter bufferedWriter = new BufferedWriter(fileWriter);
            DateTime currentTime = new DateTime(DateTime.Now.Ticks);
            string currentlogtime = DateTime.Now.ToString()+ " "+GetVersion();
            bufferedWriter.Append(currentlogtime + ": " + logMessage);
            bufferedWriter.NewLine();
            bufferedWriter.Flush();
            bufferedWriter.Close();
        }
        public File GetLogFile()
        {

            File path = Android.App.Application.Context.GetExternalFilesDir(null);
            File appPath = new File(path + "/Log");
            appPath.Mkdirs();
            string fileName = null;
            DateTime date = new DateTime(DateTime.Now.Ticks);
            string dateAsString = DateTime.Now.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
            fileName = dateAsString;

            File logFile = new File(appPath, fileName + ".txt");
            if (!logFile.Exists())
            {
                logFile.CreateNewFile();
            }

            return logFile;

        }
    }
}
