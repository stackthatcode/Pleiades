function CommerceFileUploader() {
    $fub = $('#fine-uploader-basic');
    $messages = $('#messages');

    // TODO: move this into some kind of configuration
    $loadingImage = '/Pleiades/Content/fineuploader/loading.gif';
    $uploadEndpoint = '/Pleiades/Admin/Image/UploadFile';

    return new qq.FineUploaderBasic({
        button: $fub[0],
        request: {
            endpoint: $uploadEndpoint,
        },
        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'gif', 'png'],
            sizeLimit: 204800 // 200 kB = 200 * 1024 bytes
        },
        callbacks: {
            onSubmit: function (id, fileName) {
                $messages.append('<div id="file-' + id + '" class="alert" style="margin: 20px 0 0"></div>');
            },
            onUpload: function (id, fileName) {
                $('#file-' + id).addClass('alert-info')
                          .html('<img src="' + $loadingImage + '" alt="Initializing. Please hold."> ' +
                                'Initializing ' +
                                '“' + fileName + '”');
            },
            onProgress: function (id, fileName, loaded, total) {
                if (loaded < total) {
                    progress = Math.round(loaded / total * 100) + '% of ' + Math.round(total / 1024) + ' kB';
                    $('#file-' + id).removeClass('alert-info')
                          .html('<img src="' + $loadingImage + '" alt="Initializing. Please hold."> ' +
                                  'Uploading ' +
                                  '“' + fileName + '” ' +
                                  progress);
                } else {
                    $('#file-' + id).addClass('alert-info')
                          .html('<img src="' + $loadingImage + '" alt="Initializing. Please hold."> ' +
                                  'Saving ' +
                                  '“' + fileName + '”');
                }
            },
            onComplete: function (id, fileName, responseJSON) {
                if (responseJSON.success) {
                    alert(responseJSON);
                    $('#file-' + id).removeClass('alert-info')
                            .addClass('alert-success')
                            .html('<i class="icon-ok"></i> ' +
                                  'Successfully saved ' +
                                  '“' + fileName + '”');
                } else {
                    $('#file-' + id).removeClass('alert-info')
                            .addClass('alert-error')
                            .html('<i class="icon-exclamation-sign"></i> ' +
                                  'Error with ' +
                                  '“' + fileName + '”: ' +
                                  responseJSON.error);
                }
            }
        },
        debug: true
    });
}
