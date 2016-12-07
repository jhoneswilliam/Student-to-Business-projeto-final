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
    public class ProdutosController : Controller
    {
        private bd_projeto_data db = new bd_projeto_data();
        //sessao
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

        // GET: Produto
        public async Task<ActionResult> Index()
        {
            return View(await db.PRODUTOes.ToListAsync());
        }

        // GET: Produto/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUTO pRODUTO = await db.PRODUTOes.FindAsync(id);
            if (pRODUTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUTO);
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "COD_PRODUTO,COD_FUNCIONARIO,NOME,PRECO,QUANTIDADE")] PRODUTO pRODUTO)
        {
            if (ModelState.IsValid)
            {
                db.PRODUTOes.Add(pRODUTO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pRODUTO);
        }

        // GET: Produto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUTO pRODUTO = await db.PRODUTOes.FindAsync(id);
            if (pRODUTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUTO);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "COD_PRODUTO,COD_FUNCIONARIO,NOME,PRECO,QUANTIDADE")] PRODUTO pRODUTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRODUTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pRODUTO);
        }

        // GET: Produto/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUTO pRODUTO = await db.PRODUTOes.FindAsync(id);
            if (pRODUTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUTO);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PRODUTO pRODUTO = await db.PRODUTOes.FindAsync(id);
            db.PRODUTOes.Remove(pRODUTO);
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
