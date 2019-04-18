using MyEFCodeFirstModel;
using MyEFCodeFirstModel.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_Helper;

namespace WGEFAndSpring.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (MyTest1 db = new MyEFCodeFirstModel.MyTest1())
            //{
            //    Stopwatch sw = new Stopwatch();
            //    sw.Start();
            //    DateTime t1 = new DateTime(2018, 1, 1, 0, 0, 0);
            //    DateTime t2 = t1.AddDays(1);
            //    var list = db.MyEntities.Where(d => d.UpDateTime >= t1 && d.UpDateTime < t2).OrderBy(d => d.UpDateTime).Skip(1).Take(500).ToList();
            //    //var st = db.JDStudent.FirstOrDefault();
            //    //var name = st.JD_Class.Name;
            //    sw.Stop();
            //    TimeSpan ts2 = sw.Elapsed;
            //    var seconds = ts2.TotalMilliseconds;
            //}

            //using (DbContext db = new MyEFCodeFirstModel.MyTest1())
            //{
            //    //var model = db.Set<JD_Commodity_001>().Where(d => d.Id == 525352).FirstOrDefault();
            //    //db.Database.Log = (sql) =>
            //    //{
            //    //    ViewData["ShowLog"] = sql;
            //    //};
            //}
            //TestDBAsync();

            //var list = OperateContext.Current.BLLSession.IJD_Commodity_001BLL.GetListById(525352);
            HttpCookie cookie = new HttpCookie("data");
            cookie.Values.Add("name", "123");
            cookie.Expires = DateTime.Now.AddMinutes(1);
            Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Session["wj"] = 1;
            return View();
        }

        public ActionResult Test2()
        {

            var sessions = System.Web.HttpContext.Current.Session["ijosgx5dvvagj5gc3q4tnmyi"];
            return View();
        }

        public JsonResult OldSiteTest()
        {
            DownLoadTest dwtest = new DownLoadTest();
            var task1 = dwtest.DownLoadString("https://stackoverflow.com/");
            var task2 = dwtest.DownLoadString("https://github.com/");
            //Debug.WriteLine(string.Format("task1.Result.Length=" + task1.Result.Item1)
            //Debug.WriteLine("task2.Result.Length=" + task2.Result.Length);
            return Json(new { task1=task1.Item2, task2 = task2.Item2, }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SiteTest()
        {
            //var list=await GetTest();

            //DownLoadTest dwtest = new DownLoadTest();
            //var task1 = dwtest.DownLoadStringTaskAsync("https://stackoverflow.com/");
            //var task2 = dwtest.DownLoadStringTaskAsync("https://github.com/");
            //Debug.WriteLine("task.Result等待结果打印");
            //task1.Wait();
            //task2.Wait();
            //Debug.WriteLine("task1.Result.Length=" + task1.Result.Length);
            //Debug.WriteLine("task2.Result.Length=" + task2.Result.Length);
            //return Json(new { task1Status = task1.Status, task2Status = task2.Status }, JsonRequestBehavior.AllowGet);

            //DownLoadTest dwtest = new DownLoadTest();
            //var task1 =await dwtest.DownLoadStringTaskAsync("https://stackoverflow.com/");
            //var task2 = await dwtest.DownLoadStringTaskAsync("https://github.com/");
            ////Debug.WriteLine(string.Format("task1.Result.Length=" + task1.Result.Item1)
            ////Debug.WriteLine("task2.Result.Length=" + task2.Result.Length);
            //return Json(new { task1 = task1.Item2, task2 = task2.Item2, }, JsonRequestBehavior.AllowGet);
            TestDBAsync();
            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async void GetFile()
        {
            byte[] b = new Byte[1024];//定义两个字节数组
            byte[] mb = new Byte[1024];
            int intRead = 0;
            Stream StreamRead = System.IO.File.OpenRead(@"F:\视频2\视频\黑马\05 并发编程\1.txt");//打开被读取的文件
            intRead = await StreamRead.ReadAsync(b,
                0, 1024);//写入到数组b中，并还回b的数据长度

            MemoryStream myMemoryStream = new MemoryStream(intRead);//声明内存流大小     
            await myMemoryStream.WriteAsync(b, 0, intRead);//将b写入myMemoryStream中
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            var fileName = HttpUtility.UrlEncode("测试");
            Response.AppendHeader("Content-Disposition", "attachment; filename = " + "测试");
            Response.BinaryWrite(myMemoryStream.GetBuffer());
        }
        public async Task<List<int>> GetTest()
        {
            List<int> list = new List<int>();

            var task1 = TaskAmount1Async();
            var task2 = TaskAmount2Async();
            var task3 = TaskAmount3Async();
            //list.Add(await TaskAmount1Async());
            //list.Add(await TaskAmount2Async());
            //list.Add(await TaskAmount3Async());
            return list;
        }

        public Task<int> TaskAmount1Async()
        {
            return Task.Run(() =>
            {
                return 1;
            });
        }
        public Task<int> TaskAmount2Async()
        {
            return Task.Run(() =>
            {
                return 2;
            });
        }
        public Task<int> TaskAmount3Async()
        {
            return Task.Run(() =>
            {
                return 3;
            });
        }

        #region async await
        static async void TestDBAsync()
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
                        List<Tuple<DateTime?, int, int>> timeList = new List<Tuple<DateTime?, int, int>>();

                        for (var i = 1; i < 6; i++)
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
                            var pageList = db.MyEntities.Where(d => d.CreateTime >= t1 && d.CreateTime < t2).OrderByDescending(d => d.UpDateTime).Skip((i - 1) * 500).Take(500).ToList();
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


    }

    public class DownLoadTest
    {
        Stopwatch watch = new Stopwatch();
        public DownLoadTest()
        {
            watch.Start();
        }
        public async Task<Tuple<string, int>> DownLoadStringTaskAsync(string url)
        {
            Debug.WriteLine(string.Format("异步程序获取{0}开始运行:{1,4:N0}ms", url, watch.Elapsed.TotalMilliseconds));
            WebClient wc = new WebClient();
            string str = await wc.DownloadStringTaskAsync(url);
            Debug.WriteLine(string.Format("异步程序获取{0}运行结束:{1,4:N0}ms", url, watch.Elapsed.TotalMilliseconds));
            return new Tuple<string, int>(str, (int)watch.Elapsed.TotalSeconds);
        }
        public Tuple<string,int> DownLoadString(string url)
        {
            Debug.WriteLine(string.Format("异步程序获取{0}开始运行:{1,4:N0}ms", url, watch.Elapsed.TotalMilliseconds));
            WebClient wc = new WebClient();
            string str =  wc.DownloadString(url);
            Debug.WriteLine(string.Format("异步程序获取{0}运行结束:{1,4:N0}ms", url, watch.Elapsed.TotalMilliseconds));
            return new Tuple<string, int>(str, (int)watch.Elapsed.TotalSeconds);
        }
    }
}