xdbTracker = function () {
    function triggerEvent(id, data, itemid) {
        if (itemid) {
            sendToServer(id, { "id": id, "data": data, "itemid": itemid });
        } else {
            sendToServer(id, { "id": id, "data": data });
        }
    }
    function triggerOutcome(id, currency, value, type) {
        sendToServer(id, { "id": id, "outcomeCurrency": currency, "outcomeValue": value, "outcomeType": type, "type": "outcome" });
    }
    function sendToServer(id, payload) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", '/sitecore modules/web/xdbtracker/track.ashx', true);
        xhr.setRequestHeader("Content-Type", "application/json");
        var data = JSON.stringify(payload);
        xhr.send(data);
    }
    return {
        triggerEvent: triggerEvent,
        triggerOutcome: triggerOutcome
    }
}();