using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Tools
{
    public class PropertyComparer<T1,T2> : IEqualityComparer<T1> 
    {
        Func<T1, T2> selector;

        public PropertyComparer(Func<T1, T2> selector)
        {
            this.selector = selector;
        }
        public bool Equals(T1 x, T1 y)
        {
            return selector(x).Equals(selector(y));
        }
        public int GetHashCode(T1 obj)
        {
            return obj.GetHashCode();
        }
    }
}