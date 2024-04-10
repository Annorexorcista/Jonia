$('.boton-estado').click(function () {
    var abonoId = $(this).find('input[type=hidden]').val();
    var nuevoEstado = 0; // Estado de anulado
    location.reload(true); 
    $.ajax({
        url: '/Abonos/ActualizarEstado',
        type: 'POST',
        data: { id: abonoId, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
            // Puedes realizar alguna acción adicional si lo necesitas
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });
});