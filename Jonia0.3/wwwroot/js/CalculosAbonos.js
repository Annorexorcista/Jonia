$(document).ready(function () {
    var idReserva = $("#idReserva").val();
    obtenerDeuda(idReserva);

    function obtenerDeuda(idReserva) {
        $.ajax({
            url: "/Abonos/obtenerDeuda",
            type: "GET",
            data: { id: idReserva },
            success: function (data) {
                
                // Manipula la respuesta JSON aquí
                $("#deuda").val(data.costo);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }

    // Evento para actualizar la deuda cuando cambia el ID de reserva
    $("#idReserva").change(function () {
        idReserva = $(this).val();
        obtenerDeuda(idReserva);
    });
});