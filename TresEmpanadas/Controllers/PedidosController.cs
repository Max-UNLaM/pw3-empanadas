﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Models.ViewModels;
using TresEmpanadas.Services;
namespace TresEmpanadas.Controllers
{
    public class PedidosController : Controller
    {
        PedidoService servicioPedido = new PedidoService();
        UsuarioService servicioUsuario = new UsuarioService();
        //Entities contexto = new Entities();

        // Iniciar Pedido
        public ActionResult IniciarPedido(int? idPedido)
        {
            System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
            ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
            ViewBag.usuariosDisponibles = servicioUsuario.ListarUsuarios();
            if (idPedido == null)
            {
                ViewBag.conModelo = false;
                return View();
            }
            else
            {
                ViewBag.conModelo = true;
                int idParametro = (int)idPedido;
                Pedido pedidoBuscado = servicioPedido.BuscarPedidoPorId(idParametro);
                return View(pedidoBuscado);
            }
        }

        //Guardar Pedido
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados)
        {
            servicioPedido.GuardarPedido(pedido, gustos, usuariosInvitados);
            return View("PedidoIniciado");
        }

        //Listado Pedidos
        public ActionResult ListadoPedidos()
        {
            if (Session["idUsuario"] != null)
            {
                ViewBag.pedidoEliminado = Session["pedidoEliminado"];
                //Session["pedidoEliminado"] = null;
                Session.Remove("pedidoEliminado");
                var listadoPedidos = servicioPedido.ListadoPedidosAsociadosUsuario();
                ViewBag.pedidosUsuario = listadoPedidos;
                return View();
            }
            else
            {
                return Redirect("/Home/Login?redirigir=/Pedidos/listadoPedidos/");
            }
        }

        //Detalle Pedidos
        public ActionResult DetallePedido(int? idPedido)
        {
            Pedido detallePedido;
            if (idPedido != null)
            {
                detallePedido = servicioPedido.BuscarPedidoPorId((int)idPedido);
            }
            else
            {
                int idRecibido = (int)TempData["idPedido"];
                if (idRecibido > 0)
                {
                    detallePedido = servicioPedido.BuscarPedidoPorId((int)idRecibido);
                }
                else
                {
                    return View();
                }
            }
            ViewBag.detallePedido = detallePedido;
            return View(detallePedido);

        }

        //Eliminar Pedidos

        public RedirectToRouteResult Eliminar(int idPedido)
        {
            var nombrePedidoEliminado = servicioPedido.BuscarPedidoPorId(idPedido);
            servicioPedido.EliminarPedido(idPedido);
            Session["pedidoEliminado"] = nombrePedidoEliminado.NombreNegocio;
            return RedirectToAction("ListadoPedidos");
        }
        public ActionResult EliminarPedido(int idPedido)
        {
            var pedido = servicioPedido.BuscarPedidoPorId(idPedido);
            var invitacionesConfirmadas = servicioPedido.BuscarInvitacionesConfirmadas(idPedido);
            ViewBag.cantidadInvitaciones = invitacionesConfirmadas;
            return View(pedido);
        }
        // Eliminar con JavaScrit
        //[HttpGet]
        //public ActionResult Eliminar(int? idPedido)
        //{
        //    var valor = idPedido;
        //    var result = 0;
        //    if (valor != null) {
        //        servicioPedido.EliminarPedido((int)valor);
        //        // return RedirectToAction("ListadoPedidos");
        //        result = 1;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EditarPedido(int idPedido)
        {
            Boolean estadoPedido = servicioPedido.EstadoPedido(idPedido);
            if (estadoPedido)
            {
                System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
                ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
                ViewBag.usuariosDisponibles = servicioUsuario.ListarUsuarios();
                ViewBag.conModelo = true;
                int idParametro = (int)idPedido;
                Pedido pedidoBuscado = servicioPedido.BuscarPedidoPorId(idParametro);
                return View(pedidoBuscado);
            }
            else
            {
                //var detallePedido = servicioPedido.BuscarPedidoPorId(idPedido);
                //ViewBag.detallePedido = detallePedido;
                //return View("",detallePedido);
                TempData["idPedido"] = idPedido;
                return RedirectToAction("DetallePedido");
            }
        }

        public ActionResult ElegirGustos()
        {
            int idPedido;
            try
            {

                idPedido = Int32.Parse(Request.QueryString["IdPedido"]);
            }
            catch
            {
                return RedirectToAction("ListadoPedidos");
            }
            var elegirPedidoService = new ElegirPedidoService();
            return View(elegirPedidoService.BuildElegirPedido(idPedido));
        }
    }
}
