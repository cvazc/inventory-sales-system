import type { ReactNode } from "react"
import { useEffect, useMemo, useState } from "react"
import { AuthContext, type AuthUser } from "./AuthContext"
import { request } from "../../shared/lib/apiClient"
import type { LoginRequest } from "./types/auth.types"

const ACCESS_TOKEN_KEY = "accessToken"

type LoginResponse = {
    accessToken: string
    email: string
    role: "Admin" | "Clerk"
}

export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<AuthUser | null>(null)
    const [accessToken, setAccessToken] = useState<string | null>(null)
    const [isLoading, setIsLoading] = useState(true)

    const isAuthenticated = !!accessToken

    useEffect(() => {
        // Restore session
        const token = localStorage.getItem(ACCESS_TOKEN_KEY)
        if (!token) {
            setIsLoading(false)
            return
        }

        setAccessToken(token)
        ;(async () => {
            try {
                // tu backend ya tiene /api/auth/me
                const me = await request<AuthUser>("/api/auth/me", {
                    method: "GET",
                    accessToken: token,
                })
                setUser(me)
            } catch {
                // token inválido/expirado
                localStorage.removeItem(ACCESS_TOKEN_KEY)
                setAccessToken(null)
                setUser(null)
            } finally {
                setIsLoading(false)
            }
        })()
    }, [])

    async function login(payload: LoginRequest) {
        const res = await request<LoginResponse>("/api/auth/login", {
            method: "POST",
            body: payload,
        })

        localStorage.setItem(ACCESS_TOKEN_KEY, res.accessToken)
        setAccessToken(res.accessToken)
        setUser({ email: res.email, role: res.role })
    }

    function logout() {
        localStorage.removeItem(ACCESS_TOKEN_KEY)
        setAccessToken(null)
        setUser(null)
    }

    const value = useMemo(
        () => ({
            user,
            accessToken,
            isAuthenticated,
            isLoading,
            login,
            logout,
        }),
        [user, accessToken, isAuthenticated, isLoading],
    )

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
