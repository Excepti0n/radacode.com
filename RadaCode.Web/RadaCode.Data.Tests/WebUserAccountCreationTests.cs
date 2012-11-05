using NUnit.Framework;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Repositories;
using RadaCode.Web.Data.Utils;

namespace RadaCode.Data.Tests
{
    [TestFixture]
    public class WebUserAccountCreationTests
    {
        private WebUserRepository _repo;
        private RadaCodeWebStoreContext _context;

        public WebUserAccountCreationTests()
        {
            _repo = new WebUserRepository();
        }

        //[Test]
        //public void CreateRoleAndUserTest()
        //{
        //    _repo.AddRole("Star");
        //    var user = _repo.CreateUser("Max Pavlov", Crypto.HashPassword("q1w2e3"), "max@radacode.com");

        //    _repo.AddRoleToUser(user.Id, "Star");

        //    var exists = _repo.UserExists(user);
        //    var isInRole = _repo.GetRolesForUser(user)[0].RoleName == "Star";

        //    Assert.IsTrue(exists);
        //    Assert.IsTrue(isInRole);
        //}

        //[Test]
        //public void IsUserInRole()
        //{
        //    _repo = new WebUserRepository(_context);

        //    var inRole = (_repo.GetRolesForUser("Max Pavlov")[0] != null &&
        //                  _repo.GetRolesForUser("Max Pavlov")[0].RoleName == "Star");

        //    Assert.IsTrue(inRole);
        //}
    }
}
