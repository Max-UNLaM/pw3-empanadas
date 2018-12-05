$(document).ready(function () {

    $("#btnEliminar").click(function () {
        var idPedido;
        idPedido = $("#IdHidden").val();
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

    $("#btnGuardarPedido").click(function () {
        var select = document.getElementById("gustos[]").selectedIndex;
        if (select == null || select == 0) {
            $("#selectGustos").rules("add", {
                required: true,
                messages: {
                    required: "<h4>El campo nombre es obligatorio</h4>"
                }
            });
        }
    });

    //$('#mySelect2').select2({
    //    dropdownParent: $('#myModal')
    //});
    $('.js-example-basic-multiple').select2({
        placeholder: 'Select an option',
        tags: true ,
        tokenSeparators: [',', ' ']
    });

    $('#js-prueba').select2({
        placeholder: 'Seleccione un Invitado',
        tags: true,
        tokenSeparators: [',', ' '],
        createTag: function (params) {
            // Don't offset to create a tag if there is no @ symbol
            if (params.term.indexOf('@') === -1) {
                // Return null to disable tag creation
                return null;
            }

            return {
                id: params.term,
                text: params.term
            }
        }
       // dropdownParent: $('#myModal')
    });
});


function confirmar() {
    var idPedido;
    idPedido = $("#IdHidden").val();
    if (confirm("¿Está seguro que desea Confirmar el pedido? Una vez confirmado no se podrá modificar.")) {
        $.ajax({
            url: "/Pedidos/CerrarPedido",
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
}