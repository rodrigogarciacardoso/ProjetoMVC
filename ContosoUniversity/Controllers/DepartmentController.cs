using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Department
        public async Task<ActionResult> Index()
        {
            var departments = db.Departamentos.Include(d => d.Administrador);
            return View(await departments.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Commenting out original code to show how to use a raw SQL query.
            //Department department = await db.Departments.FindAsync(id);

            // Create and execute raw SQL query.
            string query = "SELECT * FROM Departamento WHERE DepartamentoID = @p0";
            Departamento department = await db.Departamentos.SqlQuery(query, id).SingleOrDefaultAsync();

            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instrutores, "ID", "NomeCompleto");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartamentoID,Nome,Despesas,DataInicio,InstrutorID")] Departamento department)
        {
            if (ModelState.IsValid)
            {
                db.Departamentos.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instrutores, "ID", "NomeCompleto", department.InstrutorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento department = await db.Departamentos.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instrutores, "ID", "NomeCompleto", department.InstrutorID);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Nome", "Despesas", "DataInicio", "InstrutorID", "RowVersion" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var departmentToUpdate = await db.Departamentos.FindAsync(id);
            if (departmentToUpdate == null)
            {
                Departamento deletedDepartment = new Departamento();
                TryUpdateModel(deletedDepartment, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewBag.InstructorID = new SelectList(db.Instrutores, "ID", "NomeCompleto", deletedDepartment.InstrutorID);
                return View(deletedDepartment);
            }

            if (TryUpdateModel(departmentToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(departmentToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Departamento)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Departamento)databaseEntry.ToObject();

                        if (databaseValues.Nome != clientValues.Nome)
                            ModelState.AddModelError("Nome", "Current value: "
                                + databaseValues.Nome);
                        if (databaseValues.Despesas != clientValues.Despesas)
                            ModelState.AddModelError("Despesas", "Current value: "
                                + String.Format("{0:c}", databaseValues.Despesas));
                        if (databaseValues.DataInicio != clientValues.DataInicio)
                            ModelState.AddModelError("DataInicio", "Current value: "
                                + String.Format("{0:d}", databaseValues.DataInicio));
                        if (databaseValues.InstrutorID != clientValues.InstrutorID)
                            ModelState.AddModelError("InstrutorID", "Current value: "
                                + db.Instrutores.Find(databaseValues.InstrutorID).NomeCompleto);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        departmentToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.InstructorID = new SelectList(db.Instrutores, "ID", "NomeCompleto", departmentToUpdate.InstrutorID);
            return View(departmentToUpdate);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento department = await db.Departamentos.FindAsync(id);
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Departamento department)
        {
            try
            {
                db.Entry(department).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = department.DepartamentoID });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(department);
            }
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
