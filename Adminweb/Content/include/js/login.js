$(function(){
	var check = false;
	$("#rem").click(function(){
		check = !check;
		if(check)
			$("#rem .checkbox>i").removeClass("icon-check-empty").addClass("icon-check");
		else
			$("#rem .checkbox>i").removeClass("icon-check").addClass("icon-check-empty");
	});
})
