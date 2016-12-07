using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2B.Models;

namespace S2B.Controllers
{
    public class HomeController : Controller
    {
        private static bd_projeto_data db = new bd_projeto_data();//Connecta ao banco
        //quando a classe do controller for construida ele verificara se o usuario ta logado //SESSAO
        public ActionResult Index()
        {                      
            return View();
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

        public ActionResult Dashboard()
        {
            HttpCookie Login_User = Request.Cookies["Login"];
            HttpCookie Login_Senha = Request.Cookies["Senha"];
            if (Login_User != null && Login_Senha != null)
            {
                string senha = Login_Senha.Value;
                string user = Login_User.Value;
                if ((db.FUNCIONARIOs.Where(a => a.LOGIN.Equals(user) && a.SENHA.Equals(senha)).Count() == 1))
                {
                     return View();
                }
                else
                {
                    Response.Redirect("/Home/Login");
                }
            }
            else
            {
                Response.Redirect("/Home/Login");
            }
            return null;
        }
        // GET: Login
        public ActionResult Login(string id)
        {
            //fazer uma condição que verifica se o usuario já esta logado
            //assim faz o redirecionamento para a pagina principal
            ViewBag.mensagen = id;
            return View();
        }



        // POST: Login//SESSAO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Login_User, string Login_Senha)
        {
            if (ModelState.IsValid)
            {
                if (db.FUNCIONARIOs.Where(a => a.LOGIN.Equals(Login_User) && a.SENHA.Equals(Login_Senha)).Count() == 1)
                {
                    HttpCookie Login = new HttpCookie("Login");
                    Login.Value = Login_User;
                    Login.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(Login);

                    HttpCookie Senha = new HttpCookie("Senha");
                    Senha.Value = Login_Senha;
                    Senha.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(Senha);

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ViewBag.mensagen = "Login ou senha Invalidos";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public RedirectResult Sair()
        {
            //SESSAO
            try
            {
                HttpCookie Login = new HttpCookie("Login");
                HttpCookie Senha = new HttpCookie("Senha");
                Login.Expires = DateTime.Now.AddDays(-1);
                Senha.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Login);
                Response.Cookies.Add(Senha);
                return Redirect("Index");
            }
            catch (Exception)
            {
                return Redirect("Index");
            }
        }        
    }
}