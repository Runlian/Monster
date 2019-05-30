using System.Web.Mvc;
using Monster.Data;
using SqlSugar;

namespace Monster.Web
{
    [HandlerLogin]
    [HandlerAuthorize]
    public class EntityController<T> : BaseController where T : BaseEntity, new()
    {
        [HttpGet]
        [HandlerAuthorize(false)]
        public virtual ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize(false)]
        public virtual ActionResult Edit(string id)
        {
            var entity = Context.Queryable<T>().InSingle(id);
            return View(entity);
        }

        [HandlerAuthorize(false)]
        public virtual ActionResult Get(string id)
        {
            var result = Context.Queryable<T>().InSingle(id);
            return Success(result, 1);
        }

        [HandlerAuthorize(false)]
        public virtual ActionResult List(string keyword, Pagination pagination, string baseId = "")
        {
            var predicate = Expressionable.Create<T>().AndIF(!string.IsNullOrEmpty(keyword),
                   n => n.Name.Contains(keyword)).ToExpression();
            var count = 0;
            var result = Context.Queryable<T>().Where(predicate).OrderBy(n => n.CreateTime, OrderByType.Desc).ToPageList(pagination.page, pagination.limit, ref count);
            return Success(result, count);
        }

        [HandlerAuthorize(false)]
        public virtual ActionResult GetSelect(string baseId = "")
        {
            var result = Context.Queryable<T>().OrderBy(n => n.CreateTime, OrderByType.Desc).Select(n => new { n.Id, n.Name }).ToList();
            return Success(result, result.Count);
        }

        [HttpPost]
        [HandlerAuthorize(false)]
        public virtual ActionResult Add(T entity)
        {
            entity.Create();
            var result = Context.Insertable(entity).ExecuteCommand() > 0;
            return result ? Success("添加成功") : Error("添加失败");
        }

        [HttpPost]
        [HandlerAuthorize(false)]
        public virtual ActionResult Edit(T entity)
        {
            var result = Context.Updateable(entity).IgnoreColumns(n => n.CreateTime).ExecuteCommand() > 0;
            return result ? Success("更新成功") : Error("更新失败");
        }

        [HandlerAuthorize(false)]
        public virtual ActionResult Delete(string id)
        {
            var result = Context.Deleteable<T>().Where(n => n.Id.Equals(id)).ExecuteCommand() > 0;
            return result ? Success("删除成功") : Error("删除失败");
        }
    }
}