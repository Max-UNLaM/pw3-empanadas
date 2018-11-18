using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TresEmpanadas.Services
{
    public class UsuarioService
    {
        Entities Entities = new Entities();
        public MultiSelectList ListarUsuarios()
        {
            var listadoUsuarios = new MultiSelectList(Entities.Usuario, "IdUsuario", "Email");
            return listadoUsuarios;
        }

        public Usuario GetUsuario(int id)
        {
            return Entities.Usuario.Single(us => us.IdUsuario == id);
        }

    }
}