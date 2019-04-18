using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBaseBLL<T>
    {
        //数据层  父接口

        #region  增加数据 + int Add(T model);
        int Add(T model);
        #endregion

        #region 删除数据 + int Del(T model);
        int Del(T model);
        #endregion

        #region 根据条件删除数据 +int DelBy(Expression<Func<T, bool>> delwhere)
        int DelBy(Expression<Func<T, bool>> delwhere);
        #endregion

        #region 根据条件修改数据 +  int ModiftyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifyStr);
        int ModiftyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifyStr);
        #endregion

        #region 根据条件得到数据 + List<T> GetListBy(Expression<Func<T, bool>> whereLambda);
        List<T> GetListBy(Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 根据条件查询排序 + List<T> GetListBy<Tkey>(Expression<Func<T, bool>> whereLambad, Expression<Func<T, Task>> orderLambad);
        List<T> GetListBy<Tkey>(Expression<Func<T, bool>> whereLambad, Expression<Func<T, Task>> orderLambad);
        #endregion

        #region 根据条件分页 + List<T> GetPageList<Tkey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambad, Expression<Func<T, Tkey>> orderwhere, bool isAsc);
        List<T> GetPageList<Tkey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambad, Expression<Func<T, Tkey>> orderwhere, bool isAsc);
        #endregion
    }
}
