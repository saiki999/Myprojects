$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Employee/GetEmployeeList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';

                html += '<td>' + item.EmployeeId + '</td>';
                html += '<td>' + item.EmployeeFirstName + '</td>';
                html += '<td>' + item.EmployeeLastName + '</td>';
                html += '<td>' + item.Location + '</td>';
                html += '<td>' + item.Department + '</td>';
                html += '<td>' + item.Email + '</td>';
                html += '<td>' + item.Phone + '</td>';
                html += '<td>' + item.EmployeePhotoUrl + '</td>';

                html += '<td><a href="#" onclick="return Details(' + item.Id + ')">Edit</a> | <a href="#" onclick="Delele(' + item.Id + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Data Function   
function Add() {
    //var res = validate();
    //if (res == false) {
    //    return false;
    //}
    var empObj = {
        Id: $('#Id').val(),
        EmployeeId: $('#EmployeeId').val(),
        EmployeeFirstName: $('#EmployeeFirstName').val(),
        EmployeeLastName: $('#EmployeeLastName').val(),
        Location: $('#Location').val(),
        Department: $('#Department').val(),
        Email: $('#Email').val(),
        Phone: $('#Phone').val(),
        EmployeePhotoUrl: $('#EmployeePhotoUrl').val()
    };
    $.ajax({
        url: "/Employee/Create",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID  
function Details(EmpID) {
    $('#Id').css('border-color', 'lightgrey');
    $('#EmployeeId').css('border-color', 'lightgrey');
    $('#EmployeeFirstName').css('border-color', 'lightgrey');
    $('#EmployeeLastName').css('border-color', 'lightgrey');
    $('#Location').css('border-color', 'lightgrey');
    $('#Department').css('border-color', 'lightgrey');
    $('#Email').css('border-color', 'lightgrey');
    $('#Phone').css('border-color', 'lightgrey');
    $('#EmployeePhotoUrl').css('border-color', 'lightgrey');

    $.ajax({
        url: "/Employee/Details/" + EmpID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#EmployeeId').val(result.EmployeeId);
            $('#EmployeeFirstName').val(result.EmployeeFirstName);
            $('#EmployeeLastName').val(result.EmployeeLastName);
            $('#Location').val(result.Location);
            $('#Department').val(result.Department);
            $('#Email').val(result.Email);
            $('#Phone').val(result.Phone);
            $('#EmployeePhotoUrl').val(result.EmployeePhotoUrl);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record  
function Update() {
    //var res = validate();
    //if (res == false) {
    //    return false;
    //}
    var empObj = {
        Id: $('#Id').val(),
        EmployeeId: $('#EmployeeId').val(),
        EmployeeFirstName: $('#EmployeeFirstName').val(),
        EmployeeLastName: $('#EmployeeLastName').val(),
        Location: $('#Location').val(),
        Department: $('#Department').val(),
        Email: $('#Email').val(),
        Phone: $('#Phone').val(),
        EmployeePhotoUrl: $('#EmployeePhotoUrl').val()
    };
    $.ajax({
        url: "/Employee/Edit",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#Id').val("");
            $('#EmployeeId').val("");
            $('#EmployeeFirstName').val("");
            $('#EmployeeLastName').val("");
            $('#Location').val("");
            $('#Department').val("");
            $('#Email').val("");
            $('#Phone').val("");
            $('#EmployeePhotoUrl').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record  
function Delele(Id) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Employee/Delete/" + Id,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
                alert("1");
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#EmployeeID').val("");
    $('#Name').val("");
    $('#Age').val("");
    $('#State').val("");
    $('#Country').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#Age').css('border-color', 'lightgrey');
    $('#State').css('border-color', 'lightgrey');
    $('#Country').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
//function validate() {
//    var isValid = true;
//    if ($('#Name').val().trim() == "") {
//        $('#Name').css('border-color', 'Red');
//        isValid = false;
//    }
//    else {
//        $('#Name').css('border-color', 'lightgrey');
//    }
//    if ($('#Age').val().trim() == "") {
//        $('#Age').css('border-color', 'Red');
//        isValid = false;
//    }
//    else {
//        $('#Age').css('border-color', 'lightgrey');
//    }
//    if ($('#State').val().trim() == "") {
//        $('#State').css('border-color', 'Red');
//        isValid = false;
//    }
//    else {
//        $('#State').css('border-color', 'lightgrey');
//    }
//    if ($('#Country').val().trim() == "") {
//        $('#Country').css('border-color', 'Red');
//        isValid = false;
//    }
//    else {
//        $('#Country').css('border-color', 'lightgrey');
//    }
//    return isValid;
//}  