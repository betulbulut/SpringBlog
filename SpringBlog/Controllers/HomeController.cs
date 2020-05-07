﻿using SpringBlog.Models;
using SpringBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace SpringBlog.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string q, int? cid, int page=1)
        {
            var pageSize = 10;

            IQueryable<Post> posts = db.Posts;
            Category category = null;
            if (q!=null)
            {
                posts = posts.Where(x => x.Category.CategoryName.Contains(q) || x.Title.Contains(q)
                   || x.Content.Contains(q));           
            }

            if (cid!=null && q==null)
            {
                category = db.Categories.Find(cid);
                if (category==null)
                {
                    return HttpNotFound();
                }
                posts = posts.Where(x => x.CategoryId == cid);
            }
            var vm = new HomeIndexViewModel
            {
                Posts = db.Posts.OrderByDescending(x => x.CreateTime).ToPagedList(page,pageSize),
                Category=category,
                SearchTerm=q,
                CategoryId = cid

             };

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CategoriesPartial()
        {
            var cats = db.Categories.OrderBy(x => x.CategoryName).ToList();
           return PartialView("_CategoriesPartial",cats);
        }

         
    }
}