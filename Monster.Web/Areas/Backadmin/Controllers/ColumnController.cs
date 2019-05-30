using System.Web.Mvc;
using Monster.Data;

namespace Monster.Web.Areas.Backadmin.Controllers
{
    public class ColumnController : EntityController<Column>
    {
        public ActionResult GetTree()
        {
            var query = Context.Queryable<Column>().OrderBy(n => n.SortCode).ToList();
            return Success(query, query.Count);
        }
        public override ActionResult Add()
        {
            return View();
        }
    }
}