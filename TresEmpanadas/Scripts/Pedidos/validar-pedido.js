"use strict";

let formElements = {
    invitadosMultiselect: document.getElementById('js-prueba'),
    invitadosError: document.getElementById('invitados-error'),
    selectGustos: document.getElementById('selectGustos'),
    selectGustosError: document.getElementById('selectGustos-error')
}

function validateForm() {
    let valid = false;
    resetErrors();
    validateMultiSelect();
    return valid;
}

function resetErrors() {
    let mensajesDeError = document.getElementsByClassName('val-error-message');
    for (let i = 0; i < mensajesDeError.length; i++) {
        mensajesDeError[i].classList.add('val-hidden');
    }
}

function validateMultiSelect() {
    if (formElements.invitadosMultiselect.value === '') {
        formElements.invitadosError.classList.remove('val-hidden')
    }
}

function validateGustos() {
    if (formElements.selectGustos.value === '') {
        formElements.selectGustosError.classList.remove('val-hidden')
    }
}