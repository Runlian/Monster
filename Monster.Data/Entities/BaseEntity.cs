using System;
using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class BaseEntity : ModelContext
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsNullable = false, Length = 50)]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 150)]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 初始化方法
        /// </summary>
        public virtual void Create()
        {
            Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }
    }
}