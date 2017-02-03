using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using HemtentaTdd2017;
using Moq;

namespace UnitTestProjectHemtenta
{
    [TestFixture]
    public class BlogTest
    {
        private string name;
        private Blog b;
        private Mock<IAuthenticator> mockUser;

        [SetUp]
        public void SetUpBlogTest()
        {
            name = "Anna";
            b = new Blog();
            mockUser = new Mock<IAuthenticator>();
            b.Authenticator = mockUser.Object;
        } 
            

        // Söker igenom databasen efter en användare med namnet "username". 
        // Returnerar ett giltigt User-objekt om den hittade en användare, annars null.
        [Test]
        public void LoginUser_CantFindUser_UserIsNull_Throws()
        {
            mockUser.Setup(x => x.GetUserFromDatabase(It.IsAny<string>())).Returns(new User(name));
            Assert.That(() => b.LoginUser(null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void LoginUser_CorrectUsernameAndPassword_Success()
        {
            mockUser.Setup(x => x.GetUserFromDatabase(It.IsAny<string>())).Returns(new User(name));
            b.LoginUser(new User(name));
            Assert.That( b.UserIsLoggedIn, Is.True);
            mockUser.Verify(x => x.GetUserFromDatabase(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoginUser_CorrectUsernameWrongPassword_FailLogin()
        {
            User u = new User(name);
            u.Password = "hej";
            mockUser.Setup(x => x.GetUserFromDatabase(It.IsAny<string>())).Returns(u);
            
            User u2 = new User(name);
            u2.Password = "hejdå";

            b.LoginUser(u2);
            Assert.That(b.UserIsLoggedIn, Is.False);
            mockUser.Verify(x => x.GetUserFromDatabase(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LogoutUser_AlreadyLoggedIn_Success()
        {
            User u = new User(name);
            mockUser.Setup(x => x.GetUserFromDatabase(It.IsAny<string>())).Returns(u);
            b.LoginUser(u);
            b.LogoutUser(u);
            Assert.That(b.UserIsLoggedIn, Is.False);
            mockUser.Verify(x => x.GetUserFromDatabase(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LogoutUser_NotLoggedIn_Fail()
        {
            User u = new User(name);
            b.UserIsLoggedIn = false;
            b.LogoutUser(u);
            Assert.That(b.UserIsLoggedIn, Is.False);
        }



        // För att publicera en sida måste Page vara ett giltigt Page-objekt och användaren
        // måste vara inloggad. Returnerar true om det gick att publicera,
        // false om publicering misslyckades och
        // exception om Page har ett ogiltigt värde.
        [Test]
        public void PublishPage_IncorrectValues_Fail_Throws()
        {
            Page p = new Page();
            Assert.That(() => b.PublishPage(null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void PublishPage_CorrectValues_UserIsLoggedIn_Success()
        {
            Page p = new Page();
            p.Title = "Page1";
            p.Content = "Content1";

            User u = new User(name);
            mockUser.Setup(x => x.GetUserFromDatabase(It.IsAny<string>())).Returns(u);
            b.LoginUser(u);

            bool result = b.PublishPage(p);
            Assert.That(result, Is.True);
        }

        [Test]
        public void PublishPage_UserIsNotLoggedIn_Fail()
        {
            Page p = new Page();
            p.Title = "Page1";
            p.Content = "Content1";

            b.UserIsLoggedIn = false;

            bool result = b.PublishPage(p);
            Assert.That(result, Is.False);
        }

        [Test]
        public void SendEmail_IncorrectValues_Fail()
        {
            string address = "ak@gmail.com", caption = "Best email ever", body = "You have won (SEK)10 billions!";
            b.UserIsLoggedIn = false;
            Assert.That(() => b.SendEmail(null, caption, body), Is.EqualTo(0));
            Assert.That(() => b.SendEmail("", caption, body), Is.EqualTo(0));
            Assert.That(() => b.SendEmail(address, null, body), Is.EqualTo(0));
            Assert.That(() => b.SendEmail(address, "", body), Is.EqualTo(0));
            Assert.That(() => b.SendEmail(address, caption, null), Is.EqualTo(0));
            Assert.That(() => b.SendEmail(address, caption, ""), Is.EqualTo(0));
        }

        [Test]
        public void SendEmail_CorretValues_UserIsLoggedIn()
        {
            string address = "ak@gmail.com", caption = "Best email ever", body = "You have won (SEK)10 billions!";
            b.UserIsLoggedIn = true;
            Assert.That(() => b.SendEmail(address, caption, body), Is.EqualTo(1));
        }
    }
}
