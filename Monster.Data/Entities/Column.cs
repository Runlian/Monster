using System.Collections.Generic;
using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 栏目
    /// </summary>
    public class Column : BaseEntity
    {
        /// <summary>
        /// 上级Id
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string ParentId { get; set; } = "";

        /// <summary>
        /// 栏目类型
        /// </summary>
        public ColumnType Type { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 500)]
        public string Link { get; set; }

        /// <summary>
        /// 是否在导航显示
        /// </summary>
        public bool ShowNav { get; set; }

        /// <summary>
        /// 是否需要认证
        /// </summary>
        public bool IsAuth { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string SortCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 上级栏目
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Column Parent { get; set; }

        /// <summary>
        /// 子栏目
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<Column> Childrens { get; set; }

        /// <summary>
        /// 文章列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<Article> Articles { get; set; }
    }
}