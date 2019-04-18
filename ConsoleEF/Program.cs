using MyEFCodeFirstModel;
using MyEFCodeFirstModel.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleEF
{
    class Program
    {
        private static int number = 0;
        private static object locker = new object();
        static int money = 10000;
        static void Main(string[] args)
        {
            //var resultss = Math.Log(100000000, 10);
            //Console.WriteLine(resultss);

            //ThreadTest2();

            #region 多线程分组采集

            //TestDB();
            //CheckDB();

            #endregion

            //DownLoad2();
            //Console.WriteLine("skfdsklafjsafj");


            //UserAsync();
            //List<string> list = UserAsync2().Result;


            //TestDB();
            //GetUpdateTimeStamp();
            TestTran2();
        }

        #region 时间戳timestamp）类型
        static void GetUpdateTimeStamp()
        {
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();


                var list = db.MyEntities.Where(d => d.Id == 1).FirstOrDefault();
                var list2 = db.MyEntities.Where(d => d.Id == 2).FirstOrDefault();
                string str1 = "0x" + BitConverter.ToString(list.LastUpdateTime as System.Byte[], 0).Replace("-", "");
                string str2 = "0x" + BitConverter.ToString(list2.LastUpdateTime as System.Byte[], 0).Replace("-", "");
                var t1 = Convert.ToInt32(str1, 16);
                var t2 = Convert.ToInt32(str2, 16);
                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                var seconds = ts2.TotalSeconds;
            }
        }

        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        #region 多线程分组采集

        static void GroupBy()
        {
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                var t1 = new DateTime(2016, 3, 1, 0, 0, 0);
                var t2 = new DateTime(2016, 6, 1, 0, 0, 0);
                var list = db.MyEntities.Where(d => d.CreateTime > t1 && d.CreateTime <= t2).Select(d => new
                {
                    d.CreateTime,
                    d.CreateTime.Value.Year,
                    d.CreateTime.Value.Month,
                    d.CreateTime.Value.Day,
                    d.CategoryId
                });

                var groupByResult = list.GroupBy(d => new { d.Year, d.Month, d.Day, d.CategoryId }, (x, y) => new
                {
                    x.Year,
                    x.Month,
                    x.Day,
                    x.CategoryId,
                    Amount = y.Count()
                }).ToList();


                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                var seconds = ts2.TotalSeconds;
                Console.WriteLine("耗时:{0},获取数据:{1}", seconds, groupByResult.Count());

            }
        }

        static void TestDB()
        {
            while (true)
            {
                using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    //List<Tuple<DateTime?, int, int>> timeListss = new List<Tuple<DateTime?, int, int>>();
                    //timeListss.Add(null);
                    //timeListss.Add(new Tuple<DateTime?, int, int>(DateTime.Now, 1, 1));
                    //var count = timeListss.Count();

                    DateTime? t1 = null;
                    DateTime? t2 = null;
                    DateTime? colltionTime = db.Collecction.Where(d => d.CateId == 38).FirstOrDefault() == null ? (new Collecction().CreateTime) : db.Collecction.Where(d => d.CateId == 38).FirstOrDefault().CreateTime;
                    if (colltionTime == null)
                    {
                        var dbTime = db.MyEntities.OrderBy(d => d.CreateTime).FirstOrDefault().CreateTime;
                        t1 = dbTime;
                        Collecction colletionmodel = new Collecction();
                        colletionmodel.CateId = 38;
                        colletionmodel.CreateTime = dbTime;
                        db.Collecction.Add(colletionmodel);
                        db.SaveChanges();
                    }
                    else
                    {
                        t1 = colltionTime;
                    }
                    t2 = t1.Value.AddDays(1);
                    if (t1 <= DateTime.Now)
                    {
                        List<Task> taskList = new List<Task>();
                        TaskFactory taskFactory = new TaskFactory();
                        List<Tuple<DateTime?, int, int>> timeList = new List<Tuple<DateTime?, int, int>>();
                        for (int i = 1; i < 6; i++)
                        {
                            int ii = i;
                            taskList.Add(taskFactory.StartNew(() => timeList.Add(TestList(ii, t1, t2))));//开启一个线程   里面创建索引
                        }

                        Task.WaitAll(taskList.ToArray());

                        Console.WriteLine("开始结算>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        var seconds = ts2.TotalSeconds;

                        var code = timeList.Where(d => d.Item1 != null).Sum(d => d.Item3);
                        if (code == 0)
                        {
                            Console.WriteLine("开始时间:{0}--结束时间:{1},更新:{2}数据 耗时:{3}", t1, t2, timeList.Sum(d => d.Item2), seconds);
                            var LastCreateTime = timeList.Max(d => d.Item1);
                            var upCollection = db.Collecction.Where(d => d.CateId == 38).FirstOrDefault();
                            upCollection.CreateTime = LastCreateTime;
                            db.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("开始时间:{0}--结束时间:{1},更新:{2}数据,采集出错事务回滚到时间:{3} 耗时:{4}", t1, t2, timeList.Sum(d => d.Item2), t1, seconds);
                        }
                    }
                }
            }
        }
        static Tuple<DateTime?, int, int> TestList(int i, DateTime? t1, DateTime? t2)
        {
            Console.WriteLine("线程:{0}进入方法,开始时间{1}--结束时间{2}", i, t1, t2);
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GroupByDto> list = new List<GroupByDto>();
                        var pageList = db.MyEntities.Where(d => d.CreateTime >= t1 && d.CreateTime < t2).OrderByDescending(d => d.UpDateTime).Skip((i - 1) * 500).Take(500).ToList();
                        Console.WriteLine("线程:{0}进入方法,开始时间{1}--结束时间{2}，获取源数据:{3}", i, t1, t2, pageList.Count());
                        if (pageList.Count() > 0)
                        {
                            list = pageList.Select(d => new
                            GroupByDto()
                            {
                                CategoryId = d.CategoryId,
                                UpDateTime = d.UpDateTime,
                                CreateTime = d.CreateTime,
                                Year = d.CreateTime.Value.Year,
                                Month = d.CreateTime.Value.Month,
                                Day = new DateTime(d.CreateTime.Value.Year, d.CreateTime.Value.Month, d.CreateTime.Value.Day)
                            }).ToList();
                        }
                        var LastUpdateTime = t1;
                        var LastCreateTime = t1;
                        var groupResult = list.GroupBy(d => new { d.Year, d.Month, d.Day, d.CategoryId }, (x, y) => new
                        {
                            x.CategoryId,
                            x.Year,
                            x.Month,
                            x.Day,
                            Amount = y.Count()
                        }).ToList();

                        if (groupResult.Count() > 0)
                        {
                            LastUpdateTime = list.Max(d => d.UpDateTime);
                            LastCreateTime = list.Max(d => d.CreateTime) > t2 ? list.Max(d => d.CreateTime) : t2;

                            var cateIds = groupResult.Select(d => d.CategoryId).Distinct().ToArray();
                            var cateTitle = db.Category.Where(d => cateIds.Contains(d.Id)).ToList();
                            long timeKey = 0;
                            int cateKey = 0;
                            foreach (var item in groupResult)
                            {
                                var dim_timeModel = db.Dim_Time.Where(d => d.Year == item.Year && d.Month == item.Month && d.Day == item.Day).FirstOrDefault();
                                if (dim_timeModel == null)
                                {
                                    Dim_Time model = new Dim_Time();
                                    model.Year = item.Year;
                                    model.Month = item.Month;
                                    model.Day = item.Day;
                                    model.UpdateTime = LastUpdateTime;
                                    db.Dim_Time.Add(model);
                                    db.SaveChanges();
                                    timeKey = model.TimeKey;
                                }
                                else
                                {
                                    dim_timeModel.UpdateTime = LastUpdateTime;
                                    db.SaveChanges();
                                    timeKey = dim_timeModel.TimeKey;
                                }


                                var dim_CateTypeModel = db.Dim_CateType.Where(d => d.CategoryId == item.CategoryId).FirstOrDefault();
                                if (dim_CateTypeModel == null)
                                {
                                    Dim_CateType model = new Dim_CateType();
                                    model.CategoryId = item.CategoryId;
                                    model.Title = cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault() == null ? string.Empty : cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault().Name;
                                    db.Dim_CateType.Add(model);
                                    db.SaveChanges();
                                    cateKey = model.CateTypeKey;
                                }
                                else
                                {
                                    dim_CateTypeModel.Title = cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault() == null ? string.Empty : cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault().Name;
                                    db.SaveChanges();
                                    cateKey = dim_CateTypeModel.CateTypeKey;
                                }

                                var jd_ExamModel = db.JD_ExamData.Where(d => d.TimeKey == timeKey && d.CateTypeKey == cateKey).FirstOrDefault();
                                if (jd_ExamModel == null)
                                {
                                    JD_ExamData model = new JD_ExamData();
                                    model.CateTypeKey = cateKey;
                                    model.TimeKey = timeKey;
                                    model.Amount = item.Amount;
                                    db.JD_ExamData.Add(model);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    jd_ExamModel.Amount += item.Amount;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            LastCreateTime = t1.Value.AddHours(1);
                        }
                        Console.WriteLine("线程:{0}结束方法,开始时间{1}--结束时间{2}:更新数据:{3}", i, t1, t2, groupResult.Sum(d => d.Amount) == null ? 0 : groupResult.Sum(d => d.Amount));
                        Thread.Sleep(500);
                        dbContextTransaction.Commit();
                        return new Tuple<DateTime?, int, int>(LastCreateTime, groupResult.Sum(d => d.Amount), 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("线程:{0}出错,开始时间{1}--结束时间{2}", i, t1, t2);
                        dbContextTransaction.Rollback();
                        Console.WriteLine(ex.Message);
                        return new Tuple<DateTime?, int, int>(t1, 0, -1);
                    }
                }
            }
        }
        static void CheckDB()
        {
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                var timeList = db.Dim_Time.ToList();
                foreach (var time in timeList)
                {
                    var t1 = time.Day;
                    var t2 = time.Day.Value.AddDays(1);
                    var upDateTime = db.MyEntities.Where(d => d.CreateTime >= t1 && d.CreateTime < t2).Max(d => d.UpDateTime);
                    Console.WriteLine("重新采集:开始时间:{0},结束时间:{1},采集重新时间:{2} 更新时间:{3}", t1, t2, time.UpdateTime, upDateTime);
                    if (upDateTime > time.UpdateTime)
                    {
                        db.MyEntities.SqlQuery("delete from JD_Commodity_002 where timekey=" + time.TimeKey);
                        db.SaveChanges();
                        Reset(t1, t2);

                    }
                }
            }
        }
        static void Reset(DateTime? t1, DateTime t2)
        {
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                List<Task> taskList = new List<Task>();
                TaskFactory taskFactory = new TaskFactory();
                List<Tuple<DateTime?, int, int>> timeList = new List<Tuple<DateTime?, int, int>>();
                for (int i = 1; i < 6; i++)
                {
                    int ii = i;
                    taskList.Add(taskFactory.StartNew(() => timeList.Add(RetTestList(ii, t1, t2))));//开启一个线程   里面创建索引
                }

                Task.WaitAll(taskList.ToArray());

                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                var seconds = ts2.TotalSeconds;

                var code = timeList.Where(d => d.Item1 != null).Sum(d => d.Item3);
                if (code == 0)
                {
                    Console.WriteLine("重新采集开始时间:{0}--结束时间:{1},更新:{2}数据 耗时:{3}", t1, t2, timeList.Sum(d => d.Item2), seconds);
                    var LastCreateTime = timeList.Max(d => d.Item1);
                    var upCollection = db.Collecction.Where(d => d.CateId == 38).FirstOrDefault();
                    upCollection.CreateTime = LastCreateTime;
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("重新采集开始时间:{0}--结束时间:{1},重新更新:{2}数据,采集出错事务回滚到时间:{3} 耗时:{4}", t1, t2, timeList.Sum(d => d.Item2), t1, seconds);
                }
            }
        }
        static Tuple<DateTime?, int, int> RetTestList(int i, DateTime? t1, DateTime? t2)
        {
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GroupByDto> list = new List<GroupByDto>();
                        var pageList = db.MyEntities.Where(d => d.CreateTime >= t1 && d.CreateTime < t2).OrderByDescending(d => d.UpDateTime).Skip((i - 1) * 10).Take(10).ToList();
                        if (pageList.Count() > 0)
                        {
                            list = pageList.Select(d => new
                            GroupByDto()
                            {
                                CategoryId = d.CategoryId,
                                UpDateTime = d.UpDateTime,
                                CreateTime = d.CreateTime,
                                Year = d.CreateTime.Value.Year,
                                Month = d.CreateTime.Value.Month,
                                Day = new DateTime(d.CreateTime.Value.Year, d.CreateTime.Value.Month, d.CreateTime.Value.Day)
                            }).ToList();
                        }
                        var LastUpdateTime = t1;
                        var LastCreateTime = t1;
                        var groupResult = list.GroupBy(d => new { d.Year, d.Month, d.Day, d.CategoryId }, (x, y) => new
                        {
                            x.CategoryId,
                            x.Year,
                            x.Month,
                            x.Day,
                            Amount = y.Count()
                        }).ToList();

                        if (groupResult.Count() > 0)
                        {
                            LastUpdateTime = list.Max(d => d.UpDateTime);
                            LastCreateTime = list.Max(d => d.CreateTime) > t2 ? list.Max(d => d.CreateTime) : t2;

                            var cateIds = groupResult.Select(d => d.CategoryId).Distinct().ToArray();
                            var cateTitle = db.Category.Where(d => cateIds.Contains(d.Id)).ToList();
                            long timeKey = 0;
                            int cateKey = 0;
                            foreach (var item in groupResult)
                            {
                                var dim_timeModel = db.Dim_Time.Where(d => d.Year == item.Year && d.Month == item.Month && d.Day == item.Day).FirstOrDefault();
                                if (dim_timeModel == null)
                                {
                                    Dim_Time model = new Dim_Time();
                                    model.Year = item.Year;
                                    model.Month = item.Month;
                                    model.Day = item.Day;
                                    model.UpdateTime = LastUpdateTime;
                                    db.Dim_Time.Add(model);
                                    db.SaveChanges();
                                    timeKey = model.TimeKey;
                                }
                                else
                                {
                                    dim_timeModel.UpdateTime = LastUpdateTime;
                                    db.SaveChanges();
                                    timeKey = dim_timeModel.TimeKey;
                                }


                                var dim_CateTypeModel = db.Dim_CateType.Where(d => d.CategoryId == item.CategoryId).FirstOrDefault();
                                if (dim_CateTypeModel == null)
                                {
                                    Dim_CateType model = new Dim_CateType();
                                    model.CategoryId = item.CategoryId;
                                    model.Title = cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault() == null ? string.Empty : cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault().Name;
                                    db.Dim_CateType.Add(model);
                                    db.SaveChanges();
                                    cateKey = model.CateTypeKey;
                                }
                                else
                                {
                                    dim_CateTypeModel.Title = cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault() == null ? string.Empty : cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault().Name;
                                    db.SaveChanges();
                                    cateKey = dim_CateTypeModel.CateTypeKey;
                                }

                                var jd_ExamModel = db.JD_ExamData.Where(d => d.TimeKey == timeKey && d.CateTypeKey == cateKey).FirstOrDefault();
                                if (jd_ExamModel == null)
                                {
                                    JD_ExamData model = new JD_ExamData();
                                    model.CateTypeKey = cateKey;
                                    model.TimeKey = timeKey;
                                    model.Amount = item.Amount;
                                    db.JD_ExamData.Add(model);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    jd_ExamModel.Amount += item.Amount;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            LastCreateTime = t1.Value.AddHours(1);
                        }
                        dbContextTransaction.Commit();
                        return new Tuple<DateTime?, int, int>(LastCreateTime, groupResult.Sum(d => d.Amount), 0);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        Console.WriteLine(ex.Message);
                        return new Tuple<DateTime?, int, int>(t1, 0, -1);
                    }
                }
            }
        }
        #endregion

        #region async await
        static async void UserAsync()
        {
            using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //List<Tuple<DateTime?, int, int>> timeListss = new List<Tuple<DateTime?, int, int>>();
                //timeListss.Add(null);
                //timeListss.Add(new Tuple<DateTime?, int, int>(DateTime.Now, 1, 1));
                //var count = timeListss.Count();

                DateTime? t1 = null;
                DateTime? t2 = null;
                DateTime? colltionTime = db.Collecction.Where(d => d.CateId == 38).FirstOrDefault() == null ? (new Collecction().CreateTime) : db.Collecction.Where(d => d.CateId == 38).FirstOrDefault().CreateTime;
                if (colltionTime == null)
                {
                    var dbTime = db.MyEntities.OrderBy(d => d.CreateTime).FirstOrDefault().CreateTime;
                    t1 = dbTime;
                    Collecction colletionmodel = new Collecction();
                    colletionmodel.CateId = 38;
                    colletionmodel.CreateTime = dbTime;
                    db.Collecction.Add(colletionmodel);
                    db.SaveChanges();
                }
                else
                {
                    t1 = colltionTime;
                }
                t2 = t1.Value.AddDays(1);
                if (t1 <= DateTime.Now)
                {
                    List<Tuple<DateTime?, int, int>> timeList = new List<Tuple<DateTime?, int, int>>();

                    for (var i = 0; i < 6; i++)
                    {
                        timeList.Add(await TestListAsync(i, t1, t2));
                    }
                    sw.Stop();
                    TimeSpan ts2 = sw.Elapsed;
                    var seconds = ts2.TotalSeconds;

                    var code = timeList.Where(d => d.Item1 != null).Sum(d => d.Item3);
                    if (code == 0)
                    {
                        Console.WriteLine("开始时间:{0}--结束时间:{1},更新:{2}数据 耗时:{3}", t1, t2, timeList.Sum(d => d.Item2), seconds);
                        var LastCreateTime = timeList.Max(d => d.Item1);
                        var upCollection = db.Collecction.Where(d => d.CateId == 38).FirstOrDefault();
                        upCollection.CreateTime = LastCreateTime;
                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("开始时间:{0}--结束时间:{1},更新:{2}数据,采集出错事务回滚到时间:{3} 耗时:{4}", t1, t2, timeList.Sum(d => d.Item2), t1, seconds);
                    }
                }
            }
        }

        private static Task<Tuple<DateTime?, int, int>> TestListAsync(int i, DateTime? t1, DateTime? t2)
        {
            return Task.Run(() =>
             {
                 using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
                 {
                     using (var dbContextTransaction = db.Database.BeginTransaction())
                     {
                         try
                         {
                             List<GroupByDto> list = new List<GroupByDto>();
                             var pageList = db.MyEntities.Where(d => d.CreateTime >= t1 && d.CreateTime < t2).OrderByDescending(d => d.UpDateTime).Skip((i - 1) * 10).Take(10).ToList();
                             if (pageList.Count() > 0)
                             {
                                 list = pageList.Select(d => new
                                 GroupByDto()
                                 {
                                     CategoryId = d.CategoryId,
                                     UpDateTime = d.UpDateTime,
                                     CreateTime = d.CreateTime,
                                     Year = d.CreateTime.Value.Year,
                                     Month = d.CreateTime.Value.Month,
                                     Day = new DateTime(d.CreateTime.Value.Year, d.CreateTime.Value.Month, d.CreateTime.Value.Day)
                                 }).ToList();
                             }
                             var LastUpdateTime = t1;
                             var LastCreateTime = t1;
                             var groupResult = list.GroupBy(d => new { d.Year, d.Month, d.Day, d.CategoryId }, (x, y) => new
                             {
                                 x.CategoryId,
                                 x.Year,
                                 x.Month,
                                 x.Day,
                                 Amount = y.Count()
                             }).ToList();

                             if (groupResult.Count() > 0)
                             {
                                 LastUpdateTime = list.Max(d => d.UpDateTime);
                                 LastCreateTime = list.Max(d => d.CreateTime) > t2 ? list.Max(d => d.CreateTime) : t2;

                                 var cateIds = groupResult.Select(d => d.CategoryId).Distinct().ToArray();
                                 var cateTitle = db.Category.Where(d => cateIds.Contains(d.Id)).ToList();
                                 long timeKey = 0;
                                 int cateKey = 0;
                                 foreach (var item in groupResult)
                                 {
                                     var dim_timeModel = db.Dim_Time.Where(d => d.Year == item.Year && d.Month == item.Month && d.Day == item.Day).FirstOrDefault();
                                     if (dim_timeModel == null)
                                     {
                                         Dim_Time model = new Dim_Time();
                                         model.Year = item.Year;
                                         model.Month = item.Month;
                                         model.Day = item.Day;
                                         model.UpdateTime = LastUpdateTime;
                                         db.Dim_Time.Add(model);
                                         db.SaveChanges();
                                         timeKey = model.TimeKey;
                                     }
                                     else
                                     {
                                         dim_timeModel.UpdateTime = LastUpdateTime;
                                         db.SaveChanges();
                                         timeKey = dim_timeModel.TimeKey;
                                     }


                                     var dim_CateTypeModel = db.Dim_CateType.Where(d => d.CategoryId == item.CategoryId).FirstOrDefault();
                                     if (dim_CateTypeModel == null)
                                     {
                                         Dim_CateType model = new Dim_CateType();
                                         model.CategoryId = item.CategoryId;
                                         model.Title = cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault() == null ? string.Empty : cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault().Name;
                                         db.Dim_CateType.Add(model);
                                         db.SaveChanges();
                                         cateKey = model.CateTypeKey;
                                     }
                                     else
                                     {
                                         dim_CateTypeModel.Title = cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault() == null ? string.Empty : cateTitle.Where(d => d.Id == item.CategoryId).FirstOrDefault().Name;
                                         db.SaveChanges();
                                         cateKey = dim_CateTypeModel.CateTypeKey;
                                     }

                                     var jd_ExamModel = db.JD_ExamData.Where(d => d.TimeKey == timeKey && d.CateTypeKey == cateKey).FirstOrDefault();
                                     if (jd_ExamModel == null)
                                     {
                                         JD_ExamData model = new JD_ExamData();
                                         model.CateTypeKey = cateKey;
                                         model.TimeKey = timeKey;
                                         model.Amount = item.Amount;
                                         db.JD_ExamData.Add(model);
                                         db.SaveChanges();
                                     }
                                     else
                                     {
                                         jd_ExamModel.Amount += item.Amount;
                                         db.SaveChanges();
                                     }
                                 }
                             }
                             else
                             {
                                 LastCreateTime = t1.Value.AddHours(1);
                             }
                             dbContextTransaction.Commit();
                             Console.WriteLine("线程:{0},开始时间{1}--结束时间{2}:更新数据:{3}", i, t1, t2, groupResult.Sum(d => d.Amount) == null ? 0 : groupResult.Sum(d => d.Amount));
                             return new Tuple<DateTime?, int, int>(LastCreateTime, groupResult.Sum(d => d.Amount), 0);
                         }
                         catch (Exception ex)
                         {
                             dbContextTransaction.Rollback();
                             Console.WriteLine(ex.Message);
                             return new Tuple<DateTime?, int, int>(t1, 0, -1);
                         }
                     }
                 }
             });
        }
        #endregion

        #region 线程测试一
        static void ThreadTest1()
        {
            #region 测试一
            Thread th1 = null;
            Thread th2 = null;
            th1 = new Thread(() =>
            {
                for (var i = 0; i < 1000; i++)
                {

                    Console.WriteLine("Th1:{0}", number++);
                    Thread.Sleep(1);
                }

            });
            th1.Start();
            th2 = new Thread(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    th1.Join();
                    Console.WriteLine("Th2:{0}", number++);
                    Thread.Sleep(1);
                }
            });
            th2.Start();

            //th2.Join();
            //while (th1.IsAlive) { }
            //while (th2.IsAlive) { }
            Console.WriteLine("结果:《《《》》》》----------------{0}", number);
            #endregion
        }
        #endregion

        #region 线程测试二
        static void ThreadTest2()
        {
            Thread th1 = new Thread(() =>
              {
                  for (var i = 0; i < 1000; i++)
                  {
                      GetSetMoney("wj");
                  }
              });

            Thread th2 = new Thread(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    GetSetMoney("ww");
                }
            });

            th1.Start();
            th2.Start();
            //th1.Join();
            //th2.Join();
        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        static void GetSetMoney(string name)
        {
            //Console.WriteLine(string.Format("{0}查看余额:{1}", name, money));
            ////int yue = money - 1;
            //Console.WriteLine(string.Format("{0}取钱",name));
            //money = yue;
            //money--;
            money = money - 1;
            Console.WriteLine(string.Format("{0}取完了，还有:{1}", name, money));
        }
        #endregion

        #region 线程测试三
        static async void ThreadTest()
        {
            using (FileStream fs = File.OpenRead(@"F:\视频2\视频\黑马\05 并发编程\1.txt"))
            {
                byte[] buffer = new byte[50];
                await fs.ReadAsync(buffer, 0, buffer.Length);
                string s = Encoding.Default.GetString(buffer);
                Console.WriteLine(s);
            }
        }

        static async Task<List<string>> UserAsync2()
        {
            List<string> lsit = new List<string>();
            lsit.Add(await Async());
            return lsit;
        }

        static Task<string> Async()
        {
            return Task.Run(() =>
            {

                return "wj";
            });
        }

        static void DownLoad2()
        {
            WebClient wc = new WebClient();
            string s = wc.DownloadString("http://www.rupeng.com");
            Console.WriteLine(s);
        }

        static async void DownLoad1()
        {
            WebClient wc = new WebClient();
            string s = await wc.DownloadStringTaskAsync("http://www.rupeng.com");
            Console.WriteLine(s);
            //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            //string s2 = await wc.DownloadStringTaskAsync("http://www.baidu.com");
            //Console.WriteLine(s2);
        }
        #endregion

        #region 测试事务
        public static void TestTran2()
        {
            using (MyTest1 db2 = new MyEFCodeFirstModel.MyTest1())
            {
                using (var dbContextTransaction = db2.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var model = db2.Collecction.Where(d => d.Id == 10377).FirstOrDefault();
                        model.CateId = 48;
                        db2.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

        }
    }
    #endregion
}
