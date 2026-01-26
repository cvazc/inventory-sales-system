import { useEffect, useState } from "react"
import { useAuth } from "../../auth/useAuth"
import * as salesService from "../services/salesService"
import type { PagedResult, SaleDto } from "../types/sales.types"
import { CreateSaleCard } from "../components/CreateSaleCard"

export function SalesPage() {
    const { accessToken, user } = useAuth()

    const [page, setPage] = useState(1)
    const [pageSize] = useState(10)
    const [data, setData] = useState<PagedResult<SaleDto> | null>(null)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    async function load() {
        if (!accessToken) return
        setIsLoading(true)
        setError(null)

        try {
            const result = await salesService.getSales(accessToken, {
                page,
                pageSize,
            })
            setData(result)
        } catch (err) {
            const message =
                err instanceof Error ? err.message : "Failed to load sales"
            setError(message)
        } finally {
            setIsLoading(false)
        }
    }

    useEffect(() => {
        load()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page])

    return (
        <div className="min-h-screen bg-gray-950 text-white p-6">
            <div className="mx-auto max-w-5xl space-y-6">
                <div className="flex items-end justify-between">
                    <div>
                        <h1 className="text-2xl font-semibold">Sales</h1>
                        <p className="mt-1 text-sm text-gray-300">
                            Signed in as{" "}
                            <span className="font-medium text-white">
                                {user?.email}
                            </span>{" "}
                            ({user?.role})
                        </p>
                    </div>
                    <button
                        onClick={load}
                        className="rounded-md border border-gray-800 bg-gray-900 px-3 py-2 text-sm hover:border-gray-700"
                    >
                        Refresh
                    </button>
                </div>

                <CreateSaleCard onCreated={load} />

                <div className="rounded-xl border border-gray-800 bg-gray-900">
                    <div className="border-b border-gray-800 px-4 py-3">
                        <h2 className="text-sm font-semibold text-gray-100">
                            Recent sales
                        </h2>
                    </div>

                    {error && (
                        <div className="m-4 rounded-md border border-red-900 bg-red-950 px-3 py-2 text-sm text-red-200">
                            {error}
                        </div>
                    )}

                    {isLoading && (
                        <div className="p-4 text-sm text-gray-300">
                            Loading...
                        </div>
                    )}

                    {!isLoading && data && (
                        <div className="overflow-x-auto">
                            <table className="w-full text-left text-sm">
                                <thead className="text-gray-300">
                                    <tr className="border-b border-gray-800">
                                        <th className="px-4 py-3">Id</th>
                                        <th className="px-4 py-3">Product</th>
                                        <th className="px-4 py-3">Qty</th>
                                        <th className="px-4 py-3">Created</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {data.items.map((s) => (
                                        <tr
                                            key={s.id}
                                            className="border-b border-gray-800 last:border-b-0"
                                        >
                                            <td className="px-4 py-3 font-mono text-xs text-gray-200">
                                                {s.id}
                                            </td>
                                            <td className="px-4 py-3 text-gray-100">
                                                {s.productId}
                                            </td>
                                            <td className="px-4 py-3 text-gray-100">
                                                {s.quantity}
                                            </td>
                                            <td className="px-4 py-3 text-gray-300">
                                                {new Date(
                                                    s.createdAt,
                                                ).toLocaleString()}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}

                    {!isLoading && data && (
                        <div className="flex items-center justify-between border-t border-gray-800 px-4 py-3 text-sm">
                            <div className="text-gray-300">
                                Page{" "}
                                <span className="text-white">{data.page}</span>{" "}
                                of{" "}
                                <span className="text-white">
                                    {data.totalPages}
                                </span>
                            </div>
                            <div className="flex gap-2">
                                <button
                                    className="rounded-md border border-gray-800 bg-gray-950 px-3 py-1.5 disabled:opacity-50"
                                    disabled={data.page <= 1}
                                    onClick={() =>
                                        setPage((p) => Math.max(1, p - 1))
                                    }
                                >
                                    Prev
                                </button>
                                <button
                                    className="rounded-md border border-gray-800 bg-gray-950 px-3 py-1.5 disabled:opacity-50"
                                    disabled={data.page >= data.totalPages}
                                    onClick={() => setPage((p) => p + 1)}
                                >
                                    Next
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    )
}
