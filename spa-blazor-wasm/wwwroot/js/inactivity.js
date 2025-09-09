let inactivityTimer;
let inactivityLimit = 1 * 60 * 1000; // 1 minute for testing
let ttlExtension = 5 * 60 * 1000; // Extend TTL by 5 minutes on activity

function resetInactivityTimer() {
    console.log("Something happened! Resetting timer");

    // Refresh TTL for localStorage key
    updateExpiry("userSession", ttlExtension);

    clearTimeout(inactivityTimer);
    inactivityTimer = setTimeout(() => {
        console.log("Inactivity timeout reached. Notifying Blazor...");
        DotNet.invokeMethodAsync('BlazorWasm', 'HandleInactivityLogout');
    }, inactivityLimit);
}

function updateExpiry(key, newTTL) {
    const itemStr = localStorage.getItem(key);
    if (!itemStr) return;

    const item = JSON.parse(itemStr);
    item.expiry = Date.now() + newTTL;
    localStorage.setItem(key, JSON.stringify(item));
}

function startInactivityTracking() {
    window.onload = resetInactivityTimer;
    document.onclick = resetInactivityTimer;
    // You can uncomment these to track more types of activity:
    // document.onmousemove = resetInactivityTimer;
    // document.onkeypress = resetInactivityTimer;
    // document.onscroll = resetInactivityTimer;
}

// Initialize session key with TTL
localStorage.setItem("userSession", JSON.stringify({
    value: "active",
    expiry: Date.now() + ttlExtension
}));

startInactivityTracking();
