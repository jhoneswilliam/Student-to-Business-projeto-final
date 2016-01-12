using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2B_ProjetoFinal_Jhones.Models;

namespace S2B_ProjetoFinal_Jhones.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string id)
        {
            //fazer uma condição que verifica se o usuario já esta logado
            //assim faz o redirecionamento para a pagina principal
            ViewBag.mensagen = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Login_User, string Login_Senha)
        {
            if (ModelState.IsValid)
            {
                //muda esse if  "!"
                if (!Usuario.Verificar(Login_User, Login_Senha))
                {
                    //Depois de implementado olha aqui
                    Response.Redirect("Home");
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
            return null;
        }



        //esqueci a senha
        public ActionResult RecuperacaoDeConta()
        {
            return View();
        }

        //registra usuario
        public ActionResult Registrar()
        {
            return View();
        }
    }
}