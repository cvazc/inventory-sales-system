import type { ReactElement } from "react"
import { Navigate } from "react-router-dom"
import { useAuth } from "../../features/auth/useAuth"

export function RequireRole({
    role,
    children,
}: {
    role: "Admin" | "Clerk"
    children: ReactElement
}) {
    const { user } = useAuth()

    if (!user || user.role !== role) {
        return <Navigate to="/" replace />
    }

    return children
}
