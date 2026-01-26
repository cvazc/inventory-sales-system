import { useState } from "react"
import { useAuth } from "../useAuth"
import type { LoginRequest } from "../types/auth.types"

export function LoginPage() {
    const { login } = useAuth()

    const [form, setForm] = useState<LoginRequest>({ email: "", password: "" })
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        setError(null)
        setIsSubmitting(true)

        try {
            await login(form)
        } catch (err) {
            const message = err instanceof Error ? err.message : "Login failed"
            setError(message)
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-950 text-white p-6">
            <div className="w-full max-w-md rounded-xl border border-gray-800 bg-gray-900 p-6">
                <h1 className="text-2xl font-semibold">Sign in</h1>
                <p className="mt-1 text-sm text-gray-300">
                    Use your credentials to access the dashboard.
                </p>

                <form className="mt-6 space-y-4" onSubmit={handleSubmit}>
                    <div className="space-y-1">
                        <label
                            className="text-sm text-gray-200"
                            htmlFor="email"
                        >
                            Email
                        </label>
                        <input
                            id="email"
                            type="email"
                            autoComplete="email"
                            className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                            value={form.email}
                            onChange={(e) =>
                                setForm((prev) => ({
                                    ...prev,
                                    email: e.target.value,
                                }))
                            }
                            required
                        />
                    </div>

                    <div className="space-y-1">
                        <label
                            className="text-sm text-gray-200"
                            htmlFor="password"
                        >
                            Password
                        </label>
                        <input
                            id="password"
                            type="password"
                            autoComplete="current-password"
                            className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                            value={form.password}
                            onChange={(e) =>
                                setForm((prev) => ({
                                    ...prev,
                                    password: e.target.value,
                                }))
                            }
                            required
                        />
                    </div>

                    {error && (
                        <div className="rounded-md border border-red-900 bg-red-950 px-3 py-2 text-sm text-red-200">
                            {error}
                        </div>
                    )}

                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="w-full rounded-md bg-white px-3 py-2 text-sm font-semibold text-black disabled:opacity-60"
                    >
                        {isSubmitting ? "Signing in..." : "Sign in"}
                    </button>
                </form>
            </div>
        </div>
    )
}
