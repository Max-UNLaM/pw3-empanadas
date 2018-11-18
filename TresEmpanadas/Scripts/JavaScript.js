$(document).ready(function () {
    $("#btnEliminar").click(function () {
        var idPedido;
        idPedido = $("#IdHidden").val();
        //Id : 1 
        var valorElegido = confirm("Esta seguro que desea Eliminar");
        if (valorElegido) {
            $.ajax({
                url: "/Pedidos/Eliminar",
                type: "GET",
                data: { idPedido: idPedido },
                success: function (result) {
                    if (result == 1) {
                        window.location.replace("/Pedidos/ListadoPedidos");
                        alert("OK!");
                    }
                    //if (result == 1) {
                    //    $.ajax({
                    //        url: "/Pedidos/ListadoPedidos",
                    //        type: "GET",
                    //    });

                   // }
                },
                error: function (x, y, z) {
                    url = "/ListadoPedidos"
                    window.location.href = url;
                    alert("No se pudo eliminar!");
                    alert(x + y + z);
                }
            });
        }
    });
    $('.btn-block').click(function () {
        var idPedido;
        idPedido = $(this).attr('id');
        var valorElegido = confirm("Esta seguro que desea Eliminar" + idPedido);
        if (valorElegido) {
            $.ajax({
                url: "/Pedidos/Eliminar",
                type: "GET",
                data: { idPedido: idPedido },
                success: function (result) {
                    if (result == 1) {
                        window.location.replace("/Pedidos/ListadoPedidos");
                        alert("OK!");
                    }
                },
                error: function (x, y, z) {
                    url = "/ListadoPedidos"
                    window.location.href = url;
                    alert("No se pudo eliminar!");
                    alert(x + y + z);
                }
            });
        }
    });
});