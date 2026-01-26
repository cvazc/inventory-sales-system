import { useEffect, useMemo, useState } from "react"
import type { CreateProductRequest } from "../services/productsService"

type Props = {
    open: boolean
    onClose: () => void
    onSubmit: (payload: CreateProductRequest) => Promise<void>
}

export function CreateProductModal({ open, onClose, onSubmit }: Props) {
    const [form, setForm] = useState<CreateProductRequest>({
        sku: "",
        name: "",
        description: "",
        stockQuantity: 0,
        price: 0,
        isActive: true,
    })

    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const canSubmit = useMemo(() => {
        if (!form.sku.trim()) return false
        if (!form.name.trim()) return false
        if (!Number.isFinite(form.stockQuantity) || form.stockQuantity < 0)
            return false
        if (!Number.isFinite(form.price) || form.price < 0) return false
        return true
    }, [form])

    useEffect(() => {
        if (!open) return
        setForm({
            sku: "",
            name: "",
            description: "",
            stockQuantity: 0,
            price: 0,
            isActive: true,
        })
        setError(null)
        setIsSubmitting(false)
    }, [open])

    if (!open) return null

    async function handleSubmit() {
        setError(null)
        setIsSubmitting(true)
        try {
            await onSubmit({
                ...form,
                sku: form.sku.trim(),
                name: form.name.trim(),
                description: (form.description ?? "").trim(),
            })
            onClose()
        } catch (e: any) {
            setError(e?.message ?? "Failed to create product.")
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 p-4">
            <div className="w-full max-w-lg rounded-xl border border-gray-800 bg-gray-900 p-6 text-white shadow">
                <div className="flex items-start justify-between gap-4">
                    <div>
                        <h2 className="text-xl font-semibold">
                            Create product
                        </h2>
                        <p className="mt-1 text-sm text-gray-300">
                            Add a new product to the catalog.
                        </p>
                    </div>

                    <button
                        type="button"
                        className="rounded-md border border-gray-800 bg-gray-950 px-3 py-1.5 text-sm hover:border-gray-600"
                        onClick={onClose}
                        disabled={isSubmitting}
                    >
                        Close
                    </button>
                </div>

                <div className="mt-6 space-y-4">
                    <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                        <div className="space-y-1">
                            <label className="text-sm text-gray-200">SKU</label>
                            <input
                                className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                                value={form.sku}
                                onChange={(e) =>
                                    setForm((p) => ({
                                        ...p,
                                        sku: e.target.value,
                                    }))
                                }
                                placeholder="PROD-0001"
                                disabled={isSubmitting}
                            />
                        </div>

                        <div className="space-y-1">
                            <label className="text-sm text-gray-200">
                                Name
                            </label>
                            <input
                                className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                                value={form.name}
                                onChange={(e) =>
                                    setForm((p) => ({
                                        ...p,
                                        name: e.target.value,
                                    }))
                                }
                                placeholder="Wireless Mouse"
                                disabled={isSubmitting}
                            />
                        </div>
                    </div>

                    <div className="space-y-1">
                        <label className="text-sm text-gray-200">
                            Description
                        </label>
                        <input
                            className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                            value={form.description ?? ""}
                            onChange={(e) =>
                                setForm((p) => ({
                                    ...p,
                                    description: e.target.value,
                                }))
                            }
                            placeholder="Optional"
                            disabled={isSubmitting}
                        />
                    </div>

                    <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                        <div className="space-y-1">
                            <label className="text-sm text-gray-200">
                                Initial stock
                            </label>
                            <input
                                type="number"
                                className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                                value={form.stockQuantity}
                                onChange={(e) =>
                                    setForm((p) => ({
                                        ...p,
                                        stockQuantity: Number(e.target.value),
                                    }))
                                }
                                disabled={isSubmitting}
                                min={0}
                            />
                        </div>

                        <div className="space-y-1">
                            <label className="text-sm text-gray-200">
                                Price
                            </label>
                            <input
                                type="number"
                                className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                                value={form.price}
                                onChange={(e) =>
                                    setForm((p) => ({
                                        ...p,
                                        price: Number(e.target.value),
                                    }))
                                }
                                disabled={isSubmitting}
                                min={0}
                            />
                        </div>
                    </div>

                    <div className="flex items-center justify-between rounded-md border border-gray-800 bg-gray-950 px-3 py-2">
                        <div>
                            <p className="text-sm font-medium">Active</p>
                            <p className="text-xs text-gray-400">
                                Inactive products can still exist in catalog.
                            </p>
                        </div>
                        <input
                            type="checkbox"
                            checked={form.isActive}
                            onChange={(e) =>
                                setForm((p) => ({
                                    ...p,
                                    isActive: e.target.checked,
                                }))
                            }
                            disabled={isSubmitting}
                        />
                    </div>

                    {error && (
                        <div className="rounded-md border border-red-900 bg-red-950 px-3 py-2 text-sm text-red-200">
                            {error}
                        </div>
                    )}

                    <div className="flex justify-end gap-2 pt-2">
                        <button
                            type="button"
                            className="rounded-md border border-gray-800 bg-gray-950 px-4 py-2 text-sm hover:border-gray-600 disabled:opacity-60"
                            onClick={onClose}
                            disabled={isSubmitting}
                        >
                            Cancel
                        </button>

                        <button
                            type="button"
                            className="rounded-md bg-white px-4 py-2 text-sm font-semibold text-black disabled:opacity-60"
                            onClick={handleSubmit}
                            disabled={!canSubmit || isSubmitting}
                        >
                            {isSubmitting ? "Creating..." : "Create product"}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )
}
