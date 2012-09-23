function InitializeAdminMenu() {
    /**
    * the menu
    */
    var $menu = $('#ldd_menu');

    /**
    * for each list element,
    * we show the submenu when hovering and
    * expand the span element (title) to 510px
    */
    $menu.children('li').each(function () {
        var $this = $(this);
        var $span = $this.children('span');
        $span.data('width', $span.width());

        $this.bind('mouseenter', function () {
            $menu.find('.ldd_submenu').stop(true, true).hide();
            //$span.stop().animate({'width':'510px'}, 300, function(){
            $this.find('.ldd_submenu').slideDown(300);
            //});
        }).bind('mouseleave', function () {
            $this.find('.ldd_submenu').stop(true, true).hide();
            $span.stop().animate({ 'width': $span.data('width') + 'px' }, 300);
        });
    });
}