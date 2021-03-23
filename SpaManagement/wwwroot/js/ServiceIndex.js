var dataTable;
$(document).ready(function () {
    loadDataTable();
    loadDataTable1()
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "lengthMenu": [[2,10, 25, 50, -1], [2,10, 25, 50, "All"]],
        "ajax":{
            "url": "/Authenticated/API/Services/GetCustomer"
        },
        "columns":[
            {"data": "name", "width": "20%"},
            {"data": "phone", "width": "20%"},
            {"data": "identityCard", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/Services/Choose/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    Choose
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
var dataTable1;
function loadDataTable1(){
    dataTable1 = $('#tblData1').DataTable({
        "lengthMenu": [[2,10, 25, 50, -1], [2,10, 25, 50, "All"]],
        "ajax":{
            "url": "/Authenticated/API/Services/GetService"
        },
        "columns":[
            {"data": "name", "width": "20%"},
            {"data": "price", "width": "20%"},
            {"data": "note", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/services/AddService/${data}" class="btn btn-success text-white" style="cursor: pointer">
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

