import { useEffect, useState } from "react"
import {
    getProducts,
    type ProductListItem,
} from "../../products/services/productsService"
import { adjustStock } from "../services/stockAdjustmentsService"
import { AdjustStockModal } from "../components/AdjustStockModal"
import type { AdjustStockRequest } from "../types"
import { useAuth } from "../../auth/useAuth"

export default function StockAdjustmentsPage() {
    const { accessToken } = useAuth()

    const [products, setProducts] = useState<ProductListItem[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    const [selected, setSelected] = useState<ProductListItem | null>(null)
    const [toast, setToast] = useState<string | null>(null)

    async function load() {
        setIsLoading(true)
        setError(null)
        try {
            const data = await getProducts()
            setProducts(data)
        } catch (e: any) {
            setError(e?.message ?? "Failed to load products.")
        } finally {
            setIsLoading(false)
        }
    }

    useEffect(() => {
        void load()
    }, [])

    async function handleSubmit(payload: AdjustStockRequest) {
        if (!selected) return
        if (!accessToken)
            throw new Error("Missing access token. Please sign in again.")

        await adjustStock(accessToken, selected.id, payload)
        setToast("Stock updated successfully.")
        await load()
        setTimeout(() => setToast(null), 2500)
    }

    return (
        <div className="min-h-screen bg-gray-950 text-white p-6">
            <div className="mx-auto w-full max-w-5xl space-y-6">
                <div className="rounded-xl border border-gray-800 bg-gray-900 p-6">
                    <h1 className="text-2xl font-semibold">
                        Stock adjustments
                    </h1>
                    <p className="mt-1 text-sm text-gray-300">
                        Admin-only. Adjust inventory with a reason for
                        auditability.
                    </p>
                </div>

                {toast && (
                    <div className="rounded-md border border-gray-800 bg-gray-900 px-4 py-3 text-sm text-gray-100">
                        {toast}
                    </div>
                )}

                {isLoading && (
                    <div className="rounded-xl border border-gray-800 bg-gray-900 p-6 text-gray-200">
                        Loading products...
                    </div>
                )}

                {error && (
                    <div className="rounded-md border border-red-900 bg-red-950 px-4 py-3 text-sm text-red-200">
                        {error}
                    </div>
                )}

                {!isLoading && !error && (
                    <div className="overflow-hidden rounded-xl border border-gray-800 bg-gray-900">
                        <table className="w-full text-sm">
                            <thead className="bg-gray-950">
                                <tr className="text-gray-200">
                                    <th className="px-4 py-3 text-left font-medium">
                                        SKU
                                    </th>
                                    <th className="px-4 py-3 text-left font-medium">
                                        Name
                                    </th>
                                    <th className="px-4 py-3 text-left font-medium">
                                        Stock
                                    </th>
                                    <th className="px-4 py-3 text-left font-medium">
                                        Status
                                    </th>
                                    <th className="px-4 py-3 text-right font-medium">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-gray-800">
                                {products.map((p) => (
                                    <tr key={p.id} className="text-gray-100">
                                        <td className="px-4 py-3">{p.sku}</td>
                                        <td className="px-4 py-3">{p.name}</td>
                                        <td className="px-4 py-3">
                                            {p.stockQuantity}
                                        </td>
                                        <td className="px-4 py-3">
                                            {p.isActive ? "Active" : "Inactive"}
                                        </td>
                                        <td className="px-4 py-3 text-right">
                                            <button
                                                className="rounded-md border border-gray-800 bg-gray-950 px-3 py-1.5 text-sm hover:border-gray-600"
                                                onClick={() => setSelected(p)}
                                            >
                                                Adjust
                                            </button>
                                        </td>
                                    </tr>
                                ))}

                                {products.length === 0 && (
                                    <tr>
                                        <td
                                            className="px-4 py-10 text-center text-gray-400"
                                            colSpan={5}
                                        >
                                            No products found.
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                )}

                <AdjustStockModal
                    open={selected !== null}
                    productName={selected?.name ?? ""}
                    currentStock={selected?.stockQuantity ?? 0}
                    onClose={() => setSelected(null)}
                    onSubmit={handleSubmit}
                />
            </div>
        </div>
    )
}
