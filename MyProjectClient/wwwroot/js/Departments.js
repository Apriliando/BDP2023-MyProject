$(document).ready(function () {
    //async function getNumberRows() {
    //    let response = await fetch('http://localhost:8081/api/Departments');
    //    let data = await response.json();
    //    let numRows = [];
    //    for (let i = 1; i <= data.length; i++){
    //        numRows.push(i);
    //        console.log(i);
    //    }
    //    return numRows;
    //};
    //var numberRows = getNumberRows();
    //console.log(numberRows);
    //debugger;
    $('#Tbl_Departments').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                text: "Tambah Departemen",
                attr: {
                    "id": "insertBtn",
                    "data-toggle": "modal",
                    "data-target": "#departmentModal",
                }
            },
            "copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            type: "GET",
            url: "http://localhost:8081/api/Departments",
            dataType: "JSON",
            dataSrc: "data",
            headers: { "Authorization": "Bearer " + sessionStorage.getItem("jwt") }, // 23 - 5 - 2023
            //success: function (response) { 
            //    console.log(response);
            //    //swal({
            //    //    title: response.status,
            //    //    text: response.message,
            //    //    icon: "success"
            //    //});
            //},
            error: function (response) {
                //console.log(response);
                //alert(response.message);
                swal({
                    title: response.status,
                    text: response.statusText,
                    icon: "error"
                })
                .then((errorAlert) => {
                    if (errorAlert)
                        location.replace("/Home/Login");
                    else
                        location.replace("/Home/Login");
                });
            },
            complete: function () {
                //alert("COMPLETE -> proses login selesai!");
            }
        },
        "columns": [
            { data: "id", orderable: false }, //sequence of id == sequence of fnRowCallback
            { data: "name" },
            {
                data: "id", "render": function (data) {
                    return '<button class="btn btn-warning updateBtn" data-toggle="modal" data-target="#departmentModal" data-deptid=' + data + '><i class="fas fa-edit"></i></button><button class="btn btn-danger" style="margin-left:12px;" onclick="deleteDept(' + data + ')"><i class="fas fa-trash-alt"></i></button>'
                },
                orderable: false
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            //var index = iDisplayIndex + 1;
            $('td:eq(0)', nRow).html(iDisplayIndexFull + 1);
            //return nRow;
        },
        "lengthChange": true,
        "autoWidth": false,
        "responsive": true
    });

    //t.on('order.dt search.dt', function () {
    //    let i = 1;
    //    t.cells(null, 0, { search: 'applied', order: 'applied' }).every(function (cell) {
    //        this.data(i++);
    //    });
    //}).draw();

    $("#insertBtn").on("click", function () {
        $("#insertBtn1").removeAttr("disabled");
        $("#updateBtn1").prop("disabled", true);
    });

    $(document).on("click", ".updateBtn", function () { //changed from &(".updateBtn")
        $("#updateBtn1").removeAttr("disabled");
        $("#insertBtn1").prop("disabled", true);
        var deptid = $(this).data("deptid");
        passDepartmentId(deptid);
    });

    $("#insertBtn1").on("click", function () {
        var name = $("#name").val();
        addDept(name);
    });

    function passDepartmentId(deptid) {
        $("#updateBtn1").on("click", function () {
            var name = $("#name").val();
            editDept(deptid, name);
        });
    }
})

function addDept(name) {
    var Department = { Name: name }
    $.ajax({
        type: "POST",
        url: "http://localhost:8081/api/Departments",
        dataType: "JSON",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(Department),
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("jwt") }, // 23 - 5 - 2023
        success: function (response) {
            console.log(response);
            //alert(response.message);
            swal({
                title: response.message,
                icon: "success"
            });
            $('#Tbl_Departments').DataTable().ajax.reload();
            $("#departmentModal").modal("hide");
        },
        error: function (response) {
            console.log(response);
            //alert(response.message);
            swal({
                title: response.message,
                text: "Coba lihat Console pada DevTools",
                icon: "error"
            });
        }
    });
    $("#name").val("");
}

function editDept(deptid, name) {
    var Department = { ID: deptid,  Name: name};
    $.ajax({
        type: "PUT",
        url: "http://localhost:8081/api/Departments",
        data: JSON.stringify(Department),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("jwt") }, // 23 - 5 - 2023
        success: function (response) {
            console.log(response);
            //alert(response.message);
            swal({
                title: response.message,
                icon: "success"
            });
            $('#Tbl_Departments').DataTable().ajax.reload();
            $("#departmentModal").modal("hide");
        },
        error: function (response) {
            console.log(response);
            //alert(response.message);
            swal({
                title: response.message,
                text: "Coba lihat Console pada DevTools",
                icon: "error"
            });
        }
    });
    $("#name").val("");
}

function deleteDept(data) {
    //if (confirm("Apakah anda yakin ingin menghapus data departemen ini?")) {
    swal({
        title: "Apakah anda yakin?",
        text: "Ingin menghapus departemen ini secara permanen?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
    .then((confirmDelete) => {
        if (confirmDelete) {
            $.ajax({
                type: "DELETE",
                url: "http://localhost:8081/api/Departments?ID=" + data,
                //data: JSON.stringify({ id: data }),
                //dataType: "json",
                //contentType: "application/json",
                headers: { "Authorization": "Bearer " + sessionStorage.getItem("jwt") }, // 23 - 5 - 2023
                success: function (response) {
                    console.log(response);
                    //alert(response.message);
                    swal({
                        title: response.message,
                        icon: "success"
                    });
                    $('#Tbl_Departments').DataTable().ajax.reload();
                },
                error: function (response) {
                    console.log(response);
                    //alert(response.message);
                    swal({
                        title: response.message,
                        text: "Coba lihat Console pada DevTools",
                        icon: "error"
                    });
                }
            });
        }
        else {
            swal("Penghapusan departemen dibatalkan!")
        }
    })
    //}
}