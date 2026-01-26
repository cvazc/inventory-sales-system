import { createContext } from "react"

export type AuthUser = {
    email: string
    role: "Admin" | "Clerk"
}

export type AuthContextValue = {
    user: AuthUser | null
    accessToken: string | null
    isAuthenticated: boolean
    isLoading: boolean
    login: (payload: { email: string; password: string }) => Promise<void>
    logout: () => void
}

export const AuthContext = createContext<AuthContextValue | null>(null)
