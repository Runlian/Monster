using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Monster.Common;
using Monster.Data;
using SqlSugar;

namespace Monster.Web.Areas.Backadmin.Controllers
{
    public class UserController : EntityController<User>
    {
        [HttpGet]
        public override ActionResult Add()
        {
            var roles = Context.Queryable<Role>().Where(n => n.IsEnabled == true && n.IsDeleted == false).OrderBy(n => n.CreateTime).Select(n => new TreeModel()
            {
                Id = n.Id,
                Text = n.Name,
                Value = n.Id
            }).ToList();
            var entity = new User() { Roles = roles };

            return View(entity);
        }

        [HttpGet]
        public override ActionResult Edit(string id)
        {
            var roleIds = Context.Queryable<UserRole>().Where(n => n.UserId.Equals(id)).Select(n => n.RoleId).ToList();
            var roles = Context.Queryable<Role>().Where(n => n.IsEnabled == true && n.IsDeleted == false).OrderBy(n => n.CreateTime).ToList()
                .Select(n => new TreeModel()
                {
                    Id = n.Id,
                    Text = n.Name,
                    Value = n.Id,
                    Checked = roleIds.Contains(n.Id)
                }).ToList();
            var entity = Context.Queryable<User>().InSingle(id);
            entity.Roles = roles;
            return View(entity);
        }

        [HttpGet]
        public override ActionResult List(string keyword, Pagination pagination, string baseId = "")
        {
            var predicate = Expressionable.Create<User>().AndIF(!string.IsNullOrEmpty(keyword),
                    n => n.Name.Contains(keyword) || n.Account.Equals(keyword))
                .ToExpression();
            var count = 0;
            var result = Context.Queryable<User>().ToPageList(pagination.page, pagination.limit, ref count);
            return Success(result, result.Count);
        }

        [HttpGet]
        [HttpPost]
        public override ActionResult Add(User entity)
        {
            if (Context.Queryable<User>().Any(n => n.Account.Equals(entity.Account)))
                throw new Exception("帐号已存在");

            entity.Create();
            if (string.IsNullOrEmpty(entity.Password))
                entity.Password = "123@abc";
            entity.Password = DESEncrypt.DoubleEncrypt(entity.Password);

            var userRoles = new List<UserRole>();
            foreach (var roleId in entity.RoleId)
            {
                var item = new UserRole() { RoleId = roleId, UserId = entity.Id };
                item.Create();
                userRoles.Add(item);
            }

            var result = Context.Ado.UseTran(() =>
            {
                Context.Insertable(entity).ExecuteCommand();
                Context.Insertable(userRoles).ExecuteCommand();
            });

            return result.IsSuccess ? Success("添加成功") : Error("添加失败");
        }

        [HttpPost]
        public override ActionResult Edit(User entity)
        {
            if (Context.Queryable<User>().Any(n => n.Account.Equals(entity.Account) && n.Id != entity.Id))
                throw new Exception("帐号已存在");

            if (!string.IsNullOrEmpty(entity.Password))
                entity.Password = DESEncrypt.DoubleEncrypt(entity.Password);//加密密码

            var result = Context.Ado.UseTran(() =>
           {
               var sql = Context.Updateable(entity).UpdateColumns(n => new { n.Name, n.Account, n.IsEnabled, n.Password })
                .IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand();//为空字段不需要更新
               Context.Deleteable<UserRole>(n => n.UserId.Equals(entity.Id)).ExecuteCommand();

               if (entity.RoleId != null)
               {
                   var userRoles = new List<UserRole>();
                   foreach (var roleId in entity.RoleId)
                   {
                       var item = new UserRole() { RoleId = roleId, UserId = entity.Id };
                       item.Create();
                       userRoles.Add(item);
                   }
                   Context.Insertable(userRoles).ExecuteCommand();
               }

           });
            return result.IsSuccess ? Success("更新成功") : Error("更新失败");
        }

        [HttpPost]
        public ActionResult ResetPassword(string id)
        {
            var password = DESEncrypt.DoubleEncrypt("123@abc");
            var result = Context.Updateable<User>().UpdateColumns(n => n.Password == password)
                             .Where(n => n.Id.Equals(id)).ExecuteCommand() > 0;
            return result ? Success("密码重置成功") : Error("密码重置失败");
        }
    }
}