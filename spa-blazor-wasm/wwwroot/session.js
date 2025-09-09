window.revokeRefreshToken = async function () {
    const msalInstance = window.msalInstance; // Ensure this is globally available
    const request = {
        scopes: ["https://graph.microsoft.com/.default"]
    };

    try {
        const response = await msalInstance.acquireTokenSilent(request);
        const accessToken = response.accessToken;

        const revokeResponse = await fetch("https://graph.microsoft.com/v1.0/me/revokeSignInSessions", {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${accessToken}`,
                "Content-Type": "application/json"
            }
        });

        if (revokeResponse.ok) {
            console.log("Refresh tokens revoked successfully.");
        } else {
            console.error("Failed to revoke tokens:", revokeResponse.statusText);
        }
    } catch (error) {
        console.error("Error revoking tokens:", error);
    }
};
