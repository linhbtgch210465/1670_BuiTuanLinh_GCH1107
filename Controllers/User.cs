using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{

    public class User : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public User(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // Lấy danh sách tất cả người dùng (bao gồm cả người dùng đang đăng nhập)
            var allUsers = _userManager.Users.ToList();

            return View(allUsers);
        }
    }

}