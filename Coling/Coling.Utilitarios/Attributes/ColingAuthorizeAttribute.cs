using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Utilitarios.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ColingAuthorizeAttribute : Attribute
    {
        public string Rols { get; set; }
        public ColingAuthorizeAttribute(string rols)
        {
            this.Rols = rols;
        }
    }
}