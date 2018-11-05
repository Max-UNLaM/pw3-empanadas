using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TresEmpanadas.Services
{
    public class UsuarioService
    {
        Entities entities = new Entities();
        public SelectList ListarUsuarios() {
            var listadoUsuarios = new SelectList(entities.Usuario, "IdUsuario", "Email");
            return listadoUsuarios;
        }


    }
}