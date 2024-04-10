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

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Rol/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#rolTable tbody').html($(data).find('#rolTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Usuarios/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#usuariosTable tbody').html($(data).find('#usuariosTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Habitaciones/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#habitacionesTable tbody').html($(data).find('#habitacionesTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/TipoHabitacion/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#tipoTable tbody').html($(data).find('#tipoTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Servicios/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#servicioTable tbody').html($(data).find('#servicioTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/TipoServicio/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#tipoSTable tbody').html($(data).find('#tipoSTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Paquetes/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#paquetesTable tbody').html($(data).find('#paquetesTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });

    $('#searchInput').on('keyup', function () {
        var searchValue = $(this).val();
        $.ajax({
            url: '/Clientes/Index',
            type: 'GET',
            data: { search: searchValue },
            success: function (data) {
                // Busca la tabla por su ID y actualiza solo el cuerpo (tbody)
                $('#clientesTable tbody').html($(data).find('#clientesTable tbody').html());

            },
            error: function (xhr, status, error) {
                console.error('Error al realizar la búsqueda: ' + error);
            }
        });
    });
});