const sendMessageId = document.getElementById("sendmessageid");
if (sendMessageId) {
  sendMessageId.onclick = function() {
    chrome.tabs.query({ active: true, currentWindow: true }, function(tabs) {
      chrome.tabs.sendMessage(
        tabs[0].id,
        {
          tabId: tabs[0].id
        },
        function(response) {
          console.log("message with url sent");
          window.close();
        }
      );
    });
  };
}