#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Entities;
using Business.Services;
using Business.Models;
using DataAccess.Results.Bases;

//Generated from Custom Template.
namespace MVC.Controllers
{
    public class BlogsController : Controller
    {
        // TODO: Add service injections here
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET: Blogs
        public IActionResult Index()
        {
            List<BlogModel> blogList = _blogService.Query().ToList(); // TODO: Add get collection service logic here
            return View(blogList);
        }

        // GET: Blogs/Details/5
        public IActionResult Details(int id)
        {
            BlogModel blog = _blogService.Query().SingleOrDefault(s => s.Id == id); // TODO: Add get item service logic here
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                Result result = _blogService.Add(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                // TODO: Add insert service logic here
                // ViewData["ViewMessage"] = result.Message;
                ViewBag.ViewMessage = result.Message;
            }
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public IActionResult Edit(int id)
        {
            BlogModel blog = _blogService.Query().SingleOrDefault(s => s.Id == id); // TODO: Add get item service logic here
            if (blog == null)
            {
                return NotFound();
            }
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View(blog);
        }

        // POST: Blogs/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update service logic here
                Result result = _blogService.Update(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
					return RedirectToAction(nameof(Details), new {id = blog.Id});

				}
				ViewBag.ViewMessage = result.Message;

			}
			// TODO: Add get related items service logic here to set ViewData if necessary
			return View(blog);
        }

        // GET: Blogs/Delete/5
        public IActionResult Delete(int id)
        {
            BlogModel blog = _blogService.Query().SingleOrDefault(s => s.Id == id); // TODO: Add get item service logic here
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _blogService.Delete(id);
            TempData["Message"] = result.Message;
			return RedirectToAction(nameof(Index));

			// TODO: Add delete service logic here
		}
	}
}
