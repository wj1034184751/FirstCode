
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MyTest1;user id=sa;password=wj1034184751;Integrated Security=True;MultipleActiveResultSets=True");

            //var result = connection.Execute("Insert into Customer values (@Name, @Age, @Sex,@IsDelete)",
            //                       new { Name = "jack", Age = 1, Sex = 0, IsDelete = 1 });

            //var usersList = Enumerable.Range(0, 10).Select(d => new Customer()
            //{
            //    Name = "wj" + d,
            //    Age = d,
            //    Sex = d % 2 == 1 ? 1 : 0,
            //    IsDelete = d % 2 == 1 ? true : false
            //});
            //var result = connection.Execute("Insert into Customer values (@Name, @Age, @Sex,@IsDelete)", usersList);
            var query = connection.Query<Customer>("select * from Customer where Name=@Name", new { Name = "jack" });

        }
    }
    public partial class Customer
    {
        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Sex { get; set; }

        public bool? IsDelete { get; set; }
    }
}
