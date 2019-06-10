using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructor
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();

            viewModel.Instructors = db.Instrutores
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Cursos.Select(c => c.Departamento))
                .OrderBy(i => i.UltimoNome);

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single().Cursos;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                var selectedCourse = viewModel.Courses.Where(x => x.CursoID == courseID).Single();
                db.Entry(selectedCourse).Collection(x => x.Matriculas).Load();
                foreach (Matricula enrollment in selectedCourse.Matriculas)
                {
                    db.Entry(enrollment).Reference(x => x.Aluno).Load();
                }

                viewModel.Enrollments = selectedCourse.Matriculas;
            }

            return View(viewModel);
        }


        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrutor instructor = db.Instrutores.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        public ActionResult Create()
        {
            var instructor = new Instrutor
            {
                Cursos = new List<Curso>()
            };
            PopulateAssignedCourseData(instructor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UltimoNome,PrimeiroNome,DataContratacao,OfficeAssignment")]Instrutor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Cursos = new List<Curso>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Cursos.Find(int.Parse(course));
                    instructor.Cursos.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Instrutores.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }


        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrutor instructor = db.Instrutores
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Cursos)
                .Where(i => i.ID == id)
                .Single();
            PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instrutor instructor)
        {
            var allCourses = db.Cursos;
            var instructorCourses = new HashSet<int>(instructor.Cursos.Select(c => c.CursoID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CursoID,
                    Title = course.Titulo,
                    Assigned = instructorCourses.Contains(course.CursoID)
                });
            }
            ViewBag.Courses = viewModel;
        }
        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instrutores
               .Include(i => i.OfficeAssignment)
               .Include(i => i.Cursos)
               .Where(i => i.ID == id)
               .Single();

            if (TryUpdateModel(instructorToUpdate, "",
               new string[] { "UltimoNome","PrimeiroNome","DataContratacao","OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Localizacao))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }
        private void UpdateInstructorCourses(string[] selectedCourses, Instrutor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Cursos = new List<Curso>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Cursos.Select(c => c.CursoID));
            foreach (var course in db.Cursos)
            {
                if (selectedCoursesHS.Contains(course.CursoID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CursoID))
                    {
                        instructorToUpdate.Cursos.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CursoID))
                    {
                        instructorToUpdate.Cursos.Remove(course);
                    }
                }
            }
        }



        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrutor instructor = db.Instrutores.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instrutor instructor = db.Instrutores
              .Include(i => i.OfficeAssignment)
              .Where(i => i.ID == id)
              .Single();

            instructor.OfficeAssignment = null;
            db.Instrutores.Remove(instructor);

            var department = db.Departamentos
                .Where(d => d.InstrutorID == id)
                .SingleOrDefault();
            if (department != null)
            {
                department.InstrutorID = null;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
