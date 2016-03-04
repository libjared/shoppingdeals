using System;
using System.Threading.Tasks;
using NUnit.Framework;
using ShoppingDeals.Controllers;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    public partial class DealsDbTests
    {
        private async Task RegUserTestA()
        {
            await db.RegisterUser("jared", "securepassw0rd");
        }

        private async Task<Guid> LoginUserTestA()
        {
            return await db.LoginUser("jared", "securepassw0rd");
        }

        [Test]
        public void TestUserEquality()
        {
            var u1 = new User("bla", "pass");
            var u2 = new User("bla", "pass");
            var u3 = new User("notbla", "notpass");
            Assert.That(u1, Is.EqualTo(u2));
            Assert.That(u1, Is.Not.EqualTo(u3));
        }

        [Test]
        public async Task TestUserRegister()
        {
            await RegUserTestA();
        }

        [Test]
        public async Task TestUserDuplicate()
        {
            await RegUserTestA();
            try
            {
                await RegUserTestA();
            }
            catch (AlreadyExistsException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            Assert.Fail("No exception thrown");
        }

        [Test]
        public async Task TestLogin()
        {
            await RegUserTestA();
            var apikey = await LoginUserTestA();
            Assert.That(apikey, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task TestLoginDuplicate()
        {
            await RegUserTestA();
            var apikey1 = await LoginUserTestA();
            var apikey2 = await LoginUserTestA(); //login again, making a new key
            Assert.That(apikey1, Is.Not.EqualTo(apikey2));
        }
    }
}
