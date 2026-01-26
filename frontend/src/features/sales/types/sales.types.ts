export interface SaleDto {
    id: string
    productId: string
    quantity: number
    createdAt: string
}

export interface PagedResult<T> {
    items: T[]
    page: number
    pageSize: number
    totalItems: number
    totalPages: number
}

export interface GetSalesQuery {
    page: number
    pageSize: number
}

export interface CreateSaleRequest {
    productId: string
    quantity: number
}

export interface CreateSaleResponse {
    id: string
}
