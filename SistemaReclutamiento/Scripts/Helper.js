function ProgressBarModal(showHide) {
    if (showHide === 'show') {
        $('#mod-progress').modal('show')
        if (arguments.length >= 2) {
            $('#progressBarParagraph').text(arguments[1])

        } else {
            $('#progressBarParagraph').text('U sdasdas')
        }
        window.ProgressBarActive = true
    } else {
        $('#mod-progress').modal('hide')
        window.ProgressBarActive = false
    }
}