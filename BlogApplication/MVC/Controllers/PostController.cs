#nullable disable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Business.Services;
using Business.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = _blogService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var blog = _blogService.GetItem(post.BlogId);
            if (blog == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(post.UserId);
            if (user != null)
            {
                post.UserName = user.UserName;
            }

            post.BlogName = blog.Name;

            return View("/Views/Post/Edit.cshtml", post); // Specify the correct path to the view
        }

        // POST: Posts/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostModel post)
        {
            if (ModelState.IsValid)
            {
                post.UpdatedAt = DateTime.Now; // Ensure the UpdatedAt field is set to the current time
                var result = _blogService.UpdatePost(post);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Details", "Blogs", new { id = post.BlogId });
                }
                ViewBag.ViewMessage = result.Message;
            }

            var blog = _blogService.GetItem(post.BlogId);
            if (blog != null)
            {
                post.BlogName = blog.Name;
                var user = await _userManager.FindByIdAsync(post.UserId);
                if (user != null)
                {
                    post.UserName = user.UserName;
                }
            }
            return View("/Views/Post/Edit.cshtml", post); // Specify the correct path to the view
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = _blogService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var blog = _blogService.GetItem(post.BlogId);
            if (blog == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(post.UserId);
            if (user != null)
            {
                post.UserName = user.UserName;
            }

            post.BlogName = blog.Name;

            return View("/Views/Post/Delete.cshtml", post); // Specify the correct path to the view
        }

        // POST: Posts/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _blogService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var result = _blogService.DeletePost(id);
            TempData["Message"] = result.Message;
            return RedirectToAction("Details", "Blogs", new { id = post.BlogId });
        }

        // GET: Posts/AddPost
        public IActionResult AddPost(int id)
        {
            var blog = _blogService.GetItem(id);
            if (blog == null)
            {
                return NotFound();
            }
            var postModel = new PostModel { BlogId = id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            ViewBag.BlogName = blog.Name;
            return View("/Views/Post/AddPost.cshtml", postModel); // Specify the correct path to the view
        }

        // POST: Posts/AddPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(PostModel post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedAt = DateTime.Now;
                post.UpdatedAt = DateTime.Now;

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                post.UserId = user.Id;
                post.UserName = user.UserName;

                var result = _blogService.AddPostToBlog(post.BlogId, post);
                if (result.IsSuccessful)
                {
                    return RedirectToAction("Details", "Blogs", new { id = post.BlogId });
                }

                ModelState.AddModelError("", result.Message);
            }
            return View("/Views/Post/AddPost.cshtml", post); // Specify the correct path to the view
        }
    }
}
