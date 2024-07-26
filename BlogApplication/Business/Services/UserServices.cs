using System;
using System.Collections.Generic;
using System.Linq;
using Business.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public interface IUserService
    {
        IQueryable<UserModel> Query();
        Result Add(UserModel model);
        Result Update(UserModel model);
        Result Delete(string id); // Update to string
        List<UserModel> GetList();
        UserModel GetItem(string id); // Update to string
        int GetPostCount(string userId);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Db _db;

        public UserService(UserManager<ApplicationUser> userManager, Db db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IQueryable<UserModel> Query()
        {
            return _db.Users.Select(u => new UserModel
            {
                Id = u.Id,
                Username = u.UserName, // Correct property name
                Email = u.Email,
            });
        }

        public Result Add(UserModel model)
        {
            var entity = new ApplicationUser
            {
                UserName = model.Username?.Trim(),
                Email = model.Email?.Trim(),
            };

            var result = _userManager.CreateAsync(entity, model.Password).Result;
            if (result.Succeeded)
            {
                return new SuccessResult("User added successfully.");
            }
            return new ErrorResult(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public Result Update(UserModel model)
        {
            var existingUser = _userManager.FindByIdAsync(model.Id).Result;
            if (existingUser == null)
            {
                return new ErrorResult("User not found!");
            }

            existingUser.UserName = model.Username?.Trim();
            existingUser.Email = model.Email?.Trim();
            var result = _userManager.UpdateAsync(existingUser).Result;

            if (result.Succeeded)
            {
                return new SuccessResult("User updated successfully.");
            }
            return new ErrorResult(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public Result Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return new ErrorResult("User not found!");
            }

            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return new SuccessResult("User deleted successfully.");
            }
            return new ErrorResult(string.Concat(", ", result.Errors.Select(e => e.Description)));
        }

        public List<UserModel> GetList()
        {
            return Query().ToList();
        }

        public UserModel GetItem(string id)
        {
            return Query().SingleOrDefault(u => u.Id == id);
        }

        public int GetPostCount(string userId)
        {
            return _db.Posts.Count(p => p.UserId == userId); // Ensure UserId is compared as string
        }
    }
}
