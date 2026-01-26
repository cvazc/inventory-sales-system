import { request } from "../../../shared/lib/apiClient"
import type {
    CreateSaleRequest,
    CreateSaleResponse,
    GetSalesQuery,
    PagedResult,
    SaleDto,
} from "../types/sales.types"

export async function getSales(
    accessToken: string,
    query: GetSalesQuery,
): Promise<PagedResult<SaleDto>> {
    const qs = new URLSearchParams({
        page: String(query.page),
        pageSize: String(query.pageSize),
    })

    return request<PagedResult<SaleDto>>(`/api/sales?${qs.toString()}`, {
        method: "GET",
        accessToken,
    })
}

export async function createSale(
    accessToken: string,
    payload: CreateSaleRequest,
): Promise<CreateSaleResponse> {
    return request<CreateSaleResponse>("/api/sales", {
        method: "POST",
        accessToken,
        body: payload,
    })
}
