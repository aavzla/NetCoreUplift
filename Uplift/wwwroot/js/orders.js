//This file is for handling calls to the API.

var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("approved")) {
        loadDataTable("GetAllApprovedOrders");
    }
    else if (url.includes("pending")) {
        loadDataTable("GetAllPendingOrders");
    }
    else {
        loadDataTable("GetAllOrders");
    }
});

function loadDataTable(url) {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/order/" + url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //"data" must have the name of the property that we want to use in the column, we are using camelCase.
            { "data": "name", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "email", "width": "15%" },
            { "data": "serviceCount", "width": "15%" },
            { "data": "status", "width": "15%" },
            {
                //this is the column for the button Details and we need the Id. That's why we use the id for "data"
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Order/Details/${data}" class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                    <i class='far fa-edit'></i> Details
                                </a>
                            </div>
                            `;
                }, "width": "15%"
            }
        ],
        "language": {
            "emptyTable": "No records found."
        },
        "width": "100%"
    });
}