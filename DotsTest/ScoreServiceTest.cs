using System;
using DotsCore.Entity;
using DotsCore.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotsTest
{
    [TestClass]
    public class ScoreServiceTest
    {
        [TestMethod]
        public void CreateTest()
        {
            IScoreService service = CreateService();
            Assert.AreEqual(0, service.GetTopScores().Count);
        }
        
        [TestMethod]
        public void AddTest1()
        {
            IScoreService service = CreateService();
            service.AddScore(new Score{PlayerName = "Jaro", Points = 100});
            
            Assert.AreEqual(1, service.GetTopScores().Count);
            Assert.AreEqual("Jaro", service.GetTopScores()[0].PlayerName);
            Assert.AreEqual(100, service.GetTopScores()[0].Points);
        }
        
        [TestMethod]
        public void AddTest3()
        {
            IScoreService service = CreateService();
            service.AddScore(new Score{PlayerName = "Jaro", Points = 50});
            service.AddScore(new Score{PlayerName = "Peter", Points = 100});
            service.AddScore(new Score{PlayerName = "Jozo", Points = 20});
            
            Assert.AreEqual(3, service.GetTopScores().Count);
            
            Assert.AreEqual("Peter", service.GetTopScores()[0].PlayerName);
            Assert.AreEqual(100, service.GetTopScores()[0].Points);
            
            Assert.AreEqual("Jaro", service.GetTopScores()[1].PlayerName);
            Assert.AreEqual(50, service.GetTopScores()[1].Points);
            
            Assert.AreEqual("Jozo", service.GetTopScores()[2].PlayerName);
            Assert.AreEqual(20, service.GetTopScores()[2].Points);
        }
        
        [TestMethod]
        public void AddTest5()
        {
            IScoreService service = CreateService();
            service.AddScore(new Score{PlayerName = "Zuzka", Points = 10});
            service.AddScore(new Score{PlayerName = "Jaro", Points = 50});
            service.AddScore(new Score{PlayerName = "Anca", Points = 5});
            service.AddScore(new Score{PlayerName = "Peter", Points = 100});
            service.AddScore(new Score{PlayerName = "Jozo", Points = 20});
            
            Assert.AreEqual(3, service.GetTopScores().Count);
            
            Assert.AreEqual("Peter", service.GetTopScores()[0].PlayerName);
            Assert.AreEqual(100, service.GetTopScores()[0].Points);
            
            Assert.AreEqual("Jaro", service.GetTopScores()[1].PlayerName);
            Assert.AreEqual(50, service.GetTopScores()[1].Points);
            
            Assert.AreEqual("Jozo", service.GetTopScores()[2].PlayerName);
            Assert.AreEqual(20, service.GetTopScores()[2].Points);
        }
        [TestMethod]
        public void ResetTest()
        {
            IScoreService service = CreateService();
            
            service.AddScore(new Score{PlayerName = "Jaro", Points = 50});
            service.AddScore(new Score{PlayerName = "Jozo", Points = 20});

            service.ResetScore();

            Assert.AreEqual(0, service.GetTopScores().Count);
        }

        private IScoreService CreateService()
        {
            var service = new ScoreServiceEF();
            service.ResetScore();
//            return new ScoreServiceFile();
            return service;
        }
    }
}