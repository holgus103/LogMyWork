using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Commons.Web
{
    public class KeyValuePairSelectList : SelectList
    {
        public KeyValuePairSelectList(IEnumerable<KeyValuePair<object, string>> model) : base(model, "Key", "Value")
        {

        }

        public KeyValuePairSelectList(IEnumerable<KeyValuePair<object, string>> model, object selectedValue) : base(model, "Key", "Value", selectedValue)
        {

        }
    }
}
