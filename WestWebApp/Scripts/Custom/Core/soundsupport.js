function playsound(index) {
    sendMessage('Playing sound: ' + index);
    if (index) {
        if ((index >= 1) && (index <= 3)) {
            var AudioElem = null;
            switch (index) {
                case 1:
                    AudioElem = document.getElementById("idsound-1");
                    break;
                case 2:
                    AudioElem = document.getElementById("idsound-2");
                    break;
                case 3:
                    AudioElem = document.getElementById("idsound-3");
                    break;
            }
            if (AudioElem) {
                AudioElem.play();
            }

        }
    }
}

function stopsound(index) {
    if (index) {
        if ((index >= 1) && (index <= 3)) {
            var AudioElem = null;
            switch (index) {
                case 1:
                    audio1Stopped = true;
                    AudioElem = document.getElementById("idsound-1");
                    break;
                case 2:
                    AudioElem = document.getElementById("idsound-2");
                    break;
                case 3:
                    AudioElem = document.getElementById("idsound-3");
                    break;
            }
            if (AudioElem) {
                AudioElem.pause();
                AudioElem.currentTime = 0;
            }

        }
    }
}

function hideaudio() {
    $("audio[id~='idsound']").hide();
}