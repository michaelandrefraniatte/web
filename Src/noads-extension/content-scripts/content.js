chrome.runtime.onMessage.addListener(function(request, sender, sendResponse) {
    
    var playButtonFinderInterval = '';
    var skipMovieFinderInterval = '';
    var closeBannerFinderInterval = '';

    (function () {
        getScript('https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js', function () {
            $(document).ready(function () {
                removeAds();
                playButtonFinderInterval = setInterval(() => {
                    var playButton = getPlayButton();
                    if (playButton) {
                        playButton.click();
                    }
                }, 100);
                skipMovieFinderInterval = setInterval(() => {
                    var skipButton = getSkipButton();
                    if (skipButton) {
                        skipButton.click();
                    }
                }, 100);
                closeBannerFinderInterval = setInterval(() => {
                    var closeButton = getCloseButton();
                    if (closeButton) {
                        closeButton.click();
                    }
                }, 100);
            });
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

    function removeAds() {
        try {
            var els = document.getElementsByClassName("video-ads ytp-ad-module");
            for (var i=0;i<els.length; i++) {
                els[i].click();
            }
            document.cookie = "VISITOR_INFO1_LIVE = oKckVSqvaGw; path =/; domain =.youtube.com";
            var cookies = document.cookie.split("; ");
            for (var i = 0; i < cookies.length; i++)
            {
                var cookie = cookies[i];
                var eqPos = cookie.indexOf("=");
                var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
            }
            var el = document.getElementsByClassName("ytp-ad-skip-button");
            for (var i=0;i<el.length; i++) {
                el[i].click();
            }
            var element = document.getElementsByClassName("ytp-ad-overlay-close-button");
            for (var i=0;i<element.length; i++) {
                element[i].click();
            }
            var scripts = document.getElementsByTagName("script");
            for (let i = 0; i < scripts.length; i++)
            {
                var content = scripts[i].innerHTML;
                if (content.indexOf("ytp-ad") > -1) {
                    scripts[i].innerHTML = "";
                }
                var src = scripts[i].getAttribute("src");
                if (src.indexOf("ytp-ad") > -1) {
                    scripts[i].setAttribute("src", "");
                }
            }
            var iframes = document.getElementsByTagName("iframe");
            for (let i = 0; i < iframes.length; i++)
            {
                var content = iframes[i].innerHTML;
                if (content.indexOf("ytp-ad") > -1) {
                    iframes[i].innerHTML = "";
                }
                var src = iframes[i].getAttribute("src");
                if (src.indexOf("ytp-ad") > -1) {
                    iframes[i].setAttribute("src", "");
                }
            }
            var allelements = document.querySelectorAll("*");
            for (var i = 0; i < allelements.length; i++) {
                var classname = allelements[i].className;
                if (classname.indexOf("ytp-ad") > -1)  {
                        allelements[i].innerHTML = "";
                }
            }
            var players = document.getElementById("movie_player");
            for (let i = 0; i < players.length; i++)
            {
                players.classList.remove("ad-showing");
                players.classList.remove("ad-interrupting");
                players.classList.remove("playing-mode");
                players.classList.remove("ytp-autohide");
                players.classList.add("ytp-hide-info-bar");
                players.classList.add("playing-mode");
                players.classList.add("ytp-autohide");
            }
        }
        catch { }
        setTimeout(removeAds, 3000);
    }

    function getPlayButton() {
        return $(".ytp-large-play-button:visible");
    }

    function getSkipButton() {
        return $(".ytp-ad-skip-button");
    }

    function getCloseButton() {
        return $(".ytp-ad-overlay-close-button");
    }
    
    var stringinject = `
    <style>
        .ad-showing, .ad-container, .ytp-ad-overlay-open, .video-ads, .ytp-ad-overlay-image, .ytp-ad-overlay-container {
            display: none !important;
        }
    </style>`;
    document.getElementsByTagName('head')[0].innerHTML += stringinject;

    sendResponse({ fromcontent: "This message is from content.js" });
    
});