import { useEffect, useMemo, useState } from "react"
import { CreateProductModal } from "../components/CreateProductModal"
import {
    createProduct,
    getProducts,
    type CreateProductRequest,
    type ProductListItem,
} from "../services/productsService"
import { Link } from "react-router-dom"
import { useAuth } from "../../auth/useAuth"

export default function ProductsPage() {
    const { accessToken } = useAuth()
    const [products, setProducts] = useState<ProductListItem[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [toast, setToast] = useState<string | null>(null)

    const [createOpen, setCreateOpen] = useState(false)
    const [search, setSearch] = useState("")

    const filtered = useMemo(() => {
        const q = search.trim().toLowerCase()
        if (!q) return products
        return products.filter((p) => {
            return (
                p.sku.toLowerCase().includes(q) ||
                p.name.toLowerCase().includes(q)
            )
        })
    }, [products, search])

    async function load() {
        setIsLoading(true)
        setError(null)
        try {
            const data = await getProducts(accessToken ?? undefined)
            setProducts(data)
        } catch (e: any) {
            setError(e?.message ?? "Failed to load products.")
        } finally {
            setIsLoading(false)
        }
    }

    async function handleCreate(payload: CreateProductRequest) {
        if (!accessToken)
            throw new Error("Missing access token. Please sign in again.")
        await createProduct(accessToken, payload)
        setToast("Product created successfully.")
        await load()
        setTimeout(() => setToast(null), 2500)
    }

    return (
        <div className="space-y-6">
            <div className="rounded-xl border border-gray-800 bg-gray-900 p-6">
                <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
                    <div>
                        <h1 className="text-2xl font-semibold">Products</h1>
                        <p className="mt-1 text-sm text-gray-300">
                            Admin-only. Manage the product catalog and
                            inventory.
                        </p>
                    </div>

                    <div className="flex flex-col gap-2 md:flex-row md:items-center">
                        <Link
                            to="/stock-adjustments"
                            className="rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm hover:border-gray-600"
                        >
                            Stock adjustments
                        </Link>

                        <button
                            className="rounded-md bg-white px-3 py-2 text-sm font-semibold text-black"
                            onClick={() => setCreateOpen(true)}
                        >
                            Create product
                        </button>
                    </div>
                </div>

                <div className="mt-6">
                    <input
                        className="w-full rounded-md border border-gray-800 bg-gray-950 px-3 py-2 text-sm outline-none focus:border-gray-600"
                        placeholder="Search by SKU or name..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                </div>
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
                                    Price
                                </th>
                                <th className="px-4 py-3 text-left font-medium">
                                    Status
                                </th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-800">
                            {filtered.map((p) => (
                                <tr key={p.id} className="text-gray-100">
                                    <td className="px-4 py-3">{p.sku}</td>
                                    <td className="px-4 py-3">{p.name}</td>
                                    <td className="px-4 py-3">
                                        {p.stockQuantity}
                                    </td>
                                    <td className="px-4 py-3">{p.price}</td>
                                    <td className="px-4 py-3">
                                        {p.isActive ? "Active" : "Inactive"}
                                    </td>
                                </tr>
                            ))}

                            {filtered.length === 0 && (
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

            <CreateProductModal
                open={createOpen}
                onClose={() => setCreateOpen(false)}
                onSubmit={handleCreate}
            />
        </div>
    )
}
