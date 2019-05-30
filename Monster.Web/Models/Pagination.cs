namespace Monster.Web
{
    /// <summary>
    /// 翻页器
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页行数
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 排序方式 asc: 升序、desc: 降序、null: 默认排序
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int count { get; set; }

        public Pagination()
        {
            page = 1;
            limit = 20;
            field = "CreateTime";
            type = "desc";
        }
    }
}
