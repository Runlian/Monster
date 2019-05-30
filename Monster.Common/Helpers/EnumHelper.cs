using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Monster.Common
{
    public class EnumUtil
    {
        public static List<EnumModel> GetList<T>()
        {
            var data = new List<EnumModel>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var model = new EnumModel();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    model.Description = da?.Description;
                }
                model.Value = e.GetHashCode();
                model.Key = e.ToString();
                data.Add(model);
            }
            return data;
        }
    }
}