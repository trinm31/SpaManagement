var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/Products/GetAll"
        },
        "columns":[
            {"data": "name", "width": "10%"},
            {"data": "importPrice", "width": "10%"},
            {"data": "price", "width": "10%"},
            {"data": "supplier.name", "width": "10%"},
            {"data": "typeOfProduct.name", "width": "10%"},
            {"data": "manufacturer.name", "width": "10%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/API/Products/Upsert/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a class="btn btn-danger text-white" onclick=Delete("/Authenticated/API/Products/Delete/${data}") style="cursor: pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                    `;
                },"width":"30%"
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

