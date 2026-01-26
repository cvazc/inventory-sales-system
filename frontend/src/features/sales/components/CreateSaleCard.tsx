import { useState } from "react"
import { useAuth } from "../../auth/useAuth"
import * as salesService from "../services/salesService"
import type { CreateSaleRequest } from "../types/sales.types"

export function CreateSaleCard({ onCreated }: { onCreated: () => void }) {
    const { accessToken, user } = useAuth()

    const canCreate = user?.role === "Admin" || user?.role === "Clerk"

    const [form, setForm] = useState<CreateSaleRequest>({
        productId: "",
        quantity: 1,
    })
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState<string | null>(null)

    async function submit() {
        if (!accessToken) return
        setIsSubmitting(true)
        setError(null)
        setSuccess(null)

        try {
            await salesService.createSale(accessToken, form)
            setSuccess("Sale created")
            setForm({ productId: "", quantity: 1 })
            onCreated()
        } catch (err) {
            const message =
                err instanceof Error ? err.message : "Failed to create sale"
            setError(message)
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="rounded-xl border border-gray-800 bg-gray-900 p-4">
            <div className="flex items-center justify-between">
                <h2 className="text-sm font-semibold">Create sale</h2>
                {!canCreate && (
                    <span className="text-xs text-gray-400">Not allowed</span>
                )}
            </div>

            <div className="mt-4 grid grid-cols-1 gap-3 sm:grid-cols-3">
                <input
                    className="rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600 sm:col-span-2"
                    placeholder="Product ID"
                    value={form.productId}
                    onChange={(e) =>
                        setForm((p) => ({ ...p, productId: e.target.value }))
                    }
                    disabled={!canCreate || isSubmitting}
                />
                <input
                    className="rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                    type="number"
                    min={1}
                    value={form.quantity}
                    onChange={(e) =>
                        setForm((p) => ({
                            ...p,
                            quantity: Number(e.target.value),
                        }))
                    }
                    disabled={!canCreate || isSubmitting}
                />
            </div>

            {error && (
                <div className="mt-3 rounded-md border border-red-900 bg-red-950 px-3 py-2 text-sm text-red-200">
                    {error}
                </div>
            )}
            {success && (
                <div className="mt-3 rounded-md border border-green-900 bg-green-950 px-3 py-2 text-sm text-green-200">
                    {success}
                </div>
            )}

            <button
                onClick={submit}
                disabled={!canCreate || isSubmitting || !form.productId}
                className="mt-4 rounded-md bg-white px-3 py-2 text-sm font-semibold text-black disabled:opacity-60"
            >
                {isSubmitting ? "Creating..." : "Create sale"}
            </button>
        </div>
    )
}
