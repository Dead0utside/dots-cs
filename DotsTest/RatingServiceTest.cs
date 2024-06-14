using System;
using DotsCore.Entity;
using DotsCore.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotsTest
{
    public class RatingServiceTest
    {
         [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void CreateTest()
            {
                IRatingService service = CreateService();
                Assert.AreEqual(0, service.GetRatings().Count);
            }
            
            [TestMethod]
            public void AddTest1()
            {
                IRatingService service = CreateService();
                service.AddRating(new Rating{PlayerName = "Jaro", Rate = 50});
                
                Assert.AreEqual(1, service.GetRatings().Count);
                Assert.AreEqual("Jaro", service.GetRatings()[0].PlayerName);
            }
            
            [TestMethod]
            public void AddTest3()
            {
                IRatingService service = CreateService();
                service.AddRating(new Rating(){PlayerName = "Jaro", Rate = 76});
                service.AddRating(new Rating{PlayerName = "Peter", Rate = 99});
                service.AddRating(new Rating{PlayerName = "Jozo", Rate= 10});
                
                Assert.AreEqual(3, service.GetRatings().Count);
                
                Assert.AreEqual("Jaro", service.GetRatings()[0].PlayerName);
                Assert.AreEqual(76, service.GetRatings()[0].Rate);
                
                Assert.AreEqual("Peter", service.GetRatings()[1].PlayerName);
                Assert.AreEqual(99, service.GetRatings()[1].Rate);
                
                Assert.AreEqual("Jozo", service.GetRatings()[2].PlayerName);
                Assert.AreEqual(10, service.GetRatings()[2].Rate);
            }

            [TestMethod]
            public void OutOfRangeTest()
            {
                IRatingService service = CreateService();
                
                Assert.ThrowsException<Exception>(() => service.AddRating(new Rating {PlayerName = "Juraj", Rate = 110}));
            }
            
            [TestMethod]
            public void ResetTest()
            {
                IRatingService service = CreateService();
                
                service.AddRating(new Rating{PlayerName = "Jaro", Rate = 50});
                service.AddRating(new Rating{PlayerName = "Jozo", Rate = 100});

                service.ResetRating();

                Assert.AreEqual(0, service.GetRatings().Count);
            }

            private IRatingService CreateService()
            {
                var service = new RatingServiceEF();
                service.ResetRating();
//            return new ScoreServiceFile();
                return service;
            }
        }   
    }
}