import React, { createContext, useContext, useEffect, useState } from "react";

interface ClaimValue {
    type: string;
    value: string;
}

interface UserInfoResponse {
    isAuthenticated: boolean;
    nameClaimType: string;
    roleClaimType: string;
    claims: ClaimValue[];
}

// Shape we use in the app
interface User {
    email: string;
    name?: string;
}

interface AuthContextValue {
    isAuthenticated: boolean;
    user: User | null;
    login: () => void;
    logout: () => void;
    loading: boolean; // Added loading state
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export const AuthProvider: React.FC<React.PropsWithChildren<{}>> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true); // Track loading state
    const isAuthenticated = Boolean(user);

    useEffect(() => {
        fetch("/api/user", { credentials: "include" })
            .then(res => {
                console.log("API Response:", res);
                return res.json();
            })
            .then((info: UserInfoResponse) => {
                console.log("Parsed User Info:", info);

                if (info.isAuthenticated && info.claims) {
                    // Extract user information from claims
                    const emailClaim = info.claims.find(claim =>
                        claim.type === "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" ||
                        claim.type === "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" ||
                        claim.type === info.nameClaimType
                    );

                    const nameClaim = info.claims.find(claim =>
                        claim.type === "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname" ||
                        claim.type === "name"
                    );

                    if (emailClaim) {
                        setUser({
                            email: emailClaim.value,
                            name: nameClaim?.value
                        });
                    }
                } else {
                    setUser(null);
                }
            })
            .catch(err => {
                console.error("Fetch failed:", err);
                setUser(null);
            })
            .finally(() => {
                setLoading(false);
            });
    }, []);

    const login = () => {
        // redirect to our ASP.NET Core login endpoint
        window.location.href = `/login?returnUrl=${encodeURIComponent(window.location.pathname)}`;
    };

    const logout = () => {
        // construct a URL including the returnUrl
        const returnUrl = encodeURIComponent(window.location.pathname);
        window.location.href = `/logout?returnUrl=${returnUrl}`;
    };


    return (
        <AuthContext.Provider value={{ isAuthenticated, user, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = (): AuthContextValue => {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("useAuth must be used within an AuthProvider");
    return ctx;
};