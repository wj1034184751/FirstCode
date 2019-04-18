using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class  DbContextFactory: IDBContextFactory
    {
        #region 创建EF上下文对象 + DbContext GetDbContext()
        /// <summary>
        /// 创建EF上下文对象
        /// 在线程中共享一个上下文对象
        /// </summary>
        /// <returns></returns>
        public System.Data.Entity.DbContext GetDbContext()
        {
            //从当前线程中获得EF上下文对象
            DbContext dbContext = CallContext.GetData(typeof(DbContextFactory).Name) as DbContext;
            if (dbContext == null)
            {
                dbContext = new MyEFModel.MyTest();
                CallContext.SetData(typeof(DbContextFactory).Name, dbContext);
            }
            return dbContext;
        }
        #endregion
    }
}
