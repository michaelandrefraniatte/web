chrome.runtime.onMessage.addListener(function(request, sender, sendResponse) {

    (function () {

        getScript('https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js', function () {

            $(".ytp-large-play-button").not(".ytp-large-play-button:visible").click();
    
            var elements = document.getElementsByTagName("yt-formatted-string");
            var strtemp = [];
            var speecher = "";
        
            for (i = 0; i < elements.length; i++) {
                try {
                    if (elements[i].id == "content-text") {
                        var str = elements[i].innerText;
                        str = str.replaceAll('"', ' ').replaceAll('/', ' ');
                        if (!str.includes("http:") & !str.includes("https:") & !str.includes("www.") & !str.includes("@hotmail.com") & !str.includes("@gmail.com") & str != null & str.length > 10) {
                            if (!strtemp.includes(str)) {
                                strtemp.push(str);
                                speecher += str + ". ";
                            }
                        }
                    }
                }
                catch {}
            }
            
            wrapper(speecher);

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

    function wrapper(str) {
        try {
            var msg = new SpeechSynthesisUtterance();
            msg.text = str;
            window.speechSynthesis.speak(msg);
        }
        catch {}
    }

    sendResponse({ fromcontent: "This message is from content.js" });
    
});