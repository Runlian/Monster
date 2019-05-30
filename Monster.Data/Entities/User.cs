using System.Collections.Generic;
using Monster.Common;
using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 帐号表
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 50, ColumnDescription = "登录名")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 150)]
        public string Password { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 仅用于接收表单提交
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string[] RoleId { get; set; }

        /// <summary>
        /// 仅用于返回视图模型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TreeModel> Roles { get; set; }

        public override void Create()
        {
            IsEnabled = true;
            base.Create();
        }

    }
    
}