var dataTable;
$(document).ready(function () {
    loadDataTable();
    loadDataTable1();
    loadDataTable2();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "lengthMenu": [[2,10, 25, 50, -1], [2,10, 25, 50, "All"]],
        "ajax":{
            "url": "/Authenticated/ServiceDetail/GetCustomer"
        },
        "columns":[
            {"data": "name", "width": "20%"},
            {"data": "phone", "width": "20%"},
            {"data": "identityCard", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/ServiceDetail/Choose/${data}" class="btn btn-success text-white" style="cursor: pointer">
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
            "url": "/Authenticated/ServiceDetail/GetService"
        },
        "columns":[
            {"data": "categoryService.name", "width": "20%"},
            {"data": "price", "width": "20%"},
            {"data": "categoryService.note", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/ServiceDetail/ServiceDetail/${data}" class="btn btn-success text-white" style="cursor: pointer">
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
var dataTable2;
function loadDataTable2(){
    dataTable2 = $('#tblData2').DataTable({
        "lengthMenu": [[2,10, 25, 50, -1], [2,10, 25, 50, "All"]],
        "ajax":{
            "url": "/Authenticated/ServiceDetail/GetServiceUser"
        },
        "columns":[
            {"data": "serviceDetail.orderDate", "width": "20%"},
            {"data": "serviceDetail.slot", "width": "20%"},
            {"data": "serviceDetail.price", "width": "20%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/ServiceDetail/ServiceUser/${data}" class="btn btn-success text-white" style="cursor: pointer">
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
