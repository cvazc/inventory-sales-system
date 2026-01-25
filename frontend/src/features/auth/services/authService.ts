import { env } from "../../../shared/config/env"
import type {
    LoginRequest,
    LoginResponse,
    MeResponse,
} from "../types/auth.types"

const jsonHeaders = {
    "Content-Type": "application/json",
} as const

function buildUrl(path: string): string {
    return `${env.apiBaseUrl}${path}`
}

async function parseJson<T>(response: Response): Promise<T> {
    const contentType = response.headers.get("content-type") ?? ""
    const isJson = contentType.includes("application/json")

    if (!response.ok) {
        const errorBody = isJson
            ? await response.json().catch(() => null)
            : await response.text().catch(() => null)

        throw new Error(
            `Request failed (${response.status}). ${typeof errorBody === "string" ? errorBody : JSON.stringify(errorBody)}`,
        )
    }

    if (!isJson) {
        throw new Error(
            "Expected JSON response but received a different content type.",
        )
    }

    return response.json() as Promise<T>
}

export async function login(payload: LoginRequest): Promise<LoginResponse> {
    const response = await fetch(buildUrl("/api/auth/login"), {
        method: "POST",
        headers: jsonHeaders,
        body: JSON.stringify(payload),
    })

    return parseJson<LoginResponse>(response)
}

export async function me(accessToken: string): Promise<MeResponse> {
    const response = await fetch(buildUrl("/api/auth/me"), {
        method: "GET",
        headers: {
            ...jsonHeaders,
            Authorization: `Bearer ${accessToken}`,
        },
    })
    return parseJson<MeResponse>(response)
}
