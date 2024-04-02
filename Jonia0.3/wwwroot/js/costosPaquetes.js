
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

    

function actualizarTotalPaquete(servicios) {
    var costoHabitacion = parseFloat($('#costohab').val());
    var totalServicios = 0;

    servicios.forEach(function (servicio) {
        totalServicios += parseFloat(servicio.Precio);
    });

    var totalPaquete = costoHabitacion + totalServicios;

    $('#total-paquete').val(Math.round(totalPaquete,0));
}

$('#habitacionsel').change(function () {
    var idhabitacion = $(this).val();
    $.ajax({
        url: '/Paquetes/obtenerCostoHabitacion',
        type: 'GET',
        data: { id: idhabitacion },
        success: function (data) {
            var costo = data.costo;
            $('#costohab').val(costo);
            actualizarTotalPaquete(serviciosSeleccionados);
        },
        error: function () {
            console.error("Error al obtener el costo de la habitación");
        }
    });
});
$('#serviciosel').change(function () {
    var idservicio = $(this).val();
    $.ajax({
        url: '/Paquetes/obtenerCostoServicio',
        type: 'GET',
        data: { id: idservicio },
        success: function (data) {
            var costo = data.costo;
            $('#costoser').val(costo);
        },
        error: function () {
            console.error("Error al obtener el costo del servicio");
        }
    });
});

   
