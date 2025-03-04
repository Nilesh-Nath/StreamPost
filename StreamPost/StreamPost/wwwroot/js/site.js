$(document).ready(function () {
    let debounceTimer;

    $("#searchBox").on("input", function () {
        let query = $(this).val().trim();

        clearTimeout(debounceTimer);

        if (query.length > 0) {
            debounceTimer = setTimeout(() => {
                $.ajax({
                    url: "/Home/Search",
                    method: "GET",
                    data: { query: query },
                    success: function (response) {
                        $("#searchResults").empty().removeClass("d-none");

                        if (response.length > 0) {
                            response.forEach(item => {
                                $("#searchResults").append(`
                                    <li class="list-group-item search-result-item" data-id="${item.postID}">
                                        ${item.title}
                                    </li>
                                `);
                            });
                        } else {
                            $("#searchResults").append(`<li class="list-group-item text-muted">No results found</li>`);
                        }
                    },
                    error: function () {
                        $("#searchResults").empty().addClass("d-none");
                    }
                });
            }, 300);
        } else {
            $("#searchResults").empty().addClass("d-none");
        }
    });

    $(document).on("click", ".search-result-item", function () {
        let postId = $(this).data("id");
        window.location.href = "/Home/PostDetails?id=" + postId; 
    });

    $(document).click(function (e) {
        if (!$(e.target).closest("#searchBox, #searchResults").length) {
            $("#searchResults").empty().addClass("d-none");
        }
    });
});
