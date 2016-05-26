﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MobileSecondHand.DB.Models;
using MobileSecondHand.DB.Models.Authentication;

namespace MobileSecondHand.API.Services.Authentication {
	public class ApplicationUserManager : IApplicationUserManager {
		UserManager<ApplicationUser> userManager;
		public ApplicationUserManager(UserManager<ApplicationUser> userManager) {
			this.userManager = userManager;
		}

		public async Task<ApplicationUser> GetUser(string email) {
			return await userManager.FindByEmailAsync(email);
		}

		public Task<IdentityResult> CreateAsync(ApplicationUser user) {
			return userManager.CreateAsync(user);
		}
		public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password) {
			return await userManager.CreateAsync(user, password);
		}


		public Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo userLoginInfo) {
			return userManager.AddLoginAsync(user, userLoginInfo);
		}

		public async Task<bool> PasswordIsValid(ApplicationUser user, string password) {
			return await userManager.CheckPasswordAsync(user, password);
		}


	}
}
