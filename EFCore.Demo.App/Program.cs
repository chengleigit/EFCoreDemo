using EFCore.Demo.Data;
using EFCore.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Update_A();

        }

        static void Save_1()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            League league_A = new League()
            {
                Name = "SeveA",
                Country = "意大利"
            };

            dbContext.Add(league_A);
            int resultCount = dbContext.SaveChanges();
            Console.WriteLine(resultCount);
        }

        static void Save_2()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var svenA = dbContext.Leagues.Single(i => i.Name == "SeveA");

            League league_A = new League()
            {
                Name = "league_A",
                Country = "意大利"
            };

            League league_B = new League()
            {
                Name = "league_B",
                Country = "意大利"
            };

            Club club_A = new Club()
            {
                Name = "球赛俱乐部A",
                City = "西欧",
                DateOfEstablishment = new DateTime(1988, 2, 18),
                League = svenA
            };

            //dbContext.Leagues.AddRange(league_B, league_A);
            dbContext.AddRange(league_B, league_A, club_A);

            int resultCount = dbContext.SaveChanges();
            Console.WriteLine(resultCount);
        }

        static void Query_1()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var Leagues = dbContext.Leagues.Where(i => i.Id > 0).ToList();


            foreach (var l in Leagues)
            {
                Console.WriteLine(l.Name);
            }

            Console.WriteLine("结束！！");
        }

        static void AsNoTracking_1()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var Leagues = dbContext.Leagues.AsNoTracking().First();//AsNoTracking() 作用不对查询出的数据进行跟踪

            Leagues.Name = "~~";

            dbContext.Leagues.Update(Leagues);
            dbContext.SaveChanges();

            Console.WriteLine("结束！！");
        }


        //通过导航属性添加关系数据
        static void AddGXData_A()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var svenA = dbContext.Leagues.Single(i => i.Name == "SeveA");

            Club club_A = new Club()
            {
                Name = "球赛俱乐部A",
                City = "西欧",
                DateOfEstablishment = new DateTime(1988, 2, 18),
                League = svenA,
                Players = new List<Player>()
                {
                    new Player()
                    {
                       Name="C罗A",
                       DateOfBirth=new DateTime(1985,12,5)
                    },
                    new Player()
                    {
                       Name="C罗B",
                       DateOfBirth=new DateTime(1985,12,5)
                    }
                }
            };

            dbContext.Clubs.Add(club_A);

            int resultCount = dbContext.SaveChanges();
            Console.WriteLine(resultCount);
            Console.ReadLine();
            Console.WriteLine("结束！！");
        }

        static void AddGXData_B()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var club = dbContext.Clubs.First(i => i.Name == "球赛俱乐部A");

            club.Players.Add(new Player() { Name = "C罗D", DateOfBirth = new DateTime(1995, 11, 20) });

            int resultCount = dbContext.SaveChanges();
            Console.WriteLine(resultCount);
            Console.WriteLine();
            Console.WriteLine("结束！！");
        }

        static void AddGXData_C()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var club = dbContext.Clubs.First(i => i.Name == "球赛俱乐部A");

            club.Players.Add(new Player() { Name = "C罗E", DateOfBirth = new DateTime(1997, 11, 20) });

            {
                using DemoDBContext newDbContext = new DemoDBContext();
                newDbContext.Clubs.Attach(club);


                int resultCount = dbContext.SaveChanges();
                Console.WriteLine(resultCount);
                Console.WriteLine();
                Console.WriteLine("结束！！");

            }

        }

        static void AddGXData_D()
        {
            using DemoDBContext dbContext = new DemoDBContext();


            Resume resume = new Resume()
            {
                PlayerId = 1,
                Description = "球员简历描述！"
            };

            dbContext.Resumes.Add(resume);

            int resultCount = dbContext.SaveChanges();
            Console.WriteLine(resultCount);
            Console.WriteLine("=====================");
            Console.WriteLine("结束！！");



        }

        //查询关联数据
        static void Query_A()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            //Include 查询导航属性
            var clus = dbContext.Clubs
                      .Include(i => i.League)
                      .Include(x => x.Players)
                        .ThenInclude(y => y.Resume)
                      .Include(x => x.Players)
                        .ThenInclude(y => y.GamePlayers)
                            .ThenInclude(g=>g.Game)
                      .ToList();

            Console.WriteLine("=====================");
            Console.WriteLine("结束！！");
        }

        static void Query_B()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            //Include 查询导航属性
            var clus = dbContext.Clubs.Select(i => new
            {
                i.Id,
                LeagueName = i.League.Name,
                Players = i.Players.Where(p=>p.DateOfBirth>new DateTime(1990,1,1))
            });

            foreach (var c in clus)
            {
                foreach (var p in c.Players)
                {
                    p.Name += "~";
                }
            }
            //dbContext.SaveChanges();

            Console.WriteLine("=====================");
            Console.WriteLine("结束！！");
        }

        static void Query_C()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            //Include 查询导航属性
            var clus = dbContext.Clubs.First();

            dbContext.Entry(clus)
                .Collection(i => i.Players)
                .Load();

            dbContext.Entry(clus)
                .Reference(i => i.League)
                .Load();

            Console.WriteLine("=====================");
            Console.WriteLine("结束！！");
        }

        //修改关联属性
        static void Update_A()
        {
            using DemoDBContext dbContext = new DemoDBContext();

            var club = dbContext.Clubs
                    .Include(i => i.League)
                    .First();


            club.League.Name += "@";

            dbContext.SaveChanges();


            Console.WriteLine("=====================");
            Console.WriteLine("结束！！");
        }
    }
}
