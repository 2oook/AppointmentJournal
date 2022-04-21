using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppointmentJournal.ViewModels;
using AppointmentJournal.AppDatabase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentJournal.Controllers
{
    /// <summary>
    /// Account managment controller
    /// </summary>
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
        public ViewResult Register(string returnUrl = PathConstants.RootPath)
        {
            return View(new RegisterViewModel
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
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var user = new User 
                { 
                    UserName = registerModel.Name, 
                    City = registerModel.City, 
                    PhoneNumber = registerModel.PhoneNumber, 
                    Email = registerModel.Email 
                };

                IdentityResult createUserResult = await _userManager.CreateAsync(user, registerModel.Password);

                if (!createUserResult.Succeeded)
                {
                    AddErrorsToModelState(createUserResult);
                    return View(registerModel);
                }

                if(!Enum.IsDefined<UserType>(registerModel.UserType))
                {
                    ModelState.AddModelError(string.Empty, MessageConstants.UserTypeIsNotDefined);
                    return View(registerModel);
                }

                IEnumerable<IdentityResult> setRoleResultList = await SetRolesToUser(registerModel.UserType, user);             

                if (!setRoleResultList.All(x => (x?.Succeeded ?? false)))
                {
                    foreach (var setRoleResult in setRoleResultList)
                    {
                        AddErrorsToModelState(setRoleResult);
                    }    

                    return View(registerModel);                    
                }

                await _signInManager.SignInAsync(user, false);             
                scope.Complete();

                return Redirect(registerModel?.ReturnUrl);   
            }

            return View(registerModel);
        }

        private async Task<IEnumerable<IdentityResult>> SetRolesToUser(UserType userType, User user) 
        {
            var setRoleResultList = new List<IdentityResult>();

            switch (userType)
            {
                case UserType.Consumer:
                    setRoleResultList.Add(await _userManager.AddToRoleAsync(user, DatabaseConstants.ConsumersRole));
                    break;
                case UserType.Producer:
                    setRoleResultList.Add(await _userManager.AddToRoleAsync(user, DatabaseConstants.ConsumersRole));
                    setRoleResultList.Add(await _userManager.AddToRoleAsync(user, DatabaseConstants.ProducersRole));
                    break;
                default:
                    throw new Exception($"{nameof(UserType)} = {userType} is not defined");
            }

            return setRoleResultList;
        }

        private void AddErrorsToModelState(IdentityResult identityResult)
        {
            foreach (var error in identityResult?.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl = PathConstants.RootPath)
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
                ModelState.AddModelError(string.Empty, MessageConstants.LoginFailed);
                return View(loginModel);
            }

            User user = await _userManager.FindByNameAsync(loginModel.Name);
            
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, MessageConstants.UserIsNotFound);
                return View(loginModel);
            }

            await _signInManager.SignOutAsync();
            var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, MessageConstants.LoginFailed);
                return View(loginModel);
            }

            return Redirect(loginModel?.ReturnUrl);
        }

        public async Task<RedirectResult> Logout(string returnUrl = PathConstants.RootPath)
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var roles = await _userManager.GetRolesAsync(user);
                var rolesList = roles.ToList();

                UserType userType;

                if (rolesList.Contains(DatabaseConstants.ProducersRole) & rolesList.Contains(DatabaseConstants.ConsumersRole))
                {
                    userType = UserType.Producer;
                }
                else if (rolesList.Contains(DatabaseConstants.ConsumersRole))
                {
                    userType = UserType.Consumer;
                }
                else
                {
                    userType = UserType.None;
                }

                var userProfileViewModel = new UserProfileViewModel()
                {
                    Name = user.UserName,
                    City = user.City,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserType = userType
                };

                return View(userProfileViewModel);
            }
            catch (Exception)
            {
                // TODO : log an error
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Account/AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
