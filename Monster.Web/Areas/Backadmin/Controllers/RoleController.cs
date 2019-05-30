using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Monster.Common;
using Monster.Data;

namespace Monster.Web.Areas.Backadmin.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : EntityController<Role>
    {
        [HttpGet]
        public override ActionResult Add()
        {
            var query = Context.Queryable<Module>().OrderBy(n => n.SortCode)
                .Select(n => new TreeModel() { Id = n.Id, Text = n.Name, Value = n.Id, ParentId = n.ParentId, Checked = false }).ToList();
            var entity = new Role() { Modules = query };



            return View(entity);
        }

        [HttpPost]
        public override ActionResult Add(Role entity)
        {
            entity.Create();
            var result = Context.Insertable(entity).ExecuteCommand() > 0;
            return result ? Success("添加成功", entity, 1) : Error("添加失败");
        }

        [HttpGet]
        public override ActionResult Edit(string id)
        {
            var authorizes = Context.Queryable<RoleAuthorize>().Where(n => n.RoleId.Equals(id)).Select(n=>n.ObjectId).ToList();
            var modules = Context.Queryable<Module>().OrderBy(n => n.SortCode).ToList()
                .Select(n => new TreeModel()
                {
                    Id = n.Id,
                    Text = n.Name,
                    Value = n.Id,
                    ParentId = n.ParentId,
                    Checked = authorizes.Contains(n.Id)
                }).ToList();

            var entity = Context.Queryable<Role>().InSingle(id);
            entity.Modules = modules;
            return View(entity);
        }

        [HttpPost]
        public ActionResult Authorize(string roleId, string[] moduleId)
        {
            var result = Context.Ado.UseTran(() =>
                  {
                      Context.Deleteable<RoleAuthorize>(n => n.RoleId.Equals(roleId)).ExecuteCommand();

                      var entitys = new List<RoleAuthorize>();
                      foreach (var item in moduleId)
                      {
                          var entity = new RoleAuthorize()
                          {
                              RoleId = roleId,
                              ObjectId = item,
                              Type = ModuleType.菜单
                          };
                          entity.Create();
                          entitys.Add(entity);
                      }

                      Context.Insertable(entitys).ExecuteCommand();
                  });
            return result.IsSuccess ? Success("保存成功") : Error("保存失败");
        }
    }
}