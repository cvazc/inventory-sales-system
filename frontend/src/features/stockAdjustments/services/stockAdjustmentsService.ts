import { request } from "../../../shared/lib/apiClient"
import type { AdjustStockRequest } from "../types"

export async function adjustStock(
    accessToken: string,
    productId: number,
    payload: AdjustStockRequest,
): Promise<void> {
    await request<unknown>(`/api/products/${productId}/stock`, {
        method: "PUT",
        accessToken,
        body: payload,
    })
}
