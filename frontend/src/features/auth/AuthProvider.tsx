import { useEffect, useState } from "react"
import type {
    LoginRequest,
    LoginResponse,
    MeResponse,
} from "./types/auth.types"
import * as authService from "./services/authService"
import { AuthContext } from "./AuthContext"
import type { AuthContextValue } from "./AuthContext"

const TOKEN_KEY = "access_token"

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<MeResponse | null>(null)
    const [accessToken, setAccessToken] = useState<string | null>(
        localStorage.getItem(TOKEN_KEY),
    )
    const [isLoading, setIsLoading] = useState(true)

    const isAuthenticated = Boolean(accessToken)

    async function login(payload: LoginRequest): Promise<void> {
        const result: LoginResponse = await authService.login(payload)
        localStorage.setItem(TOKEN_KEY, result.accessToken)
        setAccessToken(result.accessToken)

        const me = await authService.me(result.accessToken)
        setUser(me)
    }

    function logout() {
        localStorage.removeItem(TOKEN_KEY)
        setAccessToken(null)
        setUser(null)
    }

    useEffect(() => {
        async function restoreSession() {
            if (!accessToken) {
                setIsLoading(false)
                return
            }

            try {
                const me = await authService.me(accessToken)
                setUser(me)
            } catch {
                logout()
            } finally {
                setIsLoading(false)
            }
        }

        restoreSession()
    }, [])

    const value: AuthContextValue & { isLoading: boolean } = {
        user,
        accessToken,
        isAuthenticated,
        login,
        logout,
        isLoading,
    }

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
