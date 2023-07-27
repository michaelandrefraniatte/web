chrome.runtime.onMessage.addListener(function(request, sender, sendResponse) {

    (function () {
        getScript('https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js', function () {
            $(".ytp-large-play-button:visible").click()
        });
        function getScript(url, success) {
            var script = document.createElement('script');
            script.src = url;
            var head = document.getElementsByTagName('head')[0],
                done = false;
            script.onload = script.onreadystatechange = function () {
                if (!done && (!this.readyState
                    || this.readyState == 'loaded'
                    || this.readyState == 'complete')) {
                    done = true;
                    success();
                    script.onload = script.onreadystatechange = null;
                    head.removeChild(script);
                }
            };
            head.appendChild(script);
        }
    })();

    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;

    var recognition = new SpeechRecognition();

    setInterval(() => { startVoicer(); }, 1000);

    function startVoicer() {
        try {
            recognition.start();
        }
        catch {}
    }

    function restartVoicer() {
        try {
            setTimeout(() => { recognition.start() }, 1000);
            recognition.stop();
        }
        catch {}
    }

    try {
        recognition.onspeechend = () => {
            restartVoicer();
        };
        recognition.onresult = (result) => {
            var str = result.results[0][0].transcript;
            if (str.length > 100) {
                restartVoicer();
            }
            if (str == "Next.") {
                try {
                    var element = $(".tiktok-1nncbiz-DivItemContainer:visible");
                    $("html, body").animate({ scrollTop: element.next().offset().top }, 'slow');
                }
                catch {}
                try {
                    var element = $("#navigation-button-down ytd-button-renderer yt-button-shape button");
                    element.click();
                    console.log("next video");
                }
                catch {}
            }
            if (str == "Previous.") {
                try {
                    var element = $(".tiktok-1nncbiz-DivItemContainer:visible");
                    $("html, body").animate({ scrollTop: element.prev().offset().top }, 'slow');
                }
                catch {}
                try {
                    var element = $("#navigation-button-up ytd-button-renderer yt-button-shape button");
                    element.click();
                    console.log("previous video");
                }
                catch {}
            }
            if (str == "Like.") {
                try {
                    var element = $(".tiktok-1ok4pbl-ButtonActionItem:visible");
                    element.click();
                }
                catch {}
                try {
                    var element = $("#like-button yt-button-shape label button").not("#dislike-button yt-button-shape label button");
                    element.click();
                    console.log("like video");
                }
                catch {}
            }
            if (str == "Unlike.") {
                try {
                    var element = $(".tiktok-1ok4pbl-ButtonActionItem:visible");
                    element.click();
                }
                catch {}
                try {
                    var element = $("#like-button yt-button-shape label button");
                    element.click();
                    console.log("dislike video");
                }
                catch {}
            }
            if (str == "Play.") {
                try {
                    var element = $(".tiktok-1nncbiz-DivItemContainer");
                    $("html, body").animate({ scrollTop: element.next().offset().top }, 'slow');
                }
                catch {}
                try {
                    var element = $(".ytp-large-play-button");
                    element.click();
                    console.log("play or pause video");
                }
                catch {}
            }
        };
    }
    catch {}

    sendResponse({ fromcontent: "This message is from content.js" });
    
});