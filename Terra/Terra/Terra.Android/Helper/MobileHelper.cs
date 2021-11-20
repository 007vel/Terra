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
        public void TerminateApp()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
        public List<string> GetAllAssetsName()
        {
            AssetManager assets = Android.App.Application.Context.Assets;
            var fileslist = assets.List(""); //ota_data_initial_1.2.46_
            return new List<string>(fileslist ==null || fileslist.Length==0? new string[1] { "ota_data_initial_0.0.0" }: fileslist);
        }

        public byte[] ReadOtaFile()
        {
            AssetManager assets = Android.App.Application.Context.Assets;
            var files = GetAllAssetsName(); //ota_data_initial_1.2.46_
            string fileName = default;
            foreach(var f in files)
            {
                if(f.Contains("ota_data_initial"))
                {
                    fileName = f;
                    break;
                }
            }
            return fileName!=default? GetImageStreamAsBytes(assets.Open(fileName)) : null;
        }
        private byte[] GetImageStreamAsBytes(System.IO.Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
