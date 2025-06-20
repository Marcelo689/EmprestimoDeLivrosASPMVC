$(document).ready(function () {

    $("#Emprestimos").DataTable({
        language:
            {
                "decimal": "",
                "emptyTable": "No data available in table",
                "info": "Mostrando _START_ registro de  _END_ em um total de _TOTAL_ Entradas",
                "infoEmpty": "Showing 0 to 0 of 0 entries",
                "infoFiltered": "(filtered from _MAX_ total entries)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ Entradas",
                "loadingRecords": "Loading...",
                "processing": "",
                "search": "Procurar:",
                "zeroRecords": "No matching records found",
                "paginate": {
                    "first": "Primeiro",
                    "last": "Último",
                    "next": "Próximo",
                    "previous": "Anterior"
                }
            }
        }
    );

    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).alert("close");
        });
    }, 5000);
});