import { useState } from 'react'
import type { LoginRequest, LoginResponse, MeResponse } from './types/auth.types'
import * as authService from './services/authService'
import { AuthContext } from './AuthContext'
import type { AuthContextValue } from './AuthContext'

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<MeResponse | null>(null)
  const [accessToken, setAccessToken] = useState<string | null>(null)

  const isAuthenticated = Boolean(accessToken)

  async function login(payload: LoginRequest): Promise<void> {
    const result: LoginResponse = await authService.login(payload)

    setAccessToken(result.accessToken)

    const me = await authService.me(result.accessToken)
    setUser(me)
  }

  function logout() {
    setAccessToken(null)
    setUser(null)
  }

  const value: AuthContextValue = {
    user,
    accessToken,
    isAuthenticated,
    login,
    logout,
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}