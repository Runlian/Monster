using System.Collections.Generic;
using Monster.Common;
using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TreeModel> Modules { get; set; }

        /// <summary>
        /// 重写Create方法
        /// </summary>
        public override void Create()
        {
            IsEnabled = true;
            IsDeleted = false;
            base.Create();
        }
    }
}