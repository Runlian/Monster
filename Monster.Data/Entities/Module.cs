using System;
using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 系统模块
    /// </summary>
    public class Module : BaseEntity
    {
        /// <summary>
        /// 上级Id
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string ParentId { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 150)]
        public string Icon { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 1000)]
        public string Url { get; set; }

        /// <summary>
        /// 跳转目标
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string Target { get; set; }

        /// <summary>
        /// 是否菜单
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool IsMenu { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 150)]
        public string SortCode { get; set; }


        public Module()
        {
            Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }
    }
}