using DotsCore.Entity;
using DotsCore.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotsTest
{
    public class CommentServiceTest
    {
         [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void CreateTest()
            {
                ICommentService service = CreateService();
                Assert.AreEqual(0, service.GetComments().Count);
            }
            
            [TestMethod]
            public void AddTest1()
            {
                ICommentService service = CreateService();
                service.AddComment(new Comment{PlayerName = "Jaro", Text = "Cool game!"});
                
                Assert.AreEqual(1, service.GetComments().Count);
                Assert.AreEqual("Jaro", service.GetComments()[0].PlayerName);
                Assert.AreEqual("Cool game!", service.GetComments()[0].Text);
            }
            
            [TestMethod]
            public void AddTest3()
            {
                ICommentService service = CreateService();
                service.AddComment(new Comment(){PlayerName = "Jaro", Text = "Cool game!"});
                service.AddComment(new Comment{PlayerName = "Peter", Text = "Fantastic!"});
                service.AddComment(new Comment{PlayerName = "Jozo", Text= "It's fine, could be better. Lots of bugs"});
                
                Assert.AreEqual(3, service.GetComments().Count);
                
                Assert.AreEqual("Jaro", service.GetComments()[0].PlayerName);
                Assert.AreEqual("Cool game!", service.GetComments()[0].Text);
                
                Assert.AreEqual("Peter", service.GetComments()[1].PlayerName);
                Assert.AreEqual("Fantastic!", service.GetComments()[1].Text);
                
                Assert.AreEqual("Jozo", service.GetComments()[2].PlayerName);
                Assert.AreEqual("It's fine, could be better. Lots of bugs", service.GetComments()[2].Text);
            }
            
            [TestMethod]
            public void ResetTest()
            {
                ICommentService service = CreateService();
                
                service.AddComment(new Comment{PlayerName = "Jaro", Text = "It's okay"});
                service.AddComment(new Comment{PlayerName = "Jozo", Text = "Will do"});

                service.ResetComment();

                Assert.AreEqual(0, service.GetComments().Count);
            }

            private ICommentService CreateService()
            {
                var service = new CommentServiceEF();
                service.ResetComment();
//            return new ScoreServiceFile();
                return service;
            }
        }   
    }
}