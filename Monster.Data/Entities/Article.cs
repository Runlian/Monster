using SqlSugar;
using System;

namespace Monster.Data
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : BaseEntity
    {
        /// <summary>
        /// 关联栏目Id
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string ColumnId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 250)]
        public string Title { get; set; }

        /// <summary>
        /// 文章副标题
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 250)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 150)]
        public string Source { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 150)]
        public string Author { get; set; }

        /// <summary>
        /// 文章网址
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 250)]
        public string Website { get; set; }

        /// <summary>
        /// 文章摘要
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 1000)]
        public string Summary { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 100000)]
        public string Content { get; set; }

        /// <summary>
        /// 文章图片
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 10000)]
        public string Images { get; set; }

        /// <summary>
        /// 文章附件
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 1000)]
        public string Attachment { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 是否热帖
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool IsHot { get; set; }

        /// <summary>
        /// 是否醒目显示
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool IsColor { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Hits { get; set; }

        /// <summary>
        /// 最后点击时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime LastHitTime { get; set; }

        /// <summary>
        /// 发文人Id
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string UserId { get; set; }

        /// <summary>
        /// 发文人姓名
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string UserName { get; set; }

        /// <summary>
        /// 最后编辑人Id
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string EditorId { get; set; }

        /// <summary>
        /// 最后编辑人姓名
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string EditorName { get; set; }

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 搜索引擎-标题
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 250)]
        public string SeoTitle { get; set; }

        /// <summary>
        /// 搜索引擎-关键字
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 250)]
        public string SeoKeywords { get; set; }

        /// <summary>
        /// 搜索引擎-说明
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 500)]
        public string SeoDescription { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool IsDisplay { get; set; }
    }
}