using Business.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IBlogService
    {
        IQueryable<BlogModel> Query();
        Result Add(BlogModel model);
        Result Update(BlogModel model);
        Result Delete(int id);
        List<BlogModel> GetList();
        BlogModel GetItem(int id);
    }

    public class BlogServices : IBlogService
    {
        private readonly Db _db;

        public BlogServices(Db db)
        {
            _db = db;
        }

        public IQueryable<BlogModel> Query()
        {
            return _db.Blogs.Select(b => new BlogModel
            {
                Id = b.Id, 
                Name = b.Name,
                Description = b.Description
            });
        }

        public Result Add(BlogModel model)
        {
            var entity = new Blog
            {
                Name = model.Name?.Trim(),
                Description = model.Description?.Trim(),
            };

            _db.Blogs.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("Blog added successfully.");
        }

        public Result Update(BlogModel model)
        {
            var existingBlog = _db.Blogs.Find(model.Id);
            if (existingBlog == null)
            {
                return new ErrorResult("Blog not found!");
            }

            existingBlog.Name = model.Name?.Trim();
            existingBlog.Description = model.Description?.Trim();

            _db.SaveChanges();

            return new SuccessResult("Blog updated successfully.");
        }

        public Result Delete(int id)
        {
            var blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return new ErrorResult("Blog not found!");
            }

            _db.Blogs.Remove(blog);
            _db.SaveChanges();

            return new SuccessResult("Blog deleted successfully.");
        }

        public List<BlogModel> GetList()
        {
            return Query().ToList();
        }

        public BlogModel GetItem(int id)
        {
            return Query().SingleOrDefault(b => b.Id == id);
        }
    }

}
