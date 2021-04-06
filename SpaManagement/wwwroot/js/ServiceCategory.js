var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/Services/GetService"
        },
        "columns":[
            {"data": "name", "width": "20%"},
            {"data": "price", "width": "20%"},
            {"data": "description", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated//Detail/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-plus"></i>
                                </a>
                            </div>
                    `;
                },"width":"40%"
            }
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}


