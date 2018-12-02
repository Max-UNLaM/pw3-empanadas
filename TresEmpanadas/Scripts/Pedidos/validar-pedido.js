"use strict";

let formElements = {
    invitadosMultiselect: document.getElementById('js-prueba')
}

function validateForm() {
    valid = false;
    console.log(formElements.invitadosMultiselect.value);
    return valid;
}


function validateMultiSelect() {
    if (formElements.invitadosMultiselect.value === '') {

    }
}