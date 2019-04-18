using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI
{
    public class StringHelper
    {
        /// <summary>
        /// 使用String创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T GetObject<T>(string objName) where T : class
        {
            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            //IBLL.IWeb_ContentBLL dll = ctx.GetObject("BLL") as IBLL.IWeb_ContentBLL;
            T t = (T)ctx.GetObject(objName);
            return t;
        }
    }
}
