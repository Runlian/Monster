using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Monster.Common
{
    public class ImageHelper
    {
        /// <summary>
        /// 根据base64字符串返回一个封装好的GDI+位图。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Bitmap GetImageFromBase64(string data)
        {
            byte[] b = Convert.FromBase64String(data);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }
    }
}