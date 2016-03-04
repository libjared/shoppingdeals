using System;
using System.Threading.Tasks;
using NUnit.Framework;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    public partial class DealsDbTests
    {
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
            await db.RegisterUser("jared", "securepassw0rd");
        }

        [Test]
        public async Task TestUserDuplicate()
        {
            await db.RegisterUser("jared", "securepassw0rd");
            try
            {
                await db.RegisterUser("jared", "securepassw0rd");
            }
            catch (ArgumentException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            Assert.Fail("No exception thrown");
        }
    }
}
