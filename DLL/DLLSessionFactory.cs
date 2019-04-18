using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class DLLSessionFactory :IDLLSessionFactory
    {
        #region 在线程中共用一个DBSession对象 + IDLL.IDLLSession GetDBSession()
        /// <summary>
        /// 提高效率，在线程中共用一个DBSession对象
        /// </summary>
        /// <returns></returns>
        public IDLLSession GetDBSession()
        {
            IDLLSession dbsession = CallContext.GetData(typeof(DLLSessionFactory).Name) as DLLDBSession;
            if (dbsession == null)
            {
                dbsession = new DLLDBSession();
                CallContext.SetData(typeof(DLLSessionFactory).Name, dbsession);
            }

            return dbsession;
        }
        #endregion
    }
}
