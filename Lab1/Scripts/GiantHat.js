$(document).ready(function() {
	$("#Profile").prop("checked", true);
	$("#Friends").prop("checked", true);
});
    
    $(document).on("click",".Profile",function() {
        if ($(".ProfileDetails").hasClass("hidden")) {
            $("#Profile").prop("checked", true);
            $(".ProfileDetails").removeClass("hidden");
        }
        else {
            $("#Profile").prop("checked", false);
            $(".ProfileDetails").addClass("hidden");
        }        
    });
    $(document).on("click",".Friends",function() {
        if ($(".FriendsDetails").hasClass("hidden")) {
            $("#Friends").prop("checked", true);
            $(".FriendsDetails").removeClass("hidden");
        }
        else {
         $("#Friends").prop("checked", false);
            $(".FriendsDetails").addClass("hidden");
        }        
    });
    $(document).on("click",".Community",function() {
        if ($(".CommunityDetails").hasClass("hidden")) {
        $("#Community").prop("checked", true);
            $(".CommunityDetails").removeClass("hidden");
        }
        else {
        $("#Community").prop("checked", false);
            $(".CommunityDetails").addClass("hidden");
        }    });

     $(document).on("click",".Filter",function() {
        if ($(".FilterDetails").hasClass("hidden")) {
        $("#Filter").prop("checked", true);
            $(".FilterDetails").removeClass("hidden");
        }
        else {
        $("#Filter").prop("checked", false);
            $(".FilterDetails").addClass("hidden");
        }    });
  