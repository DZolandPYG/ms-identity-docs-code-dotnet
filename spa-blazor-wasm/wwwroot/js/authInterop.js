//const msalConfig = {
//    auth: {
//        clientId: "333b0343-fda9-4e13-8eb0-038e877bef11",
//        authority: "https://login.microsoftonline.com/7d0e67e6-a37b-445e-82f0-6cc16260055e",
//        redirectUri: "http://localhost:5000/auth-callback"
//    },
//    cache: {
//        cacheLocation: "sessionStorage",
//        storeAuthStateInCookie: false // ✅ Helps preserve state across redirects
//    },
//    system: {
//        loggerOptions: {
//            loggerCallback: (level, message, containsPii) => {
//                console.log(`[MSAL] ${message}`);
//            },
//            piiLoggingEnabled: true,
//            logLevel: msal.LogLevel.Verbose
//        }
//    }
//};

//window.msalInstance = new msal.PublicClientApplication(msalConfig);

//window.authInterop = {
//    initialize: async function () {
//        console.log("Initializing MSAL...");
//        console.log("Full URL:", window.location.href);
//        console.log("Current location hash:", window.location.hash);

//        // Prevent multiple calls to handleRedirectPromise
//        if (!window._msalHandledRedirect) {
//            window._msalHandledRedirect = true;

//            try {
//                const response = await window.msalInstance.handleRedirectPromise();
//                console.log("handleRedirectPromise response:", response);

//                if (response && response.account) {
//                    console.log("Redirect login successful:", response.account.username);
//                    localStorage.setItem("msal.loggedIn", "true");
//                }

//                const accounts = window.msalInstance.getAllAccounts();
//                console.log("Accounts found:", accounts);

//                if (accounts.length > 0) {
//                    localStorage.setItem("msal.loggedIn", "true");
//                } else {
//                    localStorage.removeItem("msal.loggedIn");
//                }

//                // ✅ Now it's safe to rewrite the hash
//                if (window.location.hash.includes("code=")) {
//                    console.log("The hash replace triggered!!!");
//                    const query = window.location.hash.replace("#", "?");
//                    window.history.replaceState({}, "", window.location.pathname + query);
//                    console.log("After rewrite:", window.location.href);
//                }

//            } catch (error) {
//                console.error("Redirect login error:", error);
//                if (error instanceof msal.ClientAuthError) {
//                    console.error("ClientAuthError:", error.errorCode, error.errorMessage);
//                }
//                localStorage.removeItem("msal.loggedIn");
//            }
//        }

//    },

//    loginWithPrompt: async function () {
//        const loginState = localStorage.getItem("msal.loggedIn");
//        console.log("Login state:", loginState);

//        if (loginState === "true") {
//            console.log("Already logged in, skipping loginRedirect.");
//            return;
//        }

//        if (loginState === "pending") {
//            console.log("Login already in progress, skipping.");
//            return;
//        }

//        localStorage.setItem("msal.loggedIn", "pending");

//        setTimeout(() => {
//            window.msalInstance.loginRedirect({
//                scopes: ["openid", "profile", "email", "User.Read"],
//                prompt: "login"
//            });
//        }, 5000);
//    }
//};
