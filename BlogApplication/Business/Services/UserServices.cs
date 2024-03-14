using Business.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Results.Bases;
using DataAccess.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserService
    {
        IQueryable<UserModel> Query();
        Result Add(UserModel model);
        Result Update(UserModel model);
        Result Delete(int id);
        List<UserModel> GetList();
        UserModel GetItem(int id);
    }

    public class UserService : IUserService
    {
        private readonly Db _db;

        public UserService(Db db)
        {
            _db = db;
        }

        public IQueryable<UserModel> Query()
        {
            return _db.Users.Select(u => new UserModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                
            });
        }

        public Result Add(UserModel model)
        {
            var entity = new User
            {
                Username = model.Username?.Trim(),
                Email = model.Email?.Trim(),

            };

            _db.Users.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("User added successfully.");
        }

        public Result Update(UserModel model)
        {
            var existingUser = _db.Users.Find(model.Id);
            if (existingUser == null)
            {
                return new ErrorResult("User not found!");
            }

            existingUser.Username = model.Username?.Trim();
            existingUser.Email = model.Email?.Trim();


            _db.SaveChanges();

            return new SuccessResult("User updated successfully.");
        }

        public Result Delete(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return new ErrorResult("User not found!");
            }

            _db.Users.Remove(user);
            _db.SaveChanges();

            return new SuccessResult("User deleted successfully.");
        }

        public List<UserModel> GetList()
        {
            return Query().ToList();
        }

        public UserModel GetItem(int id)
        {
            return Query().SingleOrDefault(u => u.Id == id);
        }

        public int GetPostCount(int userId)
        {
         
            return _db.Posts.Count(p => p.UserId == userId);
        }
    }
}
