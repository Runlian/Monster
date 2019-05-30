namespace Monster.Web
{
    /// <summary>
    /// 统一返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReturnData<T>
    {
        /// <summary>
        /// 代码
        /// </summary>
        public readonly int code;
        /// <summary>
        /// 消息
        /// </summary>
        public readonly string msg;
        /// <summary>
        /// 数据集合
        /// </summary>
        public readonly T data;
        /// <summary>
        /// 总条数
        /// </summary>

        public readonly int count;

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code"></param>
        public ReturnData(CustomStatusCode code)
        {
            this.code = (int)code;
            this.msg = "";
            this.count = 0;
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public ReturnData(CustomStatusCode code, string msg)
        {
            this.code = (int)code;
            this.msg = msg;
            this.count = 0;
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ReturnData(CustomStatusCode code, string msg, T data)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.count = 0;
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        public ReturnData(CustomStatusCode code, string msg, int count, T data)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.count = count;
        }
    }
}