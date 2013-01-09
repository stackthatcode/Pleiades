﻿function CommerceFileUploader() {
    // TODO: move this into some kind of configuration
    $loadingImage = '/Pleiades/Content/fineuploader/loading.gif';
    $uploadEndpoint = '/Pleiades/Admin/Image/UploadFile';
    $downloadEndpoint = '/Pleiades/Admin/Image/DownloadFile/{exRID}?size=small';

    $fub = $('#fine-uploader-basic');
    $fubbusy = $('#fine-uploader-basic-busy');
    $messages = $('#messages');
    
    return new qq.FineUploaderBasic({
        button: $fub[0],
        request: {
            endpoint: $uploadEndpoint,
        },
        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'gif', 'png'],
            sizeLimit: 5242880 // 5 MB = 1024 * 1024 bytes * 5
        },
        callbacks: {            
            onSubmit: function (id, fileName) {
                $messages.children().remove();
                $fub.toggle(false);
                $fubbusy.toggle(true);
                $messages.append(
                    '<div id="file-' + id + '" class="alert" style="margin: 20px 0 0">' + '</div>');
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
                $fub.toggle(true);
                $fubbusy.toggle(false);
                if (responseJSON.success) {
                    $('#file-' + id).removeClass('alert-info')
                            .addClass('alert-success')
                            .html('<i class="icon-ok"></i> ' +
                                  '<button type="button" class="close" data-dismiss="alert">x</button>' +
                                  'Successfully saved ' +
                                  '“' + fileName + '”');

                } else {
                    $('#file-' + id).removeClass('alert-info')
                            .addClass('alert-error')
                            .html('<i class="icon-exclamation-sign"></i> ' +
                                  '<button type="button" class="close" data-dismiss="alert">x</button>' +
                                  'Error with ' +
                                  '“' + fileName + '”: ' +
                                  responseJSON.error);
                }
            }
        },
        debug: true
    });
}
