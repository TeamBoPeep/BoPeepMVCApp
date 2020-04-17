$('.update').hide();

$('.update-show').click(function (e) {
    $(this).parent().find("form").toggle();
})