using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using S2B.Models;

namespace S2B.Controllers
{
    public class FucionariosController : Controller
    {
        private bd_projeto_data db = new bd_projeto_data();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie Login_User = Request.Cookies["Login"];
            HttpCookie Login_Senha = Request.Cookies["Senha"];
            if (Login_User != null && Login_Senha != null)
            {
                string senha = Login_Senha.Value;
                string user = Login_User.Value;
                if (!(db.FUNCIONARIOs.Where(a => a.LOGIN.Equals(user) && a.SENHA.Equals(senha)).Count() == 1))
                {
                    Response.Redirect("/Home/Login");
                }
            }
            else
            {
                Response.Redirect("/Home/Login");
            }
        }

        // GET: Fucionarios
        public async Task<ActionResult> Index()
        {
            return View(await db.FUNCIONARIOs.ToListAsync());
        }

        // GET: Fucionarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FUNCIONARIO fUNCIONARIO = await db.FUNCIONARIOs.FindAsync(id);
            if (fUNCIONARIO == null)
            {
                return HttpNotFound();
            }
            return View(fUNCIONARIO);
        }

        // GET: Fucionarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fucionarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "COD_FUNCIONARIO,NOME,CPF,LOGIN,SENHA,EMAIL,ENDERECO,TELEFONE,CELULAR,TIPO")] FUNCIONARIO fUNCIONARIO)
        {
            if (ModelState.IsValid)
            {
                db.FUNCIONARIOs.Add(fUNCIONARIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fUNCIONARIO);
        }

        // GET: Fucionarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FUNCIONARIO fUNCIONARIO = await db.FUNCIONARIOs.FindAsync(id);
            if (fUNCIONARIO == null)
            {
                return HttpNotFound();
            }
            return View(fUNCIONARIO);
        }

        // POST: Fucionarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "COD_FUNCIONARIO,NOME,CPF,LOGIN,SENHA,EMAIL,ENDERECO,TELEFONE,CELULAR,TIPO")] FUNCIONARIO fUNCIONARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fUNCIONARIO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fUNCIONARIO);
        }

        // GET: Fucionarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FUNCIONARIO fUNCIONARIO = await db.FUNCIONARIOs.FindAsync(id);
            if (fUNCIONARIO == null)
            {
                return HttpNotFound();
            }
            return View(fUNCIONARIO);
        }

        // POST: Fucionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FUNCIONARIO fUNCIONARIO = await db.FUNCIONARIOs.FindAsync(id);
            db.FUNCIONARIOs.Remove(fUNCIONARIO);
            await db.SaveChangesAsync();
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
