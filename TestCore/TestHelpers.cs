using Grocery.Core.Helpers;
using NUnit.Framework;   // NUnit attributen zoals [Test], [SetUp], Assert

namespace TestCore
{
    public class TestHelpers
    {
        [SetUp]
        public void Setup()
        {
            // Setup-code indien nodig
        }

        // ✅ Happy flow (bekende wachtwoorden + hashes moeten slagen)
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";

            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        // ❌ Unhappy flow: verkeerde wachtwoord → false
        [Test]
        public void TestPasswordHelperReturnsFalse_OnWrongPassword()
        {
            string wrongPassword = "wrongpass";
            string correctHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";

            Assert.IsFalse(PasswordHelper.VerifyPassword(wrongPassword, correctHash));
        }

        // ❌ Unhappy flow: ongeldig hash-formaat → exception
        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.WRONG_HASH_123")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.INCORRECT_HASH_456")]
        public void TestPasswordHelperThrowsFormatException_OnInvalidHash(string password, string passwordHash)
        {
            Assert.Throws<System.FormatException>(() =>
                PasswordHelper.VerifyPassword(password, passwordHash));
        }
    }
}
