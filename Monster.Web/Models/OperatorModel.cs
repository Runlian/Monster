using System.Collections.Generic;
using Monster.Data;

namespace Monster.Web
{
    public class OperatorModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Account { get; set; }
        public bool IsAdmin { get; set; }
        public int LoginEnum { get; set; }
        public bool JurisdictionStatus { get; set; }
        public List<Module> Modules { get; set; }

    }
}