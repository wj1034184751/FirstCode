using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Web_Helper
{
    public class OperateContext
    {
        private IBLLSession bLLSession;

        public IBLLSession BLLSession
        {
            get
            {
                return bLLSession;
            }
        }

        public OperateContext()
        {
            bLLSession = DI.StringHelper.GetObject<IBLLSession>("BLLSession");
        }
        public static OperateContext Current
        {
            get
            {
                OperateContext Context = CallContext.GetData(typeof(OperateContext).Name) as OperateContext;
                if (Context == null)
                {
                    Context = new OperateContext();
                    CallContext.SetData(typeof(OperateContext).Name, Context);
                }
                return Context;
            }
        }
    }
}
