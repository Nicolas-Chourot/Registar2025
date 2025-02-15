using JsonDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsonDemo.Controllers
{
    public class TeachersController : Controller
    {
        public ActionResult ToogleSearch()
        {
            Session["ShowTeachersSearch"] = !(bool)Session["ShowTeachersSearch"];
            return RedirectToAction("Index");
        }
        public ActionResult SearchName(string name)
        {
            Session["SearchTeacherName"] = name;
            return RedirectToAction("Index");
        }
        
        public void InitSessionVariables()
        {
            Session["id"] = 0;
            if (Session["ShowTeachersSearch"] == null)
                Session["ShowTeachersSearch"] = false;

            if (Session["SearchTeacherName"] == null)
                Session["SearchTeacherName"] = "";

            Session["StudentsYearsList"] = DB.Students.YearsList;
        }

        public ActionResult Index()
        {
            InitSessionVariables();

            string searchName = ((string)Session["SearchTeacherName"]).ToLower();
            var teachers = DB.Teachers.ToList().OrderBy(m => m.LastName).ThenBy(m => m.FirstName).ToList();

            if ((bool)Session["ShowTeachersSearch"])
            {
                if (searchName != "")
                    teachers = teachers.Where(s => s.LastName.ToLower().StartsWith(searchName)).ToList();
               
            }

            return View(teachers);
        }
        public ActionResult Details(int id)
        {
            Teacher teacher = DB.Teachers.Get(id);
            if (teacher != null)
            {
                string d = teacher.StartDate.ToFrenchDateString();
                Session["id"] = id;
                Session["code"] = teacher.Code;
                return View(teacher);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View(new Teacher());
        }
        [HttpPost]
        public ActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                DB.Teachers.Add(teacher);
                return RedirectToAction("Index");
            }
            return View(teacher);
        }
        public ActionResult Edit()
        {
            int id = (int)Session["id"];
            Teacher teacher = DB.Teachers.Get(id);
            if (teacher != null)
            {
                ViewBag.Allocations = teacher.NextSessionCoursesToSelectList;
                // Todo prevent from allocatation of 2 teachers on at the same next session course
                ViewBag.Courses = DB.Courses.NextSessionUnAllocatedToSelectList;
                return View(DB.Teachers.Get(id));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(Teacher teacher, List<int> selectedCoursesId)
        {
            if (ModelState.IsValid)
            {
                teacher.Id = (int)Session["id"];
                teacher.Code = (string)Session["code"];
                DB.Teachers.Update(teacher, selectedCoursesId);
                return RedirectToAction("Details", new { id = teacher.Id });

            }
            ViewBag.Allocations = teacher.NextSessionCoursesToSelectList;
            // Todo prevent from allocatation of 2 teachers on at the same next session course
            ViewBag.Courses = DB.Courses.NextSessionUnAllocatedToSelectList;
            return View(teacher);
        }
        public ActionResult Delete()
        {
            int id = (int)Session["id"];
            DB.Teachers.Delete(id);
            return RedirectToAction("Index");
        }
    }
}