using System.Runtime.InteropServices;
using System.Text;

namespace Monster.Common
{
    public class IDR210Helper
    {
        [DllImport("Sdtapi.dll")]
        public static extern int InitComm(int iPort);

        [DllImport("Sdtapi.dll")]
        public static extern int CloseComm();

          [DllImport("Sdtapi.dll")]
        public static extern int CardOn();

        [DllImport("Sdtapi.dll")]
        public static extern int Authenticate();

        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, string photoPath);
    }
}