$(document).ready(function () {
    $("#LikeBtn").on("click", function (e) {
        e.preventDefault();
        var postID = $(this).attr("data-postid");
        var btn = $("#LikeBtn");

        var isLiked = btn.hasClass("btn-danger");
        btn.toggleClass("btn-danger text-white btn-outline-secondary");

        if (isLiked) {
            btn.html(`<i class="bi bi-hand-thumbs-up me-1"></i> Like`);
        } else {
            btn.html(`<i class="bi bi-hand-thumbs-up-fill me-1"></i> Liked`);
        }

        $.ajax({
            method: "POST",
            url: "/Home/LikePost/",
            data: { PostID: postID },
            success: function (response) {
                if (!response.success) {
                    console.log("Failure");
                    btn.toggleClass("btn-danger text-white btn-outline-secondary");

                    btn.html(isLiked ?
                        `<i class="bi bi-hand-thumbs-up-fill me-1"></i> Liked` :
                        `<i class="bi bi-hand-thumbs-up me-1"></i> Like`
                    );
                } else {
                    $("#likeNumber").html(`<small id="likeNumber" class="text-secondary">
                    <i class="bi bi-star me-1"></i> ${response.likeNumber} Likes
                </small>`);
                }
            },
            error: function () {
                console.log("Error!");
                btn.toggleClass("btn-danger text-white btn-outline-secondary");
                btn.html(isLiked ?
                    `<i class="bi bi-hand-thumbs-up-fill me-1"></i> Liked` :
                    `<i class="bi bi-hand-thumbs-up me-1"></i> Like`
                );
            }
        });
    });

    $("#CommentForm").on("submit", function (e) {
        e.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            method: "POST",
            url: "/Home/CommentPost/",
            data: formData,
            success: function (response) {
                if (response.success) {
                    $("#commentNumber").html(`<small id="commentNumber" class="text-secondary">
                        <i class="bi bi-chat-dots me-1"></i> ${response.commentNumber} Comments
                    </small>`);
                    const successToast = `
                    <div class="toast align-items-center border-0 bg-success mt-2 position-fixed top-0 end-0 text-white fw-2 z-1" role="alert">
                         <div class="d-flex">
                             <div class="toast-body">Commented !</div>
                             <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast"></button>
                         </div>
                     </div>
                    `;
                    $('body').append(successToast);
                    const toastElement = $('.toast').last();
                    const bsToast = new bootstrap.Toast(toastElement);

                    bsToast.show();

                    setTimeout(() => {
                        toastElement.toast('hide');
                        toastElement.remove();
                    }, 3000);
                    $("#CommentForm")[0].reset(); 
                    fetchComments();
                } else {
                    console.log(response.message);
                    const errorToast = `
                   <div class="toast align-items-center border-0 bg-danger mt-2  position-fixed top-0 end-0 text-white fw-2 z-1" role="alert">
                         <div class="d-flex">
                             <div class="toast-body text-white">An error occurred while commenting!</div>
                             <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast"></button>
                         </div>
                     </div>
                    `;
                    $('body').append(errorToast);
                    const toastElement = $('.toast').last();
                    const bsToast = new bootstrap.Toast(toastElement);

                    bsToast.show();

                    setTimeout(() => {
                        toastElement.toast('hide');
                        toastElement.remove();
                    }, 3000);
                }
            },
            error: function () {
                console.log("Something went wrong!");
                const errorToast = `
                   <div class="toast align-items-center border-0 bg-danger mt-2  position-fixed top-0 end-0 text-white fw-2 z-1" role="alert">
                         <div class="d-flex">
                             <div class="toast-body text-white">An error occurred while commenting!</div>
                             <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast"></button>
                         </div>
                     </div>
                    `;
                $('body').append(errorToast);
                const toastElement = $('.toast').last();
                const bsToast = new bootstrap.Toast(toastElement);

                bsToast.show();

                setTimeout(() => {
                    toastElement.toast('hide');
                    toastElement.remove();
                }, 3000);
            }
        });
    });

    function fetchComments() {
        var postID = $("input[name='PostID']").val();

        $.ajax({
            url: "/Home/GetComments/",
            method: "GET",
            data: { postID: postID },
            success: function (data) {
                if (data.success) {
                    updateCommentSection(data.comments, data.postAuthor);
                }
            },
            error: function () {
                console.log("Failed to fetch comments.");
            }
        });
    }

    function updateCommentSection(comments,postAuthor) {
        var commentsContainer = $(".comments-container"); 
        commentsContainer.empty(); 

        comments.forEach(function (comment) {
            var commentHtml = `
                <div class="p-3 mb-2 bg-white shadow-sm rounded d-flex justify-content-between">
                    <div>
                        <span class="fw-semibold text-dark">${comment.userName}</span>
                        <p class="text-muted mb-0">${comment.commentText}</p>
                    </div>
            `;
            if (comment.user == postAuthor) {
                commentHtml += `
                      <div class="badge text-dark">
                          <i class="bi bi-award"></i>
                          <span>Author</span>
                      </div>
                </div>
                `
            };

            commentsContainer.append(commentHtml);
        });
    }
});