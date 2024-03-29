﻿using AppointmentJournal.Controllers;
using AppointmentJournal.ViewModels;
using AppointmentJournal.AppDatabase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AppointmentJournal.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public async void Registration_Transaction_Completed()
        {
            // Arrange
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockUserManager = 
                new Mock<UserManager<User>>(
                    mockIUserStore.Object, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null);

            // подмена метода создания пользователя
            mockUserManager.Setup(m => 
                m.CreateAsync(
                    It.IsAny<User>(), 
                    It.IsAny<string>())).
                    ReturnsAsync(new DeriveIdentityResult() { SetSucceeded = true });
            // подмена метода назначения роли для пользователя
            mockUserManager.Setup(m => 
                m.AddToRoleAsync(
                    It.IsAny<User>(), 
                    It.IsAny<string>()))
                    .ReturnsAsync(new DeriveIdentityResult() { SetSucceeded = true });

            var mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockIUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSignInManager = new Mock<SignInManager<User>>(
                mockUserManager.Object, 
                mockIHttpContextAccessor.Object, 
                mockIUserClaimsPrincipalFactory.Object, 
                null, 
                null, 
                null, 
                null);

            // подмена метода входа пользователя (установки куки)
            mockSignInManager.Setup(m => m.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<string>()));

            // создать объект контроллера
            var target = new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Action
            var result = await target.Register(new RegisterViewModel() 
            { 
                Name = "user", City = "city", 
                PhoneNumber = "55555", 
                Email = "test@mail.com",
                Password = "Qaz123@", 
                PasswordConfirm = "Qaz123@", 
                UserType = UserType.Consumer, 
                ReturnUrl = "/" 
            });

            // Assert
            Assert.True(result is RedirectResult);
            // если был вызван этот метод - значит транзакция была зафиксирована
            mockSignInManager.Verify(m => 
                m.SignInAsync(
                    It.IsAny<User>(), 
                    It.IsAny<bool>(), 
                    null), 
                Times.Once);

            //Assert.Equal("city", viewModel.City);
            //Assert.Equal("user", viewModel.Name);
            //Assert.Equal("55555", viewModel.PhoneNumber);
            //Assert.Equal("test@mail.com", viewModel.Email);
            //Assert.Equal("Qaz123@", viewModel.Password);
            //Assert.Equal("Qaz123@", viewModel.PasswordConfirm); 
            //Assert.Equal(UserType.Consumer, viewModel.UserType);
            //Assert.Equal("/", viewModel.ReturnUrl);
        }

        [Fact]
        public async void Registration_Transaction_Uncompleted()
        {
            // Arrange
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockUserManager = 
                new Mock<UserManager<User>>(
                    mockIUserStore.Object, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null);

            // подмена метода создания пользователя
            mockUserManager.Setup(m => 
                m.CreateAsync(
                    It.IsAny<User>(), 
                    It.IsAny<string>())).
                    ReturnsAsync(new DeriveIdentityResult() { SetSucceeded = true });
            // подмена метода назначения роли для пользователя
            mockUserManager.Setup(m => 
                m.AddToRoleAsync(
                    It.IsAny<User>(), 
                    It.IsAny<string>())).
                    ReturnsAsync(new DeriveIdentityResult() { SetSucceeded = true });

            var mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockIUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSignInManager = 
                new Mock<SignInManager<User>>(
                    mockUserManager.Object, 
                    mockIHttpContextAccessor.Object, 
                    mockIUserClaimsPrincipalFactory.Object, 
                    null, 
                    null, 
                    null, 
                    null);
                    
            // подмена метода входа пользователя (установки куки)
            mockSignInManager.Setup(m => 
                m.SignInAsync(
                    It.IsAny<User>(), 
                    It.IsAny<bool>(), 
                    It.IsAny<string>()));

            // создать объект контроллера
            var target = new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Action
            IActionResult result = await target.Register(new RegisterViewModel() 
            { 
                Name = "user", 
                City = "city", 
                PhoneNumber = "55555", 
                Email = "test@mail.com", 
                Password = "Qaz123@", 
                PasswordConfirm = "Qaz123@", 
                UserType = (UserType)10, // ошибочное значение
                ReturnUrl = "/" 
            });

            // Assert
            Assert.True(!(result is RedirectResult));

            mockSignInManager.Verify(m => 
                m.SignInAsync(
                    It.IsAny<User>(), 
                    It.IsAny<bool>(), 
                    null), 
                Times.Never);

            //Assert.Equal("city", viewModel.City);
            //Assert.Equal("user", viewModel.Name);
            //Assert.Equal("55555", viewModel.PhoneNumber);
            //Assert.Equal("test@mail.com", viewModel.Email);
            //Assert.Equal("Qaz123@", viewModel.Password);
            //Assert.Equal("Qaz123@", viewModel.PasswordConfirm); 
            //Assert.Equal(UserType.Consumer, viewModel.UserType);
            //Assert.Equal("/", viewModel.ReturnUrl);
        }

        // Производный класс IdentityResult для установки свойства Succeeded
        // вариант подмены класса IdentityResult, если не использовать Mock
        class DeriveIdentityResult : IdentityResult
        {
            private bool _SetSucceeded;

            public bool SetSucceeded
            {
                get { return _SetSucceeded; }
                set 
                {
                    // установить свойство с модификатором protected
                    Succeeded = value;
                    _SetSucceeded = value; 
                }
            }
        }
    }
}
