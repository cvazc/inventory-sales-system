import { env } from "../config/env"

type HttpMethod = "GET" | "POST" | "PUT" | "PATCH" | "DELETE"

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

export async function request<T>(
    path: string,
    options: {
        method: HttpMethod
        accessToken?: string | null
        body?: unknown
    },
): Promise<T> {
    const headers: Record<string, string> = {
        "Content-Type": "application/json",
    }

    if (options.accessToken) {
        headers.Authorization = `Bearer ${options.accessToken}`
    }

    const response = await fetch(`${env.apiBaseUrl}${path}`, {
        method: options.method,
        headers,
        body: options.body ? JSON.stringify(options.body) : undefined,
    })

    return parseJson<T>(response)
}
