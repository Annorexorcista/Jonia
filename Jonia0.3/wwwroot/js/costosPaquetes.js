$(document).ready(function () {
    var serviciosSeleccionados = [];

    $('#add-service').click(function () {
        var selectedService = $('#serviciosel').find(':selected');
        var servicio = {
            id: selectedService.val(), 
            nombre: selectedService.text(),
            precio: parseFloat($('#costoser').val()) 
        };

        serviciosSeleccionados.push(servicio);
        actualizarTablaServicios(serviciosSeleccionados);
        actualizarTotalPaquete(serviciosSeleccionados);
    });

    function actualizarTablaServicios(servicios) {
        var tbody = $('#servicios-table tbody');
        tbody.empty();

        servicios.forEach(function (servicio) {
            var row = '<tr><td>' + servicio.nombre + '</td><td>' + servicio.precio + '</td></tr>';
            tbody.append(row);
        });
    }

    function actualizarTotalPaquete(servicios) {
        var costoHabitacion = parseFloat($('#costohab').val());
        var totalServicios = 0;

        servicios.forEach(function (servicio) {
            totalServicios += parseFloat(servicio.precio);
        });

        var totalPaquete = costoHabitacion + totalServicios;

        $('#total-paquete').val(totalPaquete.toFixed(2));
    }

    $('#habitacionsel').change(function () {
        var idhabitacion = $(this).val();
        $.ajax({
            url: '/Paquetes/obtenerCostoHabitacion',
            type: 'GET',
            data: { id: idhabitacion },
            success: function (data) {
                var costo = data.costo;
                $('#costohab').val(costo.toFixed(2));
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
                $('#costoser').val(costo.toFixed(2));
            },
            error: function () {
                console.error("Error al obtener el costo del servicio");
            }
        });
    });

    $('#create-form').submit(function () {
        $('#serviciosSeleccionados').val(JSON.stringify(serviciosSeleccionados)); // Establecer el valor del campo oculto

        return true; // Continuar con el envío del formulario
    });
});