import { useEffect, useMemo, useState } from "react"
import type { AdjustStockRequest } from "../types"

type Props = {
    open: boolean
    productName: string
    currentStock: number
    onClose: () => void
    onSubmit: (payload: AdjustStockRequest) => Promise<void>
}

export function AdjustStockModal({
    open,
    productName,
    currentStock,
    onClose,
    onSubmit,
}: Props) {
    const [delta, setDelta] = useState<number>(0)
    const [reason, setReason] = useState<string>("")
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const nextStock = currentStock + delta

    const canSubmit = useMemo(() => {
        if (!reason.trim()) return false
        if (!Number.isFinite(delta) || delta === 0) return false
        if (nextStock < 0) return false
        return true
    }, [reason, delta, nextStock])

    useEffect(() => {
        if (!open) return
        setDelta(0)
        setReason("")
        setError(null)
        setIsSubmitting(false)
    }, [open])

    if (!open) return null

    async function handleSubmit() {
        setError(null)
        setIsSubmitting(true)
        try {
            await onSubmit({ delta, reason: reason.trim() })
            onClose()
        } catch (e: any) {
            setError(e?.message ?? "Failed to adjust stock.")
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 p-4">
            <div className="w-full max-w-lg rounded-xl border border-gray-800 bg-gray-900 p-6 text-white shadow">
                <div className="flex items-start justify-between gap-4">
                    <div>
                        <h2 className="text-xl font-semibold">Adjust stock</h2>
                        <p className="mt-1 text-sm text-gray-300">
                            {productName} • Current stock:{" "}
                            <span className="font-medium text-white">
                                {currentStock}
                            </span>
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
                    <div className="space-y-1">
                        <label className="text-sm text-gray-200">Delta</label>
                        <input
                            type="number"
                            className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                            value={delta}
                            onChange={(e) => setDelta(Number(e.target.value))}
                            disabled={isSubmitting}
                            placeholder="e.g. 10 or -2"
                        />
                        <p className="text-xs text-gray-400">
                            New stock:{" "}
                            <span className="font-medium text-gray-100">
                                {nextStock}
                            </span>
                        </p>
                        {nextStock < 0 && (
                            <p className="text-xs text-red-200">
                                Stock cannot go below zero.
                            </p>
                        )}
                    </div>

                    <div className="space-y-1">
                        <label className="text-sm text-gray-200">Reason</label>
                        <input
                            type="text"
                            className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                            value={reason}
                            onChange={(e) => setReason(e.target.value)}
                            disabled={isSubmitting}
                            placeholder="e.g. Manual restock"
                            maxLength={120}
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
                            {isSubmitting ? "Saving..." : "Save adjustment"}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )
}
