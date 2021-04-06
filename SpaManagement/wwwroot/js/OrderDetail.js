var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/OrderDetail/getOrderList"
        },
        "columns":[
            {"data": "id", "width": "5%"},
            {"data": "customer.name", "width": "15%"},
            {"data": "customer.phone", "width": "15%"},
            {"data": "customer.identityCard", "width": "15%"},
            {"data": "orderDate", "width": "15%"},
            {"data": "amount", "width": "15%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/OrderDetail/Detail/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                            </div>
                    `;
                },"width":"25%"
            }
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}