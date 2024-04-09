$('#abono').keyup(function() {
    var valorAbono = $('#abono').val()
    var iva = valorAbono * 0.19
    var subtotal = valorAbono-(iva)
    $('#subtotal').val(subtotal)
    $('#iva').val(iva)
    var deuda = $('#deuda').val() 
    var porcentaje = ((100 * valorAbono) / deuda).toFixed(2)
    $('#porcentaje').val(porcentaje)
    

})

$('#btnCrear').click(function () {
   $('#totalPendiente').val($('#totalPendiente').val() - $('#abono').val())
    var idReserva = $('#IdReserva').val(); // Obtener el IdReserva del campo oculto
    window.location.href = '/Abonos/IndividualIndex?idReserva=' + idReserva; // Redirigir al IndividualIndex con el IdReserva
});

