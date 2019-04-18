using DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class BaseBLL<T> : IBaseBLL<T> where T : class, new()
    {

        public BaseBLL()
        {
            SetDAL();
        }
        public IBaseDLL<T> idal;

        //数据仓储上下文
        private IDLLSession iDBSession;

        public  IDLLSession IDBSession
        {
            get
            {
                if (iDBSession == null)
                {
                    //1.第一种读取配置文件
                    #region 第一种读取配置文件
                    //读取.dll路径
                    //string DBSessionFactoryDLL = Common.ConfigurationHelper.AppSetting("DLLSessionFactoryDLL");
                    ////读取.dll中的DBSessionFactory类工厂
                    //string DBSessionFactory = Common.ConfigurationHelper.AppSetting("DLLSessionFactory");
                    ////反射创建工厂
                    ////获取.dll路径，创建反射Assembly
                    //Assembly dll = Assembly.LoadFrom(DBSessionFactoryDLL);
                    ////通过实明（类名) 得到工厂类
                    //Type typeDBSessionFactory = dll.GetType(DBSessionFactory);
                    ////通过反射创建类(实例化类)
                    //IDLL.IDLLSessionFactory sessionfactory = Activator.CreateInstance(typeDBSessionFactory) as IDLL.IDLLSessionFactory;
                    #endregion

                    //2.第二种通过string.net创建工厂
                    #region 第二种通过string.net创建工厂
                    IDLLSessionFactory sessionfactory = DI.StringHelper.GetObject<IDLLSessionFactory>("DLLDBSessionFactory");
                    #endregion

                    //3.通过 工厂 创建DBSession对象
                    iDBSession = sessionfactory.GetDBSession();
                }
                return iDBSession;
            }
            set
            {
                iDBSession = value;
            }
        }

        public abstract void SetDAL();

        #region  增加数据 + int Add(T model);
        public int Add(T model)
        {
            return idal.Add(model);
        }
        #endregion

        #region 删除数据 + int Del(T model);
        public int Del(T model)
        {
            return idal.Del(model);
        }
        #endregion

        #region 根据条件删除数据 +int DelBy(Expression<Func<T, bool>> delwhere)
        public int DelBy(Expression<Func<T, bool>> delwhere)
        {
            return idal.DelBy(delwhere);
        }
        #endregion

        #region 根据条件修改数据 +  int ModiftyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifyStr);
        public int ModiftyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifyStr)
        {
            return idal.ModiftyBy(model, whereLambda, modifyStr);
        }
        #endregion

        #region 根据条件得到数据 + List<T> GetListBy(Expression<Func<T, bool>> whereLambda);
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return idal.GetListBy(whereLambda);
        }
        #endregion

        #region 根据条件查询排序 + List<T> GetListBy<Tkey>(Expression<Func<T, bool>> whereLambad, Expression<Func<T, Task>> orderLambad);
        public List<T> GetListBy<Tkey>(Expression<Func<T, bool>> whereLambad, Expression<Func<T, Task>> orderLambad)
        {
            return idal.GetListBy<Tkey>(whereLambad, orderLambad);
        }
        #endregion

        #region 根据条件分页 + List<T> GetPageList<Tkey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambad, Expression<Func<T, Tkey>> orderwhere, bool isAsc);
        public List<T> GetPageList<Tkey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambad, Expression<Func<T, Tkey>> orderwhere, bool isAsc)
        {
            return idal.GetPageList<Tkey>(pageIndex, pageSize, ref rowCount, whereLambad, orderwhere, isAsc);
        }
        #endregion
    }
}
