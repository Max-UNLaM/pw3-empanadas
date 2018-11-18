using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TresEmpanadas;

namespace TresEmpanadas.Services
{
    public class LoginService
    {
        Entities contexto = new Entities();

        public bool verificarDatos(Usuario usu)
        {
            Usuario usuarioEncontrado = contexto.Usuario.Where(u => u.Email.Equals(usu.Email) && u.Password.Equals(usu.Password)).FirstOrDefault();
            
            if(usuarioEncontrado != null) {
                HttpContext.Current.Session["idUsuario"] = usuarioEncontrado.IdUsuario;
                HttpContext.Current.Session["email"] = usuarioEncontrado.Email;

                return true;
            }

            return false;
        }
    }
}