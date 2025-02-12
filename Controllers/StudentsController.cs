using JsonDemo.Models;
using MDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JsonDemo.Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult ToogleSearch()
        {
            Session["ShowStudentsSearch"] = !(bool)Session["ShowStudentsSearch"];
            return RedirectToAction("Index");
        }
        public ActionResult SearchName(string name)
        {
            Session["SearchStudentName"] = name;
            return RedirectToAction("Index");
        }
        public ActionResult SearchYear(int year)
        {
            Session["SelectedStudentYear"] = year;
            return RedirectToAction("Index");
        }
        public void InitSessionVariables()
        {
            Session["id"] = 0;
            if (Session["ShowStudentsSearch"] == null)
                Session["ShowStudentsSearch"] = false;

            if (Session["SearchStudentName"] == null)
                Session["SearchStudentName"] = "";

            if (Session["SelectedStudentYear"] == null)
                Session["SelectedStudentYear"] = 0; // all years

            Session["StudentsYearsList"] = DB.Students.YearsList;
        }

        public ActionResult Index()
        {
            InitSessionVariables();
          
            string searchName = ((string)Session["SearchStudentName"]).ToLower();
            int selectedYear = (int)Session["SelectedStudentYear"];
            var students = DB.Students.ToList().OrderByDescending(m=>m.Year).ThenBy(m => m.LastName).ThenBy(m => m.FirstName).ToList();

            if ((bool)Session["ShowStudentsSearch"])
            {
                if (searchName != "")
                    students = students.Where(s => s.LastName.ToLower().StartsWith(searchName)).ToList();
                if (selectedYear != 0) 
                    students = students.Where(s => s.Year == selectedYear).ToList();
            }

            return View(students);
        }
        public ActionResult Details(int id)
        {
            Student student = DB.Students.Get(id);
            if (student != null)
            {
                Session["id"] = id;
                Session["code"] = student.Code;
                return View(student);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View(new Student());
        }
        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                DB.Students.Add(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }
        public ActionResult Edit()
        {
            int id = (int)Session["id"];
            Student student = DB.Students.Get(id);
            if (student != null)
            {
                ViewBag.Registrations = student.NextSessionCoursesToSelectList;
                ViewBag.Courses = DB.Courses.NextSessionToSelectList;
                return View(DB.Students.Get(id));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(Student student, List<int> selectedCoursesId)
        {
            if (ModelState.IsValid)
            {
                student.Id = (int)Session["id"];
                student.Code = (string)Session["code"];
                DB.Students.Update(student, selectedCoursesId);
                return RedirectToAction("Details", new { id = student.Id });
            }
            ViewBag.Registrations = student.NextSessionCoursesToSelectList;
            ViewBag.Courses = DB.Courses.NextSessionToSelectList;
            return View(student);
        }
        public ActionResult Delete()
        {
            int id = (int)Session["id"];
            DB.Students.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
