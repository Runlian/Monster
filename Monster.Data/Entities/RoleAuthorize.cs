using SqlSugar;

namespace Monster.Data
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class RoleAuthorize : BaseEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 50)]
        public string RoleId { get; set; }

        /// <summary>
        /// 模块类型
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public ModuleType Type { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 50)]
        public string ObjectId { get; set; }
    }
}