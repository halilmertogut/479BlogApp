using System;
using System.Collections.Generic;
using System.Linq;
using Business.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;

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

        PostModel GetPostById(int id);
        Result AddPostToBlog(int blogId, PostModel post);
        Result UpdatePost(PostModel model);
        Result DeletePost(int id);
    }
    public class BlogService : IBlogService
    {

        private readonly Db _db;

        public BlogService(Db db)
        {
            _db = db;
        }

        public IQueryable<BlogModel> Query()
        {
            return _db.Blogs.Select(b => new BlogModel
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                Posts = b.Posts.Select(p => new PostModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    BlogId = p.BlogId,
                    UserId = p.UserId,
                    UserName = p.User.UserName // Assuming UserName is the username field
                }).ToList()
            });
        }

        public BlogModel GetItem(int id)
        {
            var blog = _db.Blogs
                          .Include(b => b.Posts)
                          .ThenInclude(p => p.User)
                          .Where(b => b.Id == id)
                          .Select(b => new BlogModel
                          {
                              Id = b.Id,
                              Name = b.Name,
                              Description = b.Description,
                              CreatedAt = b.CreatedAt,
                              UpdatedAt = b.UpdatedAt,
                              Posts = b.Posts.Select(p => new PostModel
                              {
                                  Id = p.Id,
                                  Title = p.Title,
                                  Content = p.Content,
                                  CreatedAt = p.CreatedAt,
                                  UpdatedAt = p.UpdatedAt,
                                  BlogId = p.BlogId,
                                  UserId = p.UserId,
                                  UserName = p.User.UserName
                              }).ToList()
                          })
                          .SingleOrDefault();

            return blog;
        }


        public Result Add(BlogModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new ErrorResult("Blog name cannot be empty!");
            }

            var existingBlog = _db.Blogs.FirstOrDefault(b => b.Name == model.Name.Trim());
            if (existingBlog != null)
            {
                return new ErrorResult("Blog with the same name already exists!");
            }

            var entity = new Blog
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
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
            existingBlog.UpdatedAt = model.UpdatedAt;

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

        public PostModel GetPostById(int id)
        {
            var post = _db.Posts
                          .Include(p => p.User)
                          .Where(p => p.Id == id)
                          .Select(p => new PostModel
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Content = p.Content,
                              CreatedAt = p.CreatedAt,
                              UpdatedAt = p.UpdatedAt,
                              BlogId = p.BlogId,
                              UserId = p.UserId,
                              UserName = p.User.UserName
                          })
                          .SingleOrDefault();

            return post;
        }

        public Result AddPostToBlog(int blogId, PostModel post)
        {
            var blog = _db.Blogs.Include(b => b.Posts).FirstOrDefault(b => b.Id == blogId);
            if (blog == null)
            {
                return new ErrorResult("Blog not found!");
            }

            var entity = new Post
            {
                Title = post.Title?.Trim(),
                Content = post.Content?.Trim(),
                UserId = post.UserId,
                BlogId = post.BlogId,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };

            _db.Posts.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("Post added to blog successfully.");
        }

        public Result UpdatePost(PostModel model)
        {
            var existingPost = _db.Posts.Find(model.Id);
            if (existingPost == null)
            {
                return new ErrorResult("Post not found!");
            }

            existingPost.Title = model.Title?.Trim();
            existingPost.Content = model.Content?.Trim();
            existingPost.UpdatedAt = model.UpdatedAt;

            _db.SaveChanges();

            return new SuccessResult("Post updated successfully.");
        }

        public Result DeletePost(int id)
        {
            var post = _db.Posts.Find(id);
            if (post == null)
            {
                return new ErrorResult("Post not found!");
            }

            _db.Posts.Remove(post);
            _db.SaveChanges();

            return new SuccessResult("Post deleted successfully.");
        }
    }
}
