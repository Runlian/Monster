using System;
using System.Collections.Generic;
using Monster.Common;

namespace Monster.Data
{
    public class DbInit
    {
        public static void Init()
        {
            InitTables();
            InitData();
        }

        public static void InitDatabase()
        {
            using (var db = DbConfig.GetDbInstance())
            {
                db.DbMaintenance.CreateDatabase("Monster");
            }
        }

        public static void InitTables()
        {
            using (var db = DbConfig.GetDbInstance())
            {
                db.CodeFirst.InitTables(typeof(User), typeof(Role), typeof(UserRole), typeof(RoleAuthorize), typeof(Module), typeof(Column), typeof(Article));
            }
        }

        public static void InitData()
        {
            var r1 = new Role() { Id = Guid.NewGuid().ToString(), Name = "管理员", IsEnabled = true, CreateTime = DateTime.Now };

            var u1 = new User() { Id = Guid.NewGuid().ToString(), Name = "管理员", Account = "admin", Password = DESEncrypt.DoubleEncrypt("123456"), IsEnabled = true, IsAdmin = true, CreateTime = DateTime.Now };

            var ur1 = new UserRole() { Id = Guid.NewGuid().ToString(), UserId = u1.Id, RoleId = r1.Id, CreateTime = DateTime.Now };

            var m99 = new Module() { ParentId = "0", Name = "系统管理", Icon = "layui-icon-set", IsMenu = true, SortCode = "99" };


            var m9901 = new Module() { ParentId = m99.Id, Name = "角色管理", Url = "/Backadmin/Role/Index", IsMenu = true, SortCode = "9901" };
            var m9902 = new Module() { ParentId = m99.Id, Name = "用户管理", Url = "/Backadmin/User/Index", IsMenu = true, SortCode = "9902" };




            var rm99 = new RoleAuthorize() { Id = Guid.NewGuid().ToString(), RoleId = r1.Id, ObjectId = m99.Id, Type = ModuleType.菜单, CreateTime = DateTime.Now };
            var rm9901 = new RoleAuthorize() { Id = Guid.NewGuid().ToString(), RoleId = r1.Id, ObjectId = m9901.Id, Type = ModuleType.菜单, CreateTime = DateTime.Now };
            var rm9902 = new RoleAuthorize() { Id = Guid.NewGuid().ToString(), RoleId = r1.Id, ObjectId = m9902.Id, Type = ModuleType.菜单, CreateTime = DateTime.Now };



            var roles = new List<Role>() { r1 };
            var users = new List<User>() { u1 };
            var urs = new List<UserRole>() { ur1 };
            var modules = new List<Module> { m99, m9901, m9902 };
            var rms = new List<RoleAuthorize>() { rm99, rm9901, rm9902 };
            using (var context = DbConfig.GetDbInstance())
            {
                context.Insertable(roles).ExecuteCommand();
                context.Insertable(users).ExecuteCommand();
                context.Insertable(urs).ExecuteCommand();
                context.Insertable(modules).ExecuteCommand();
                context.Insertable(rms).ExecuteCommand();
            }
        }
    }
}
