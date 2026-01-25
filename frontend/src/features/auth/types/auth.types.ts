export type UserRole = "Admin" | "Clerk"

export interface LoginRequest {
    email: string
    password: string
}

export interface LoginResponse {
    accessToken: string
    email: string
    role: UserRole
}

export interface MeResponse {
    userId: string
    email: string
    role: UserRole
}
