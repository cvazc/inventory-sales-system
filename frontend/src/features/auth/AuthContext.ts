import { createContext } from 'react'
import type { LoginRequest, MeResponse } from './types/auth.types'

export interface AuthContextValue {
  user: MeResponse | null
  accessToken: string | null
  isAuthenticated: boolean
  login: (payload: LoginRequest) => Promise<void>
  logout: () => void
}

export const AuthContext = createContext<AuthContextValue | undefined>(undefined)