var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/Sold/GetCustomer"
        },
        "columns":[
            {"data": "name", "width": "20%"},
            {"data": "phone", "width": "20%"},
            {"data": "identityCard", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/Sold/Product/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-shopping-cart"></i>
                                </a>
                                <a href="/Authenticated/Return//${data}" class="btn btn-warning text-white" style="cursor: pointer">
                                    <i class="fas fa-undo-alt"></i>
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


