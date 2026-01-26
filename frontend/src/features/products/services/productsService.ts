import { request } from "../../../shared/lib/apiClient"

export type ProductListItem = {
    id: number
    sku: string
    name: string
    description?: string | null
    stockQuantity: number
    price: number
    isActive: boolean
}

export async function getProducts(): Promise<ProductListItem[]> {
    return request<ProductListItem[]>("/api/products", { method: "GET" })
}
