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
    public interface ICommentService
    {
        IQueryable<CommentModel> Query();
        Result Add(CommentModel model);
        Result Update(CommentModel model);
        Result Delete(int id);
        List<CommentModel> GetList();
        CommentModel GetItem(int id);
    }

    public class CommentService : ICommentService
    {
        private readonly Db _db;

        public CommentService(Db db)
        {
            _db = db;
        }

        public IQueryable<CommentModel> Query()
        {
            return _db.Comments.Select(c => new CommentModel
            {
                Id = c.Id, 
                Content = c.Content,
                UserId = c.UserId,
                PostId = c.PostId,
            });
        }

        public Result Add(CommentModel model)
        {
            var entity = new Comment
            {
                Content = model.Content.Trim(),
                UserId = model.UserId,
                PostId = model.PostId,
            };

            _db.Comments.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("Comment added successfully.");
        }

        public Result Update(CommentModel model)
        {
            var existingEntity = _db.Comments.Find(model.Id);
            if (existingEntity == null)
                return new ErrorResult("Comment not found!");

            existingEntity.Content = model.Content.Trim();
            existingEntity.UserId = model.UserId;
            existingEntity.PostId = model.PostId;

            _db.SaveChanges();

            return new SuccessResult("Comment updated successfully.");
        }

        public Result Delete(int id)
        {
            var entity = _db.Comments.Find(id);
            if (entity == null)
                return new ErrorResult("Comment not found!");

            _db.Comments.Remove(entity);
            _db.SaveChanges();

            return new SuccessResult("Comment deleted successfully.");
        }

        public List<CommentModel> GetList() => Query().ToList();

        public CommentModel GetItem(int id) => Query().SingleOrDefault(c => c.Id == id);
    }
}
