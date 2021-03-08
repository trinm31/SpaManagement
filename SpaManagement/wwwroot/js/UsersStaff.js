var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/Users/GetAll"
        },
        "columns":[
            {"data": "name", "width": "15%"},
            {"data": "email", "width": "15%"},
            {"data": "phoneNumber", "width": "15%"},
            {"data": "role", "width": "15%"},
            {
                "data": {id:"id", lockoutEnd: "LockoutEnd"},
                "render": function (data){
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today){
                        return `<div class="text-center">
                                <a class="btn btn-danger text-white" onclick=LockUnlock("${data.id}") style="cursor: pointer">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                                <a href="/Authenticated/API/Users/Edit/${data.id}" class="btn btn-primary text-white" style="cursor: pointer">
                                    <i class="fas fa-edit"></i>
                                </a>                    
                            </div>
                        `;
                    }
                    else {
                        return `<div class="text-center">
                                <a class="btn btn-success text-white" onclick=LockUnlock("${data.id}") style="cursor: pointer">
                                    <i class="fas fa-lock"></i> lock
                                </a>
                                <a href="/Authenticated/API/Users/Edit/${data.id}" class="btn btn-primary text-white" style="cursor: pointer">
                                    <i class="fas fa-edit"></i>
                                </a>                               
                            </div>
                        `;
                    }
                },"width":"25%"
            }
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}

function LockUnlock(id){
    $.ajax({
        type: "POST",
        url: '/Authenticated/API/Users/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
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

