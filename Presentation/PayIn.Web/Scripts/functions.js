$(function () {
    $("#submitbtn").click(function () {
        $("#loading").fadeIn();
    });
});
$(function () {
	$("#submitbtnLogin").click(function () {
		$("#loading").fadeIn();
	});
});
$(function () {
	$("#submitbtnReg").click(function () {
		$("#loading").fadeIn();
	});
});
$(function () {
    $("#errorDivBtn").click(function () {
        $("#errorDiv").fadeOut();
    });
});
function validateEmail(email){
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(email)) {
        return true;
    }
    else {
        return false;
    }
}

function checkfields() {

    var email = $('#email').val();
    if (email!=null && !validateEmail(email)) {
        return true;
    }
    
    var value = $('#password').val();
    if (value != null && value.length == 0) {
        return true;
    }
    value = $('#confirmPassword').val();
    if (value != null && value.length == 0) {
        return true;
    }
    value = $('#userId').val();
    if (value != null && value.length == 0) {
        return true;
    }
    value = $('#code').val();
    if (value != null && value.length == 0) {
        return true;
    }
    value = $('#name').val();
    if (value != null && value.length == 0) {
        return true;
    }
    value = $('#mobile').val();
    if (value != null && value.length == 0) {
        return true;
    }
    //value = $('#birthday').val();
    //if (value != null && value.length == 0) {
    //    return true;
    //}
}
function enabledBtn () {
    var empty = false;
    var validatePass = false;  

    empty = checkfields();
   

    if (empty) {
    	$('#submitbtn').attr('disabled', 'disabled');
    	$('#submitbtnReg').attr('disabled', 'disabled');

    }
    else {		
    	$('#submitbtn').removeAttr('disabled');
    }
}
$(document).ready(function () {
    $('.list-group-item input').blur(enabledBtn);
    $('.list-group-item input').keyup(enabledBtn);
    $('.list-group-item input').change(enabledBtn);
});