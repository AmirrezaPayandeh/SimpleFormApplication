using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

using SimpleApplication.ViewModels;
using SimpleApplication.Helpers;

namespace SimpleApplication.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration m_Config;
        CommonHelper m_Helper;

        public UserController(IConfiguration config)
        {
            m_Config = config;
            m_Helper = new CommonHelper(m_Config);
        }

        // Registeration
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserInfoViewModel vm)
        {
            // User must provide at least a family name to be able to register
            if (vm.FamilyName is null) return View();

            string Query = $"INSERT INTO [UserInformation](FirstName,FamilyName,DateOfBirth,TelephoneNumber,Address,IsMale)VALUES('{vm.FirstName}','{vm.FamilyName}','{vm.DateOfBirth}','{vm.TelephoneNumber}','{vm.Address}','{vm.IsMale}');";

            m_Helper.ExecuteQuery(Query);
            return RedirectToAction("ListUsers", "User");
        }

        // Listing Users
        [HttpGet]
        public IActionResult ListUsers()
        {
            string Query = $"SELECT * FROM [UserInformation];";
            List<UserInfoViewModel> Users = m_Helper.SelectQuery(Query);
            return View(Users);
        }

        // Deleting Users
        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            string Query = $"SELECT * FROM [UserInformation] WHERE Id='{id}';";
            UserInfoViewModel User = m_Helper.SelectUser(Query);

            return View(User);
        }
        [HttpPost]
        public IActionResult DeleteUser(int id, string unusedVal = "")
        {
            string Query = $"DELETE FROM [UserInformation] WHERE Id='{id}';";

            int Result = m_Helper.ExecuteQuery(Query);
            if (Result > 0)
            {
                return RedirectToAction("ListUsers", "User");
            }
            return View(id);
        }

        // Editing Users
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            string Query = $"SELECT * FROM [UserInformation] WHERE Id='{id}';";
            UserInfoViewModel User = m_Helper.SelectUser(Query);

            return View(User);
        }
        [HttpPost]
        public IActionResult EditUser(UserInfoViewModel vm)
        {
            // Users can't erase their family name as it is the minimum requirement
            if (vm.FamilyName is null) return View(vm);

            string Query = $"UPDATE [UserInformation] SET FirstName='{vm.FirstName}', FamilyName='{vm.FamilyName}', DateOfBirth='{vm.DateOfBirth}', TelephoneNumber='{vm.TelephoneNumber}', Address='{vm.Address}', IsMale='{vm.IsMale}' WHERE id='{vm.Id}';";

            int Result = m_Helper.ExecuteQuery(Query);
            if (Result > 0)
            {
                return RedirectToAction("ListUsers", "User");
            }
            return View(vm);
        }

        // User Details
        [HttpGet]
        public IActionResult UserDetails(int id)
        {
            string Query = $"SELECT * FROM [UserInformation] WHERE Id='{id}';";
            UserInfoViewModel User = m_Helper.SelectUser(Query);

            return View(User);
        }

        // Delete All Users
        [HttpGet]
        public IActionResult DeleteAllUsers()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteAllUsers(string unusedVal = "")
        {
            string Query = "TRUNCATE TABLE [UserInformation];";
            int Result = m_Helper.ExecuteQuery(Query);

            return RedirectToAction("ListUsers", "User");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
