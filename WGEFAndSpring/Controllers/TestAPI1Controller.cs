using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace WGEFAndSpring.Controllers
{
    public class TestAPI1Controller : ApiController
    {
        public  TestAPI1Controller()
        {
        }

        public class BaseResult
        {
            public const int crror_code = -1;
            public const int success_code = 0;
            public int code { get; set; }
            public string msg { get; set; }
            public static BaseResult Create(int code,string msg)
            {
                return new BaseResult { code = code, msg = msg };
            }
        }
        public class DataResult<T> : BaseResult
        {
           public T data { get; set; }

            public static DataResult<T> SuccessResult(T t,string msg)
            {
                return new DataResult<T> { code = 0, msg = msg, data = t };
            }
        }

        [HttpGet]
        public DataResult<User> Test1(string name, int id)
        {
            User model = new User();
            model.Name = name;
            model.Id = id;
            return DataResult<User>.SuccessResult(model, "ok");
        }

        [HttpPost]
        public DataResult<User> AddTest(User user)
        {
            User model = new User();
            model.Name = user.Name;
            model.Id = user.Id;
            return DataResult<User>.SuccessResult(model, "ok");
        }

        [HttpPost]
        public DataResult<User> AddTest2(List<string> List)
        {
            User model = new User();
            model.Name = List[0].ToString();
            model.Id = List.Count();
            return DataResult<User>.SuccessResult(model, "ok");
        }

        public class User
        {
            public string Name { get; set; }

            public int Id { get; set; }
        }

    }
}
