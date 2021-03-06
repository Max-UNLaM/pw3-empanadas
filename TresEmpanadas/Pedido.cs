//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TresEmpanadas
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedido()
        {
            this.InvitacionPedido = new HashSet<InvitacionPedido>();
            this.InvitacionPedidoGustoEmpanadaUsuario = new HashSet<InvitacionPedidoGustoEmpanadaUsuario>();
            this.GustoEmpanada = new HashSet<GustoEmpanada>();
        }
    
        public int IdPedido { get; set; }
        public int IdUsuarioResponsable { get; set; }
        public string NombreNegocio { get; set; }
        public string Descripcion { get; set; }
        public int IdEstadoPedido { get; set; }
        public int PrecioUnidad { get; set; }
        public int PrecioDocena { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        public virtual EstadoPedido EstadoPedido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvitacionPedido> InvitacionPedido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvitacionPedidoGustoEmpanadaUsuario> InvitacionPedidoGustoEmpanadaUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GustoEmpanada> GustoEmpanada { get; set; }
    }
}
