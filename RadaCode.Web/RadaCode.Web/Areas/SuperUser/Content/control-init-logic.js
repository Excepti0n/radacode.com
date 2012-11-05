$(function () {

    var UsersInitialized = false;
    var ProjectsViewInitialized = false;

    $("#control-area").tabs({
        select: function (event, ui) {
            switch(ui.index)
            {
                case 1:
                    if (!UsersInitialized) {
                        UsersView.Init();
                        UsersInitialized = true;
                    }
                    break;
                case 0:
                    if(!ProjectsViewInitialized) {
                        ProjectsView.Init();
                        ProjectsViewInitialized = true;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
    });
})