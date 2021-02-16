using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppointmentJournal.ViewModels;
using AppointmentJournal.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentJournal.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        public ViewResult Register(string returnUrl = "/")
        {
            return View(new RegisterViewModel() 
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // открыть транзакцию регистрации
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) 
                {
                    try
                    {
                        User user = new User { UserName = registerModel.Name, City = registerModel.City, PhoneNumber = registerModel.PhoneNumber, Email = registerModel.Email };

                        // добавляем пользователя
                        IdentityResult createUserResult = await _userManager.CreateAsync(user, registerModel.Password);

                        // если пользователь не был создан
                        if (!createUserResult.Succeeded)
                        {
                            foreach (var error in createUserResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return View(registerModel);
                        }

                        // список результатов установки ролей
                        List<IdentityResult> setRoleResultList = new List<IdentityResult>();

                        // установить роль пользователю в БД
                        switch (registerModel.UserType)
                        {
                            case UserType.Consumer:
                                setRoleResultList.Add(await _userManager.AddToRoleAsync(user, Constants.ConsumersRole));
                                break;
                            case UserType.Producer:
                                setRoleResultList.Add(await _userManager.AddToRoleAsync(user, Constants.ConsumersRole));
                                setRoleResultList.Add(await _userManager.AddToRoleAsync(user, Constants.ProducersRole));
                                break;
                            default:
                                ModelState.AddModelError(string.Empty, "Тип пользователя не указан");
                                return View(registerModel);
                        }

                        if (setRoleResultList.All(x => (x?.Succeeded ?? false)))
                        {
                            // установка куки, которое будет храниться до закрытия браузера
                            await _signInManager.SignInAsync(user, false);
                            // завершить транзакцию регистрации
                            scope.Complete();

                            return Redirect(registerModel?.ReturnUrl);
                        }
                        else
                        {
                            foreach (var setRoleResult in setRoleResultList)
                            {
                                foreach (var error in setRoleResult?.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
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

            return View(registerModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(loginModel.Name);
                
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl);
                    }
                }
            }

            ModelState.AddModelError("", "Неверное имя или пароль");
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
