var WorldGameWebsiteBridge = {
  GameReleaseFocus: function() {
    
    window.dispatchEvent(new CustomEvent("GameReleaseFocus", {}));
    //When in a iFrame
    if (window.parent) window.parent.postMessage({ type: "GameReleaseFocus" }, "*");
  },

  SendEventToBrowser: function(ptr) {
    var json = UTF8ToString(ptr); // Convert Unity string pointer → JS string

    if (window.parent)  window.parent.postMessage({ type: "UnityEvent", payload: json }, "*");
    
    var event = new CustomEvent("UnityEvent", { detail: json });
    window.dispatchEvent(event);
  },

  // Returns 1 if Web Share API is available, else 0
  IsShareAvailable: function() {
    try {
      return (typeof navigator !== 'undefined' && typeof navigator.share === 'function') ? 1 : 0;
    } catch (e) {
      return 0;
    }
  },
  
  Share: function(ptr) {
    try {
      var json = UTF8ToString(ptr);
      var data = {};
      try { data = JSON.parse(json || '{}'); } catch (e) { data = {}; }

      // Share via Web Share API; only send a single message (text field)
      if (typeof navigator !== 'undefined' && typeof navigator.share === 'function') 
      {
        var message = '';
        if (data && typeof data.text === 'string' && data.text.length > 0) {
          message = String(data.text);
        } else if (data && typeof data.url === 'string' && data.url.length > 0) {
          // If no text provided, fall back to url as the message
          message = String(data.url);
        }

        var shareData = {};
        if (message) shareData.text = message;

        navigator.share(shareData).catch(function(err){
          if (err && err.name !== 'AbortError') console.warn('navigator.share failed:', err);
        });
      }
      // Fall by copying to clipboard
      else if (data && (data.text || data.url)) 
      {
        var message = '';
        if (data && typeof data.text === 'string' && data.text.length > 0) {
          message = String(data.text);
        } else if (data && typeof data.url === 'string' && data.url.length > 0) {
          message = String(data.url);
        }
        var didCopy = false;
        if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
          navigator.clipboard.writeText(message).then(function(){ didCopy = true; }).catch(function(){});
        }
        try {
          var fallbackEvent = new CustomEvent('ShareFallback', { detail: { copied: didCopy, text: message } });
          window.dispatchEvent(fallbackEvent);
        } catch (e) {}
      }
    } catch (e) {
      console.warn('Share call failed:', e);
    }
  }
};
mergeInto(LibraryManager.library, WorldGameWebsiteBridge);
