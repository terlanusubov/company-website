﻿ $(document).ready(function () {
    $(".delete-button").click(function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var location = $(this).data("refresh");
        var url = $(this).data("url");
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success',
                cancelButton: 'btn btn-danger'
            },
            buttonsStyling: false
        });

        swalWithBootstrapButtons.fire({
            title: 'Silmək istədiyinizə əminsiniz?',
            text: "Bu geri dönə bilməyəcəyiniz əməliyyatdır!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Bəli, əminəm!',
            cancelButtonText: 'Xeyr, ləğv et!',
            reverseButtons: true
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: url + $(this).data("id"),
                    dataType: "json",
                    type: "post",
                    success: function (response) {
                        if (response.status === 200) {
                            swalWithBootstrapButtons.fire(
                                'Uğurlu!',
                                'Əməliyyat uğurla tamamlandı!',
                                'success'
                            ).then((r) => {
                                if (r.value) {
                                    window.location.href = location;
                                }
                            });


                        }
                        else {
                            swalWithBootstrapButtons.fire(
                                'Xəta!',
                                'Bir xəta səbəbilə əməliyyat tamamlanmadı!',
                                'error'
                            );
                        }
                    }
                });

            } else if (
                /* Read more about handling dismissals below */
                result.dismiss === Swal.DismissReason.cancel
            ) {
                swalWithBootstrapButtons.fire(
                    'Xəta!',
                    'Əməliyyat ləğv edildi!',
                    'error'
                );
            }
        });
    });

    $("#choose-language").change(function () {
        ChangeForm($(this).val());
    });

    var photos = [];
    //Phohto Upload

    //ON INPUT CHANGE PHOTO PROCESS
    $(".photo-upload-input").change(function () {
        let files = $(this).get(0).files;
        let status = $(this).data("single");
        Swal.fire({
            title: 'Əminsiniz?',
            text: "Bu şəkillər sayta yüklənəcək.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Bəli, əminəm!',
            cancelButtonText: 'Xeyr'
        }).then((result) => {
            if (result.value) {
                console.log(status)
                if (status) {
                    photos = [];
                    $("#photos option").remove();
                    if ($(".image-view li img").data("delete")) {
                        AddToDeleteSelect($(".image-view li img").data("name"));
                    }
                    $(".image-view li").remove();
                }
                UpdatePhotosFromInput(files);
                ShowPhotos(files);
                AddToSelect(files);
                Swal.fire(
                    'Uğurlu!',
                    'Əməliyyat uğurla tamamlandı!',
                    'success'
                );
            }
        });
    });
    //DELETE PHOTO
    $(".image-view ul").on("click", "li span", function (e) {
        Swal.fire({
            title: 'Əminsiniz?',
            text: "Bu şəkil siyahıdan silinəcək!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Bəli, əminəm!',
            cancelButtonText: 'Xeyr'
        }).then((result) => {
            if (result.value) {
                let name = $(this).parent().find("img").data("name");
                DeletePhotoFromArray(name);
                DeletePhotoFromSelect(name);
                console.log(photos);
                $(this).parent().remove();
                Swal.fire(
                    'Uğurlu!',
                    'Əməliyyat uğurla tamamlandı!',
                    'success'
                );
            }
        });
    });
    //SUBMIT
    $(".submit-button").click(function (e) {
        e.preventDefault();
        Swal.fire(
            'Gözləyin!',
            'Əməliyyat davam edir!',
            'warning'
        );
        UploadPhotoToServer($(this), $(this).parent("form"));
        Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Əməliyyat uğurla tamamlandı!',
            showConfirmButton: false,
            timer: 1500
        });
        $(this).parents("form").submit();
    });

    function UpdatePhotosFromInput(files) {
        for (var file of files) {
            photos.push(file);
        }
        return true;
    }
    function DeletePhotoFromArray(photo_name) {
        let element = `<option selected value="${photo_name}"></option>`;
        $("#delete-photos").append(element);

        for (var i = 0; i < photos.length; i++) {
            if (photos[i].name == photo_name) {
                photos.splice(i, 1);
            }
        }

    }
    function AddToDeleteSelect(photo_name) {
        let element = `<option selected value="${photo_name}"></option>`;
        $("#delete-photos").append(element);
    }
    function DeletePhotoFromSelect(photo_name) {
        let options = $("#photos option");
        for (let option of options) {
            if (option.value == photo_name) {
                $("#photos option[value='" + option.value + "']").remove();
            }
        }
    }
    function ShowPhotos(files) {
        for (let photo of files) {
            let fileReader = new FileReader();
            fileReader.readAsDataURL(photo);

            fileReader.onload = function () {
                let element = ` <li>
                    <span class="btn btn-danger delete-photo-button">Sil</span>
                    <img class="imageView" data-name="${photo.name}" src="${fileReader.result}" alt="Alternate Text" />
                </li>`;

                $(".image-view ul").append(element);
            };
        }
        return true;
    }
    function AddToSelect(files) {
        for (var file of files) {
            let option = `<option selected value="${file.name}"></option>`;
            $("#photos").append(option);
        }
    }
    function UploadPhotoToServer(submitButton, submitForm) {
        let path = submitButton.data("path");
        let formData = new FormData();

        for (var file of photos) {
            formData.append("Photos", file);
        }
        formData.append("folder", path);

        $.ajax({
            url: "/Admin/Photo/Upload",
            data: formData,
            dataType: "json",
            type: "post",
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
            }
        });
    }
    //ChangeForm($("#choose-language").val());
    function ChangeForm(key) {
        var formElement = $(key);
        $(".form-element").removeClass("active");
        formElement.addClass("active");
    }
});