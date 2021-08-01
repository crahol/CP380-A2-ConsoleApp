using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading;
using System.ComponentModel;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");            
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers                
                .Select(b => new { Reviews = b.Reviews.Count
                                    , Average = Math.Round(b.Reviews.Average(r => r.stars), 2)
                                    , Adjust = Math.Round(ratingAdjustmentService.Adjust(b.Reviews.Average(r => r.stars) , b.Reviews.Count), 2)
                                    , Description = b.title
                })          
                .AsEnumerable()
                .OrderByDescending(b => b.Adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output                
                Console.WriteLine($"[{j+1}] {i.Reviews,8} {i.Average, -2:F2} {i.Adjust, 8:F2}      {i.Description}");
            }
        }
    }
}
