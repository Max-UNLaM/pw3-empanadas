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
        public MultiSelectList ListarUsuarios() {
            var listadoUsuarios = new MultiSelectList(entities.Usuario, "IdUsuario", "Email");
            return listadoUsuarios;
        }


    }
}