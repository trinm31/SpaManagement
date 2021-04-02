var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/Notifications/GetAll"
        },
        "columns":[
            {"data": "content", "width": "50%"},
            {"data": "date", "width": "30%"},
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}
