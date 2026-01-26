import type { ReactElement } from "react"
import { Navigate } from "react-router-dom"
import { useAuth } from "../../features/auth/useAuth"

export function RequireAuth({ children }: { children: ReactElement }) {
    const { isAuthenticated, isLoading } = useAuth()

    if (isLoading) return null
    if (!isAuthenticated) return <Navigate to="/login" replace />

    return children
}
