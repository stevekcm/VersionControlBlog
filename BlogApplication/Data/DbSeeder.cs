using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Data
{
    public static class DbSeeder
    {
        private static readonly string _password = "Password123!";

        /// <summary>
        /// This method see the roles, role claims, and users
        /// </summary>
        /// <param name="userManager">UserManager</param>
        /// <param name="roleManager">RoleManager</param>
        public static void SeedDb(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedRoleClaim(roleManager);
            SeedUsers(userManager, roleManager);

        }

        /// <summary>
        /// This method is for seeding user in the database with email, role , and the claim
        /// </summary>
        /// <param name="userManager">User Manager</param>
        /// <param name="roleManager">Role Manager</param>
        /// <param name="email">Email</param>
        /// <param name="role">Role</param>
        /// <returns></returns>
        public static async Task AddUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                IdentityUser identityUser = new IdentityUser()
                {
                    UserName = email,
                    Email = email,
                };
                await userManager.CreateAsync(identityUser, _password);
                await AddUserToRole(identityUser, userManager, role);
                await SeedClaim(identityUser, roleManager, userManager, role);
            }
        }

        /// <summary>
        /// Seeding Users in database
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public static void SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            AddUser(userManager, roleManager, "Member1@email.com", "Admin").Wait();
            AddUser(userManager, roleManager, "Customer1@email.com", "Member").Wait();
            AddUser(userManager, roleManager, "Customer2@email.com", "Member").Wait();
            AddUser(userManager, roleManager, "Customer3@email.com", "Member").Wait();
            AddUser(userManager, roleManager, "Customer4@email.com", "Member").Wait();
            AddUser(userManager, roleManager, "Customer5@email.com", "Member").Wait();

        }

        /// <summary>
        /// This method is making a role
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static async Task AddRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                IdentityRole identityRole = new IdentityRole(role);
                roleManager.CreateAsync(identityRole).Wait();
            }
        }

        /// <summary>
        /// Setting roles
        /// </summary>
        /// <param name="roleManager"></param>
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            AddRole(roleManager, "Admin").Wait();
            AddRole(roleManager, "Member").Wait();
        }

        /// <summary>
        /// Adding claim to user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userManager"></param>
        /// <param name="claim"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task AddClaim(IdentityUser user, UserManager<IdentityUser> userManager, string claim, string value)
        {
            var storedClaim = await userManager.GetClaimsAsync(user);
            var claimCheck = storedClaim.FirstOrDefault(c => c.Type == claim);

            if(claimCheck == null)
            {
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(claim, value));
            }
        }

        /// <summary>
        /// Adding claims to role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="roleManager"></param>
        /// <param name="claim"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task AddClaim(IdentityRole role, RoleManager<IdentityRole> roleManager, string claim, string value)
        {
            var storedClaim = await roleManager.GetClaimsAsync(role);
            var claimCheck = storedClaim.FirstOrDefault(c => c.Type == claim);

            if (claimCheck == null)
            {
                await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(claim, value));
            }
        }

        /// <summary>
        /// Adding the role claims
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task AddRoleClaim(RoleManager<IdentityRole> roleManager, string role, string claim, string value)
        {
            var roleCheck = await roleManager.FindByNameAsync(role);
            if(roleCheck == null)
            {
                await AddRole(roleManager, role);
            }

            await AddClaim(roleCheck, roleManager, claim, value);
        }

        /// <summary>
        /// Seeding permissions
        /// </summary>
        /// <param name="roleManager"></param>
        public static void SeedRoleClaim(RoleManager<IdentityRole> roleManager)
        {
            AddRoleClaim(roleManager, "Admin", "Create Post", "accepted").Wait();
            AddRoleClaim(roleManager, "Admin", "Panel", "accepted").Wait();
            AddRoleClaim(roleManager, "Admin", "View Post", "accepted").Wait();
            AddRoleClaim(roleManager, "Admin", "Edit Post", "accepted").Wait();
            AddRoleClaim(roleManager, "Admin", "Comment", "accepted").Wait();
            AddRoleClaim(roleManager, "Admin", "Remove Post", "accepted").Wait();

            AddRoleClaim(roleManager, "Member", "View Post", "accepted").Wait();
            AddRoleClaim(roleManager, "Member", "Comment", "accepted").Wait();
        }

        /// <summary>
        /// Adding user to role
        /// </summary>
        /// <param name="identityUser"></param>
        /// <param name="userManager"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static async Task AddUserToRole(IdentityUser identityUser, UserManager<IdentityUser> userManager, string role)
        {
            if(!await userManager.IsInRoleAsync(identityUser, role))
            {
                await userManager.AddToRoleAsync(identityUser, role);
            }
        }

        /// <summary>
        /// Seed the claims
        /// </summary>
        /// <param name="identityUser"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static async Task SeedClaim(IdentityUser identityUser, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var storedClaims = await roleManager.GetClaimsAsync(role);
            foreach(var item in storedClaims)
            {
                await AddClaim(identityUser, userManager, item.Type, item.Value);
            }
        }
    }
}
