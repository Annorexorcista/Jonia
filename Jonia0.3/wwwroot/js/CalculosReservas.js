$('#paquetesel').change(function () {
    var idhabitacion = $(this).val()
    $.ajax({
        url: '/Reservas/obtenerNroPersonas',
        type: 'GET',
        data: { id: idhabitacion, },
        success: function (data) {
            var nro = data.nro;
            $('#nroper').val(nro)
        },
        error: function () {
            console.error("Mala suya")
        }
    })
})

$('#paquetesel').change(function () {
    var idpaquete = $(this).val()
    $.ajax({
        url: '/Reservas/obtenerCostoPaquete',
        type: 'GET',
        data: { id: idpaquete, },
        success: function (data) {
            var costo = data.costo;
            $('#costopaq').val(costo)
        },
        error: function () {
            console.error("Mala suya")
        }
    })
})

$('#paquetesel').change(function () {
    var idhabitacion = $(this).val()
    $.ajax({
        url: '/Reservas/obtenerNombreHabitacion',
        type: 'GET',
        data: { id: idhabitacion, },
        success: function (data) {
            var nombre = data.nombre;
            $('#nombrehab').val(nombre)
        },
        error: function () {
            console.error("Mala suya")
        }
    })
})

$('#paquetesel').change(function () {
    var idhabitacion = $(this).val()

    $.ajax({
        url: '/Reservas/obtenerTipoHabitacion',
        type: 'GET',
        data: { id: idhabitacion, },
        success: function (data) {
            var nombre = data.nombre;
            actualizarTotalPaquete();   //Utilizamos el ajax para actualizar en tiempo real el total del paquete (que en realidad es el calculo de todos los datos))
            $('#tipohab').val(nombre)
        },
        error: function () {
            console.error("Mala suya")
        }
    })
})

$('#serviciosel').change(function () {
    var idservicio = $(this).val()
    $.ajax({
        url: '/Reservas/obtenerCostoServicio',
        type: 'GET',
        data: { id: idservicio, },
        success: function (data) {
            var costo = data.costo;
            $('#costoser').val(costo)
        },
        error: function () {
            console.error("Mala suya")
        }
    })
})

$('#serviciosel').change(function () {
    var idservicio = $(this).val()
    $.ajax({
        url: '/Reservas/obtenerTipoServicio',
        type: 'GET',
        data: { id: idservicio, },
        success: function (data) {
            var nombre = data.nombre;
            $('#tiposer').val(nombre)
        },
        error: function () {
            console.error("Mala suya")
        }
    })
})


var serviciosSeleccionados = [];


function actualizarInputServicios() {
    $('#serviciosSeleccionados').val(JSON.stringify(serviciosSeleccionados))
}

$('#add-service').click(function () {
    var selectedService = $('#serviciosel').find(':selected');
    var servicio = {
        IdServicio: selectedService.val(),
        Nombre: selectedService.text(),
        Precio: parseFloat($('#costoser').val())
    };

    serviciosSeleccionados.push(servicio);
    actualizarTablaServicios(serviciosSeleccionados);
    actualizarTotalPaquete(serviciosSeleccionados);
    actualizarInputServicios();
});

function eliminarServicio(btn) {
    var rowIdx = $(btn).closest('tr').index();
    serviciosSeleccionados.splice(rowIdx, 1);

    $(btn).closest('tr').remove();
    actualizarTablaServicios(serviciosSeleccionados);
    actualizarTotalPaquete(serviciosSeleccionados);
    actualizarInputServicios();
}

function actualizarTablaServicios(servicios) {
    var tbody = $('#servicios-table tbody');
    tbody.empty();

    servicios.forEach(function (servicio) {
        var row = '<tr><td>' + servicio.Nombre + '</td><td>' + servicio.Precio + '</td><td><button class="btn btn-danger btn-sm" onclick="eliminarServicio(this)" type="button">Eliminar Servicio</button></td></tr>'
        tbody.append(row);
    });
}


function actualizarTotalPaquete() {

    //Calculamos fecha y el número de días
    var fechaini = new Date(document.getElementById('fechaini').value);
    var fechafin = new Date(document.getElementById('fechafin').value);
    var diasdif = fechafin.getTime() - fechaini.getTime();
    var contdias = Math.round(diasdif / (1000 * 60 * 60 * 24));
    document.getElementById('dias').value = contdias;

    var costoPaquete = 0;
    costoPaquete = $('#costopaq').val();
    $('#costopaq').val(costoPaquete)

    var totalServicios = 0;
    var subtotal = 0;

    serviciosSeleccionados.forEach(function (servicio) {
        totalServicios += parseFloat(servicio.Precio);

    });



    subtotal = (costoPaquete * contdias) + totalServicios;

    $('#subtotal-reserva').val('$ ' + Math.round(subtotal, 0));

    const Iva = 0.19;

    var total_iva = subtotal * Iva;
    $('#iva').val('$ ' + Math.round(total_iva, 0));

    var des = $('#des').val() / 100 || 0;
    var valor_des = subtotal * des;
    var total = (subtotal - valor_des) + total_iva;

    $('#totalreserva').val('$ ' + Math.round(total, 0));

}

//En caso de cambiar la fecha, se actualiza el precio sin tener que mover el paquete de nuevo
$('#fechaini').change(function () {
    actualizarTotalPaquete();
})
$('#fechafin').change(function () {
    actualizarTotalPaquete();
})

