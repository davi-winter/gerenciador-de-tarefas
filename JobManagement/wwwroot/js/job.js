function refreshFieldStatus() {
    var progressValue = document.getElementById('progress').value;

    if (progressValue == 0) {
        document.getElementById('status').value = 1;
    } else if (progressValue == 100) {
        document.getElementById('status').value = 3;
    } else {
        document.getElementById('status').value = 2;
    }
}

function refreshFieldProgress() {
    var statusValue = document.getElementById('status').value;
    var progressValue = document.getElementById('progress').value;

    if (statusValue == 1 && progressValue != 0) {
        document.getElementById('progress').value = 0;
    } else if (statusValue == 2 && progressValue == 0) {
        document.getElementById('progress').value = 1;
    } else if (statusValue == 2 && progressValue == 100) {
        document.getElementById('progress').value = 99;
    } else if (statusValue == 3 && progressValue != 100) {
        document.getElementById('progress').value = 100;
    }
}
