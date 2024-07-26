#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Business.Services;
using Business.Models;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers
{
    [Authorize] // This restricts access to all actions in this controller to authenticated users only
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        // GET: Blogs
        public IActionResult Index()
        {
            List<BlogModel> blogList = _blogService.Query().ToList();
            return View(blogList);
        }

        // GET: Blogs/Details/5
        public IActionResult Details(int id)
        {
            BlogModel blog = _blogService.GetItem(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View(new BlogModel
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                blog.CreatedAt = DateTime.Now; // Ensure the CreatedAt field is set to the current time
                blog.UpdatedAt = DateTime.Now; // Ensure the UpdatedAt field is set to the current time

                Result result = _blogService.Add(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.ViewMessage = result.Message;
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            BlogModel blog = _blogService.GetItem(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                blog.UpdatedAt = DateTime.Now; // Ensure the UpdatedAt field is set to the current time

                Result result = _blogService.Update(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = blog.Id });
                }
                ViewBag.ViewMessage = result.Message;
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            BlogModel blog = _blogService.GetItem(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _blogService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Blogs/AddPost/5
        public async Task<IActionResult> AddPost(int id)
        {
            var blog = _blogService.GetItem(id);
            if (blog == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var postModel = new PostModel
            {
                BlogId = id,
                BlogName = blog.Name,
                UserId = user.Id,
                UserName = user.UserName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ViewBag.BlogName = blog.Name;
            return View("/Views/Post/AddPost.cshtml", postModel); // Specify the correct path to the view
        }

        // POST: Blogs/AddPost
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
                    return RedirectToAction(nameof(Details), new { id = post.BlogId });
                }

                ModelState.AddModelError("", result.Message);
            }
            return View("/Views/Post/AddPost.cshtml", post); // Specify the correct path to the view
        }
    }
}
