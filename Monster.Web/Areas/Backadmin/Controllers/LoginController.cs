using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Monster.Common;
using Monster.Data;
using SqlSugar;

namespace Monster.Web.Areas.Backadmin.Controllers
{
    public class LoginController : Controller
    {
        public SqlSugarClient Context;
        public LoginController()
        {
            Context = DbConfig.GetDbInstance();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">登录名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            var user = Context.Queryable<User>().Single(n => n.Account.Equals(account));
            if (user == null)
                throw new Exception("用户不存在");
            if (!user.IsEnabled)
                throw new Exception("用户已被禁用");

            var dbPassword = DESEncrypt.DoubleEncrypt(password);
            if (user.Password != dbPassword)
                throw new Exception("密码不正确");

            var roleIds = Context.Queryable<UserRole>().Where(n => n.UserId.Equals(user.Id)).Select(n => n.RoleId).ToList();
            var tempData = new List<Module>();
            if (user.IsAdmin)
            {
                tempData = Context.Queryable<Module>().ToList();
            }
            else
            {
                var query = Context.Queryable<Module>().ToList();
                var authData = Context.Queryable<RoleAuthorize>().Where(n => roleIds.Contains(n.RoleId) && n.Type == ModuleType.菜单).ToList();
                tempData.AddRange(authData.Select(item => query.Find(n => n.Id.Equals(item.ObjectId))).Where(module => module != null));
            }

            var modules = tempData.Distinct(new EntityComparer<Module>("Id")).OrderBy(n => n.SortCode).ToList();

            var operatorModel = new OperatorModel()
            {
                UserId = user.Id,
                UserName = user.Name,
                Account = user.Account,
                IsAdmin = user.IsAdmin,
                Modules = modules
            };
            OperatorProvider.Provider.AddCurrent(operatorModel);
            return Content(new ReturnData<string>(CustomStatusCode.Success, "登录成功").ToJson());
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult OutLogin()
        {
            Session.Abandon();
            Session.Clear();
            OperatorProvider.Provider.RemoveCurrent();
            return Content(new ReturnData<string>(CustomStatusCode.Success, "退出成功").ToJson());
        }
    }
}