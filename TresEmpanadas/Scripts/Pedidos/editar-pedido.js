"use strict";

let editarPedidoElements = {
    tablaAltaGustos: document.getElementById('tabla-alta-gustos'),
    gustosEmpa: document.getElementById('select-gustos-empa').outerHTML,
    token: document.getElementById('token-invitacion').innerText
};

const templates = {
    cantidadInput: `<input data-elemento="cantindadEmpanada" type="number">`
};

function agregarGusto() {
    let cantidadFilas = editarPedidoElements.tablaAltaGustos.rows.length;
    const fila = editarPedidoElements.tablaAltaGustos.insertRow(cantidadFilas);
    const gusto = fila.insertCell(0);
    const cantidad = fila.insertCell(1);
    fila.id = `rownum${cantidadFilas}`;
    gusto.innerHTML = editarPedidoElements.gustosEmpa;
    cantidad.innerHTML = templates.cantidadInput;
}

function eliminarFila() {
    let cantidadFilas = editarPedidoElements.tablaAltaGustos.rows.length;
    editarPedidoElements.tablaAltaGustos.deleteRow(cantidadFilas - 1)
}

function botonEliminarGusto(index) {
    return `<button class="btn btn-danger" data-index="${index}" onclick="eliminarFila(this)">ELIMINAR</button>`
}

function enviarGustos() {
    let filas = Array.from(editarPedidoElements.tablaAltaGustos.rows);
    let validRows = [];
    let modificarPedido = {
        GustosPedidos: [],
        IdUsuario: 1,
        TokenInvitacion: editarPedidoElements.token
    }
    filas.forEach((fila) => {
        if (fila.id !== '') {
            validRows.push(fila.id);
        }
    });
    for (let row of validRows) {
        modificarPedido.GustosPedidos.push({
            IdGustoEmpanada: document.getElementById(row).childNodes[0].childNodes[0].value,
            Cantidad: document.getElementById(row).childNodes[1].childNodes[0].value
        })
    }
    sendGustosToApi(modificarPedido);
}

function sendGustosToApi(modificarPedido) {
    let sender = new XMLHttpRequest();
    console.log(modificarPedido);
    sender.open("POST", '/api/Pedidos/ConfirmarPedido');
    sender.setRequestHeader("Content-Type", "application/json");
    sender.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE && this.status === 201) {
            console.log(this);
        } else {
            console.log(this);
        }
    }
    console.log(JSON.stringify(modificarPedido));
    sender.send(JSON.stringify(modificarPedido));
}