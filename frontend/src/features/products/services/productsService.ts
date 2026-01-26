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

export type CreateProductRequest = {
    sku: string
    name: string
    description?: string
    stockQuantity: number
    price: number
    isActive: boolean
}

export async function getProducts(
    accessToken?: string,
): Promise<ProductListItem[]> {
    return request<ProductListItem[]>("/api/products", {
        method: "GET",
        accessToken,
    })
}

export async function createProduct(
    accessToken: string,
    payload: CreateProductRequest,
): Promise<ProductListItem> {
    return request<ProductListItem>("/api/products", {
        method: "POST",
        accessToken,
        body: payload,
    })
}
