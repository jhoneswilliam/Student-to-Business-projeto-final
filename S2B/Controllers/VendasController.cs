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
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace S2B.Controllers
{
    public class VendasController : Controller
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


        // GET: Vendas
        public async Task<ActionResult> Index()
        {
            var vENDAs = db.VENDAs.Include(v => v.FUNCIONARIO).Include(v => v.PRODUTO);
            return View(await vENDAs.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDA vENDA = await db.VENDAs.FindAsync(id);
            if (vENDA == null)
            {
                return HttpNotFound();
            }
            return View(vENDA);
        }

        // GET: Vendas/Create
        public ActionResult Create()
        {
            ViewBag.COD_FUNCIONARIO = new SelectList(db.FUNCIONARIOs, "COD_FUNCIONARIO", "NOME");
            ViewBag.COD_PRODUTO = new SelectList(db.PRODUTOes, "COD_PRODUTO", "NOME");
            return View();
        }

        //// POST: Vendas/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "COD_VENDA,COD_FUNCIONARIO,COD_PRODUTO,NUM_NF,DATA")] VENDA vENDA)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.VENDAs.Add(vENDA);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.COD_FUNCIONARIO = new SelectList(db.FUNCIONARIOs, "COD_FUNCIONARIO", "NOME", vENDA.COD_FUNCIONARIO);
        //    ViewBag.COD_PRODUTO = new SelectList(db.PRODUTOes, "COD_PRODUTO", "NOME", vENDA.COD_PRODUTO);
        //    return View(vENDA);
        //}

        // GET: Vendas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDA vENDA = await db.VENDAs.FindAsync(id);
            if (vENDA == null)
            {
                return HttpNotFound();
            }
            ViewBag.COD_FUNCIONARIO = new SelectList(db.FUNCIONARIOs, "COD_FUNCIONARIO", "NOME", vENDA.COD_FUNCIONARIO);
            ViewBag.COD_PRODUTO = new SelectList(db.PRODUTOes, "COD_PRODUTO", "NOME", vENDA.COD_PRODUTO);
            return View(vENDA);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "COD_VENDA,COD_FUNCIONARIO,COD_PRODUTO,NUM_NF,DATA")] VENDA vENDA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vENDA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.COD_FUNCIONARIO = new SelectList(db.FUNCIONARIOs, "COD_FUNCIONARIO", "NOME", vENDA.COD_FUNCIONARIO);
            ViewBag.COD_PRODUTO = new SelectList(db.PRODUTOes, "COD_PRODUTO", "NOME", vENDA.COD_PRODUTO);
            return View(vENDA);
        }

        // GET: Vendas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDA vENDA = await db.VENDAs.FindAsync(id);
            if (vENDA == null)
            {
                return HttpNotFound();
            }
            return View(vENDA);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VENDA vENDA = await db.VENDAs.FindAsync(id);
            db.VENDAs.Remove(vENDA);
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



        /*************************************************************************************************/
        //caso algum erro deve ta por aqui
        JavaScriptSerializer js = new JavaScriptSerializer();//pra ajax --> JSON
        //bd_projeto_data db = new bd_projeto_data();

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        //get preço
        public string ProdutoDetalhes(int? id)
        {
            if (id != null)
            {
                PRODUTO ProdutoSelect = db.PRODUTOes.Find(id);
                if (ProdutoSelect != null)
                {
                    var ProdutoInfo = new
                    {
                        codigo = ProdutoSelect.COD_PRODUTO.ToString(),
                        nome = ProdutoSelect.NOME.ToString(),
                        preco = ProdutoSelect.PRECO.ToString(),
                        quantidade = ProdutoSelect.QUANTIDADE.ToString()
                    };
                    return js.Serialize(ProdutoInfo);
                }
                else
                {
                    var produtoNull = new { nome = "Produto Inexistente" };
                    return js.Serialize(produtoNull);

                }
            }
            else return null;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public string ProdutoProcura(string id)
        {
            List<PRODUTO> ProdutoPesquisa = db.PRODUTOes.Where(a => a.NOME.Contains(id)).ToList();
            List<Object> produtos = new List<Object>();
            if (ProdutoPesquisa != null)
            {

                foreach (PRODUTO pro in ProdutoPesquisa)
                {
                    var produto = new
                    {
                        Cod = pro.COD_PRODUTO,
                        Nome = pro.NOME,
                        Preco = pro.PRECO,
                        Quantidade = pro.QUANTIDADE
                    };
                    produtos.Add(produto);
                }
                return js.Serialize(produtos);
            }
            else
            {
                return js.Serialize(new { Nome = "Produto Inexistente" });
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public void FinalizaVenda(List<PRODUTO> id)
        {
            var date = System.DateTime.Now.Date;
            int nNota = (db.VENDAs.Count() + 1);
            //fazer uma calculadoras
            foreach (var item in id)
            {
                PRODUTO bdProduto = db.PRODUTOes.Single(c => c.COD_PRODUTO.Equals(item.COD_PRODUTO));
                if (bdProduto.QUANTIDADE >= item.QUANTIDADE)
                {
                    bdProduto.QUANTIDADE -= item.QUANTIDADE;
                    db.Entry(bdProduto).State = EntityState.Modified;

                    VENDA itemVenda = new VENDA();
                    itemVenda.COD_VENDA = 0;
                    itemVenda.COD_FUNCIONARIO = 2; //TRABALHAR AQUI PEGAR NA SESSAO do fucionario
                    itemVenda.COD_PRODUTO = bdProduto.COD_PRODUTO;
                    itemVenda.DATA = date;
                    itemVenda.NUM_NF = nNota;
                    db.VENDAs.Add(itemVenda);
                    var status = db.SaveChanges();
                }
            }
        }

        /*************************************************************************************************/
    }
}
