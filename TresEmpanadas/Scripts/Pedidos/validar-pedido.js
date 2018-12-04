//"use strict";

let formElements = {
    invitadosMultiselect: document.getElementById('js-prueba'),
    invitadosError: document.getElementById('invitados-error'),
    selectGustos: document.getElementById('selectGustos'),
    selectGustosError: document.getElementById('selectGustos-error')
}

function validateForm() {
    //let valid = false;
    resetErrors();
    let multi = validateMultiSelect();
    let gusto = validateGustos();
    if (multi === true && gusto === true) {
        return true
    } else {
        return false
    }
    //return valid;
}

function resetErrors() {
    let mensajesDeError = document.getElementsByClassName('val-error-message');
    for (let i = 0; i < mensajesDeError.length; i++) {
        mensajesDeError[i].classList.add('val-hidden');
    }
}

function validateMultiSelect() {
    if (formElements.invitadosMultiselect.value == '') {
        formElements.invitadosError.classList.remove('val-hidden')
        return false
    }
    return true
}

function validateGustos() {
    if (formElements.selectGustos.value == '') {
        formElements.selectGustosError.classList.remove('val-hidden')
        return false
    }
    return true
}