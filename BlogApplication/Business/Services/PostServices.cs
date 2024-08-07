﻿using System;
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
    public interface IPostService
    {
        IQueryable<PostModel> Query();
        Result Add(PostModel model);
        Result Update(PostModel model);
        Result Delete(int id);
        List<PostModel> GetList();
        PostModel GetItem(int id);
    }

    public class PostService : IPostService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Db _db;

        public PostService(UserManager<ApplicationUser> userManager, Db db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IQueryable<PostModel> Query()
        {
            return _db.Posts.Select(p => new PostModel
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                UserId = p.UserId,
                BlogId = p.BlogId,
                BlogName = p.Blog.Name,
                UserName = p.User.UserName, // Correct property name
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
        }

        public Result Add(PostModel model)
        {
            var entity = new Post
            {
                Title = model.Title?.Trim(),
                Content = model.Content?.Trim(),
                UserId = model.UserId,
                BlogId = model.BlogId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            _db.Posts.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("Post added successfully.");
        }

        public Result Update(PostModel model)
        {
            var existingPost = _db.Posts.Find(model.Id);
            if (existingPost == null)
            {
                return new ErrorResult("Post not found!");
            }

            existingPost.Title = model.Title?.Trim();
            existingPost.Content = model.Content?.Trim();
            existingPost.UserId = model.UserId;
            existingPost.BlogId = model.BlogId;
            existingPost.UpdatedAt = model.UpdatedAt;

            _db.SaveChanges();

            return new SuccessResult("Post updated successfully.");
        }

        public Result Delete(int id)
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

        public List<PostModel> GetList()
        {
            return Query().ToList();
        }

        public PostModel GetItem(int id)
        {
            return Query().SingleOrDefault(p => p.Id == id);
        }
    }
}
