var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/ServiceDetails/GetAll"
        },
        "columns":[
            {"data": "categoryService.name", "width": "8%"},
            {"data": "customer.name", "width": "8%"},
            {"data": "customer.phone", "width": "10%"},
            {"data": "customer.identityCard", "width": "8%"},
            {"data": "useTime", "width": "8%"},
            {"data": "slottime", "width": "8%"},
            {"data": "price", "width": "8%"},
            {"data": "discount", "width": "8%"},
            {"data": "orderDate", "width": "8%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/API/ServiceDetails/Upsert/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a class="btn btn-danger text-white" onclick=Delete("/Authenticated/API/ServiceDetails/Delete/${data}") style="cursor: pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                    `;
                },"width":"28%"
            }
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}

function Delete(url){
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this imaginary file!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete){
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data){
                    if(data.success){
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

