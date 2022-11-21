function BindCategoryNavItems() {
    $("a[category-id]").click(function () {
        let item = $(this).closest(".category-navbar-item");
        let id = $(this).attr('category-id');

        $("#searchCategory").val(id);

        $.get("/Home/GetPostsHtml?category=" + id, function(data) {
            $("#postsHtmlBody").html(data);

            $(".category-navbar-item").removeClass("active");
            item.addClass("active");

            let articleNavs = $(".article-navbar-item a[post-id]");
            if (articleNavs.length)
            {
                articleNavs.first().click();
            }
            else
            {
                $.get("/Home/GetPostHtml?post=-1", function(data) {
                    $("#postHtmlBody").html(data);
                });
            }
        });
    });
}

function BindPostNavItems() {
    $("a[post-id]").click(function () {
        let item = $(this).closest(".article-navbar-item");
        let id = $(this).attr('post-id');

        $.get("/Home/GetPostHtml?post=" + id, function(data) {
            $("#postHtmlBody").html(data);

            $(".article-navbar-item").removeClass("active");
            item.addClass("active");
        });
    });
}

function BindСourseControlButtonsTest() {
    $("#courseCreateButton").click(function () {
        $("#courseEditDialog input[name='courseId']").attr("value", "-1");
        $("#courseEditDialog input[name='courseTitle']").attr("value", "");
        $("#courseEditDialog").modal();
    });
    $(".courseDeleteButton").click(function () {
        let id = $(this).attr('courseId');

        $("#courseDeleteDialog a").attr("href", "/Home/DeleteCategory?catId=" + id);
        $("#courseDeleteDialog").modal();
    });
    $(".courseEditButton").click(function () {
        let id = $(this).attr('courseId');
        let selectedcourseItem = $('div[courseId=' + id + '] a').text();
        // let selectedcourseTitle = selectedcourseItem.find('a').text();

        $("#courseEditDialog input[name='courseId']").attr("value", id);
        $("#courseEditDialog input[name='courseTitle']").attr("value", selectedcourseItem);
        $("#courseEditDialog").modal();
    });
}

function BindCourseControlButtons() {
    $("#courseCreateButton").click(function () {
        $("#courseEditDialog input[name='courseId']").attr("value", "-1");
        $("#courseEditDialog input[name='courseTitle']").attr("value", "");
        $("#courseEditDialog textarea[name='courseDescription']").val("");

        $("#courseEditDialog").modal();
    });
    $(".courseDeleteButton").click(function () {
        let selectedPostId = $(this).attr('courseId');

        $("#courseDeleteDialog a").attr("href", "/Admin/DeleteCourse?courseId=" + selectedPostId);
        $("#courseDeleteDialog").modal();
    });
    $(".courseEditButton").click(function () {
        let selectedCourseId = $(this).attr('courseId');
        let selectedCourseItem = $('div[post-id=' + selectedCourseId + ']');
        let selectedCourseTitle = selectedCourseItem.find('h5 a').text();
        let selectedCourseContents = selectedCourseItem.find('p').text();
        let enable = selectedCourseItem.find('p').text();


        $("#courseEditDialog input[name='courseId']").attr("value", selectedCourseId);
        $("#courseEditDialog input[name='courseTitle']").attr("value", selectedCourseTitle);
        $("#courseEditDialog textarea[name='courseDescription']").val(selectedCourseContents);
        $("#courseEditDialog textarea[name='enable']").val(selectedCourseContents);


        $("#courseEditDialog").modal();
    });
}