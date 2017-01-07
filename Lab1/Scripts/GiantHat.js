    $(document).ready(function() {
        $(".ProfileDetails").addClass("hidden");
    });
     $(document).ready(function() {
        $(".FriendsDetails").addClass("hidden");
    });
     $(document).ready(function() {
        $(".CommunityDetails").addClass("hidden");
    });

    $(document).on("click",".Profile",function() {
        if ($(".ProfileDetails").hasClass("hidden")) {
            $(".ProfileDetails").removeClass("hidden");
        }
        else {
            $(".ProfileDetails").addClass("hidden");
        }        
    });
    $(document).on("click",".Friends",function() {
        if ($(".FriendsDetails").hasClass("hidden")) {
            $(".FriendsDetails").removeClass("hidden");
        }
        else {
            $(".FriendsDetails").addClass("hidden");
        }        
    });
    $(document).on("click",".Community",function() {
        if ($(".CommunityDetails").hasClass("hidden")) {
            $(".CommunityDetails").removeClass("hidden");
        }
        else {
            $(".CommunityDetails").addClass("hidden");
        }        
    });