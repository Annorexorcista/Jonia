// Manejar el evento de cambio
$('.form-check-input').change(function () {
    var abonoId = $(this).data('abonoid');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Abonos/ActualizarEstado',
        type: 'POST',
        data: { id: abonoId, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });
    
});

$('.form-check-input').change(function () {
    var rolId = $(this).data('rolid');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Rol/ActualizarEstado',
        type: 'POST',
        data: { id: rolId, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

});

$('.form-check-input').change(function () {
    var idtipo = $(this).data('idtipo');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/TipoHabitacion/ActualizarEstado',
        type: 'POST',
        data: { id: idtipo, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

});

$('.form-check-input').change(function () {
    var idpaquete = $(this).data('idpaquete');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Paquetes/ActualizarEstado',
        type: 'POST',
        data: { id: idpaquete, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

});

$('.form-check-input').change(function () {
    var idhabitacion = $(this).data('idhabitacion');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Habitaciones/ActualizarEstado',
        type: 'POST',
        data: { id: idhabitacion, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

})

$('.form-check-input').change(function () {
    var idusuario = $(this).data('idusuario');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Usuarios/ActualizarEstado',
        type: 'POST',
        data: { id: idusuario, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

})

$('.form-check-input').change(function () {
    var idservicio = $(this).data('idservicio');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Servicios/ActualizarEstado',
        type: 'POST',
        data: { id: idservicio, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

})

$('.form-check-input').change(function () {
    var clienteid = $(this).data('clienteid');
    var nuevoEstado = $(this).prop('checked'); // true si está marcado, false si no lo está

    $.ajax({
        url: '/Clientes/ActualizarEstado',
        type: 'POST',
        data: { id: clienteid, estado: nuevoEstado },
        success: function (data) {
            console.log('Estado actualizado en la base de datos');
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el estado en la base de datos: ' + error);
        }
    });

})


