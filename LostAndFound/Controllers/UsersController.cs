using LostAndFound.DbContexts;
using LostAndFound.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LostAndFound.Controllers
{
    public class UsersController : Controller
    {
        private readonly LostAndFoundDbContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(LostAndFoundDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return Unauthorized();

            int userId = int.Parse(userIdString);

            var user = await _context.Users
                .Include(u => u.Items)
                .Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();
            var viewModel = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Phone = user.Phone,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt,
                AvatarBase64 = user.AvatarBase64,
                City = user.City,
                ShowPhone = user.ShowPhone,
                ShowEmail = user.ShowEmail,
                Items = user.Items.ToList(),
                Claims = user.Claims.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return Unauthorized();

            int userId = int.Parse(userIdString);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            var viewModel = new EditProfileViewModel
            {
                FullName = user.FullName,
                Phone = user.Phone,
                City = user.City,
                CurrentAvatarBase64 = user.AvatarBase64,
                ShowPhone = user.ShowPhone,
                ShowEmail = user.ShowEmail
            };

            return View(viewModel);
        }

            [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return Unauthorized();

            int userId = int.Parse(userIdString);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();
            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.City = model.City;
            user.ShowPhone = model.ShowPhone;
            user.ShowEmail = model.ShowEmail;

            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await model.AvatarFile.CopyToAsync(memoryStream);
                var base64String = Convert.ToBase64String(memoryStream.ToArray());
                var contentType = model.AvatarFile.ContentType;
                user.AvatarBase64 = $"data:{contentType};base64,{base64String}";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Profile");

        }
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return Unauthorized();

            int userId = int.Parse(userIdString);
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح";
            return RedirectToAction("Profile");
        }
    }

}