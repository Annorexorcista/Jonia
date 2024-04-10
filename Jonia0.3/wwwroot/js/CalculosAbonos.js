const formabono = document.getElementById('formabono')
$('#abono').keyup(function () {
    var valorAbono = $('#abono').val()
    var iva = valorAbono * 0.19
    var subtotal = valorAbono-(iva)
    $('#subtotal').val(subtotal)
    $('#iva').val(iva)
    var deuda = $('#deuda').val() 
    var porcentaje = ((100 * valorAbono) / deuda).toFixed(2)
    $('#porcentaje').val(porcentaje)
    

})
function porcentajevali() {
    if (valorAbono > deuda) {
        porcentaje = 100.00
        $('#porcentaje').val(porcentaje)
    }
}


