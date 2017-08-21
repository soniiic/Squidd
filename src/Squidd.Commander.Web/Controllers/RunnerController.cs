using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Squidd.Commander.Domain.Repositories;
using Squidd.Commander.Domain.Services;
using Squidd.Commander.Web.Models.RunnerModels;

namespace Squidd.Commander.Web.Controllers
{
    public class RunnerController : Controller
    {
        private readonly RunnerService service;

        // GET: Runner
        public RunnerController(RunnerService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            var runners = service.GetAll();
            var model = new IndexPageOutputModel()
            {
                Runners = Mapper.Map<List<RunnerOutputModel>>(runners)
            };
            return View(model);
        }

        // GET: Runner/Details/5
        public ActionResult Details(long id)
        {
            return View();
        }

        // GET: Runner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Runner/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Runner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Runner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
