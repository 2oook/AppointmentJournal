using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppointmentJournal.ViewModels;
using AppointmentJournal.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Transactions;

namespace AppointmentJournal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) 
                {
                    try
                    {
                        User user = new User { UserName = model.Name, City = model.City, PhoneNumber = model.PhoneNumber, Email = model.Email };

                        // добавляем пользователя
                        IdentityResult createUserResult = await _userManager.CreateAsync(user, model.Password);

                        // если пользователь не был создан
                        if (!createUserResult.Succeeded)
                        {
                            foreach (var error in createUserResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return View(model);
                        }

                        // результат установки роли
                        IdentityResult setRoleResult = null;

                        // установить роль пользователю в БД
                        switch (model.UserType)
                        {
                            case UserType.Consumer:
                                setRoleResult = await _userManager.AddToRoleAsync(user, "Consumers");
                                break;
                            case UserType.Producer:
                                setRoleResult = await _userManager.AddToRoleAsync(user, "Producers");
                                break;
                            default:
                                ModelState.AddModelError(string.Empty, "Тип пользователя не указан");
                                return View(model);
                        }

                        if (setRoleResult?.Succeeded ?? false)
                        {
                            // установка куки
                            await _signInManager.SignInAsync(user, false);
                            scope.Complete();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            foreach (var error in setRoleResult?.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    catch
                    {
                        scope.Dispose();
                        throw;
                    }
                }
            }

            return View(model);
        }
    }
}
