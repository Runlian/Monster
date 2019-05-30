using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRole : BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 50)]
        public string UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 50)]
        public string RoleId { get; set; }
    }
}