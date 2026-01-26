import { Navigate } from 'react-router-dom'
import { useAuth } from '../../features/auth/useAuth'

export function RequireAuth({ children }: { children: JSX.Element }) {
  const { isAuthenticated, isLoading } = useAuth()

  if (isLoading) return null
  if (!isAuthenticated) return <Navigate to="/login" replace />

  return children
}
