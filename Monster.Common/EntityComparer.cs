using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monster.Web
{
    /// <summary>
    /// 实体对比去重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityComparer<T> : IEqualityComparer<T> where T : class, new()
    {
        private readonly string[] _comparintFileName = { };
        public EntityComparer() { }

        public EntityComparer(params string[] comparintFileName)
        {
            _comparintFileName = comparintFileName;
        }
        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return false;
            }
            if (_comparintFileName.Length == 0)
            {
                return x.Equals(y);
            }
            bool result = true;
            var typeX = x.GetType();//获取类型
            var typeY = y.GetType();
            foreach (var filedName in _comparintFileName)
            {
                var xPropertyInfo = (from p in typeX.GetProperties() where p.Name.Equals(filedName) select p).FirstOrDefault();
                var yPropertyInfo = (from p in typeY.GetProperties() where p.Name.Equals(filedName) select p).FirstOrDefault();

                result = result
                    && xPropertyInfo != null && yPropertyInfo != null
                    && xPropertyInfo.GetValue(x, null).ToString().Equals(yPropertyInfo.GetValue(y, null));
            }
            return result;
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}