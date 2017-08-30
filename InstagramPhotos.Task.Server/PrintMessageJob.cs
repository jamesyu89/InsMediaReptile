using System;
using Quartz;
using System.Net;
using System.IO;


namespace InstagramPhotos.Task.Server
{
    public class PrintMessageJob : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            // 设置参数
            var url = "https://www.instagram.com/therock";
            var path = Environment.CurrentDirectory + $"/therock.html";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }
    }
}