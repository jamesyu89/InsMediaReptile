using System;
using System.IO;

namespace InstagramPhotos.Utility.Log
{
    public class TxtRecorder
    {
        private static string baseDirectory = "txtRecord";

        public static void Write(string directoryName, string fileName, string recordLog)
        {
            string fileBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string path = fileBase + "/" + baseDirectory + "/" + directoryName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = path + "/" + fileName + ".txt";
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(recordLog);
                }
            }
        }

        //public static void WriteJingDongAsynPayResultRecordLog(string recordLog)
        //{
        //    Write("JingDongAsynPayResult", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), recordLog);
        //}

        //public static void WriteJingDongQueryResultRecordLog(string recordLog)
        //{
        //    Write("JingDongQueryResult", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), recordLog);
        //}
    }
}