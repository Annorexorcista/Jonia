$(document).ready(function () {
    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Abonos/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#abonosTable tbody').html($(data).find('#abonosTable tbody').html());
                
            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Reservas/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#reservasTable tbody').html($(data).find('#reservasTable tbody').html());
                
            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });
});