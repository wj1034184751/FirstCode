using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class BaseDLL<T> : IBaseDLL<T> where T : class, new()
    {
        /// <summary>
        /// EF上下文对象
        /// </summary>
        DbContext db = new DbContextFactory().GetDbContext();

        #region  增加数据 + int Add(T model);
        public int Add(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges();
        }
        #endregion

        #region 删除数据 + int Del(T model);
        public int Del(T model)
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }
        #endregion

        #region 根据条件删除数据 +int DelBy(Expression<Func<T, bool>> delwhere)
        public int DelBy(Expression<Func<T, bool>> delwhere)
        {
            List<T> listDel = db.Set<T>().Where(delwhere).ToList();
            listDel.ForEach(d =>
            {
                db.Set<T>().Attach(d);
                db.Set<T>().Remove(d);
            });
            return db.SaveChanges();
        }
        #endregion

        #region 根据条件修改数据 +  int ModiftyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifyStr);
        public int ModiftyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifyStr)
        {
            List<T> listModles = db.Set<T>().Where(whereLambda).ToList();
            Type type = typeof(T);
            List<PropertyInfo> proinfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            Dictionary<string, PropertyInfo> dicPros = new Dictionary<string, PropertyInfo>();
            proinfos.ForEach(d =>
            {
                if (modifyStr.Contains(d.Name))
                {
                    dicPros.Add(d.Name, d);
                }
            });
            foreach (string ModifyName in modifyStr)
            {
                if (dicPros.ContainsKey(ModifyName))
                {
                    PropertyInfo proInfo = dicPros[ModifyName];
                    object value = proInfo.GetValue(model, null);
                    foreach (T user in listModles)
                    {
                        proInfo.SetValue(user, value, null);
                    }
                }
            }
            return db.SaveChanges();

        }
        #endregion

        #region 根据条件得到数据 + List<T> GetListBy(Expression<Func<T, bool>> whereLambda);
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).ToList();
        }
        #endregion

        #region 根据条件查询排序 + List<T> GetListBy<Tkey>(Expression<Func<T, bool>> whereLambad, Expression<Func<T, Task>> orderLambad);
        public List<T> GetListBy<Tkey>(Expression<Func<T, bool>> whereLambad, Expression<Func<T, Task>> orderLambad)
        {
            return db.Set<T>().Where(whereLambad).OrderBy(orderLambad).ToList();
        }
        #endregion

        #region 根据条件分页 + List<T> GetPageList<Tkey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambad, Expression<Func<T, Tkey>> orderwhere, bool isAsc);
        public List<T> GetPageList<Tkey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambad, Expression<Func<T, Tkey>> orderwhere, bool isAsc)
        {
            rowCount = db.Set<T>().Where(whereLambad).Count();
            if (isAsc)
            {
                return db.Set<T>().Where(whereLambad).OrderBy(orderwhere).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambad).OrderByDescending(orderwhere).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        #endregion
    }
}
