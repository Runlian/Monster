using System;
using System.IO;
using System.Text;

namespace Monster.Common
{
    public class CommonHelper
    {
        /// <summary>
        /// 随机产生常用汉字
        /// </summary>
        /// <param name="count">要产生汉字的个数</param>
        /// <returns>常用汉字</returns>
        public static string GenerateChineseWord(int count)
        {
            string chineseWords = "";
            Random rm = new Random(Guid.NewGuid().GetHashCode());
            Encoding gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)
                int regionCode = rm.Next(16, 56);

                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)
                var positionCode = rm.Next(1, regionCode == 55 ? 90 : 95);

                // 转换区位码为机内码
                int regionCodeMachine = regionCode + 160;// 160即为十六进制的20H+80H=A0H
                int positionCodeMachine = positionCode + 160;// 160即为十六进制的20H+80H=A0H

                // 转换为汉字
                byte[] bytes = new byte[] { (byte)regionCodeMachine, (byte)positionCodeMachine };
                chineseWords += gb.GetString(bytes);
            }
            return chineseWords;
        }

        /// <summary>
        /// 随便生成
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GenerateNumber(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }

        /// <summary>
        /// 随便生成手机号码
        /// </summary>
        /// <returns></returns>
        public static string GenerateMobile()
        {
            string[] tels = "134,135,136,137,138,139,150,151,152,157,158,159,130,131,132,155,156,133,153,180,181,182,183,185,186,176,187,188,189,177,178".Split(',');
            Random r = new Random(Guid.NewGuid().GetHashCode());
            int index = r.Next(0, tels.Length - 1);
            string first = tels[index];
            string second = (r.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (r.Next(1, 9100) + 10000).ToString().Substring(1);
            return first + second + thrid;
        }

        //读取json数据
        public static string GetFileJson(string filepath)
          {
              string json = string.Empty;
              using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
              {
                  using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                  {
                     json = sr.ReadToEnd().ToString();
                  }
             }
             return json;
       }
    }


}