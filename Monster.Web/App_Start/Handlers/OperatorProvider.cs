using Monster.Common;

namespace Monster.Web
{
    public class OperatorProvider
    {
        public static OperatorProvider Provider => new OperatorProvider();
        private string LoginUserKey = "MonsterUserKey";
        public OperatorModel GetCurrent()
        {
            return DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey)).ToObject<OperatorModel>();
        }
        public void AddCurrent(OperatorModel operatorModel)
        {
            WebHelper.WriteSession(LoginUserKey, DESEncrypt.Encrypt(operatorModel.ToJson()));
        }
        public void RemoveCurrent()
        {
            WebHelper.RemoveSession(LoginUserKey.Trim());
        }
    }
}