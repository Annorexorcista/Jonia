$('#habitacionsel').change(function () {
    var idhabitacion = $(this).val()
    console.log("Hola")
    $.ajax({
        url: '/Paquetes/obtenerCostoHabitacion',
        type: 'GET',
        data: { id: idhabitacion, },

        success: function (data) {
            var costo = data.costo;
            $('#costohab').val(costo)
        },
        error: function(){
            console.error("Mala suya")
        }
    })
})