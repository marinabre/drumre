$(document).ready( function() {
    $(".pagedEl").addClass("hidden");
    $("." + "1").removeClass("hidden");
    init_events();
});

function init_events() {
    $(document).on("click", ".pagingIndex", function () {
        var indx = this.textContent;
        $(".pagedEl").addClass("hidden");
        $("." + indx).removeClass("hidden");
});
}