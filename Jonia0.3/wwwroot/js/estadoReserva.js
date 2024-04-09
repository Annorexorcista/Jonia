$('.form-control-sm').change(function () {
    var reservaid = $(this).data('reservaid');
    var nuevoEstado = $(this).find('option:selected').data('nuevoestado');

    $.ajax({
        url: '/Reservas/ActualizarEstado',
        type: 'POST',
        data: { id: reservaid, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });
});

$(document).ready(function () {
    $('.form-control-sm').each(function () {
        var selectedValue = $(this).val();
        if (selectedValue === '5' || selectedValue === '6') {
            $(this).prop('disabled', true);
        }
    });

    $('.form-control-sm').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue === '5' || selectedValue === '6') {
            $(this).prop('disabled', true);
            location.reload(true); 
        } else {
            $(this).prop('disabled', false);
        }
    });
});