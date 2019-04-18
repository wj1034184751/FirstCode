namespace MyEFCodeFirstModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;

    public class MyTest1 : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“MyTest1”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“MyEFCodeFirstModel.MyTest1”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“MyTest1”
        //连接字符串。
        public MyTest1()
            : base("name=MyTest1")
        {
            Database.SetInitializer<MyTest1>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        public virtual DbSet<JD_Commodity_001> MyEntities { get; set; }
        public virtual DbSet<JD_Class> JDClass { get; set; }
        public virtual DbSet<JD_Student> JDStudent { get; set; }
        public virtual DbSet<Collecction> Collecction { get; set; }
        public virtual DbSet<Dim_Time> Dim_Time { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Dim_CateType> Dim_CateType { get; set; }
        public virtual DbSet<JD_ExamData> JD_ExamData { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}