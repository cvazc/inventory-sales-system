import { BrowserRouter, Routes, Route } from "react-router-dom"
import { LoginPage } from "./features/auth/pages/LoginPage"
import { RequireAuth } from "./app/guards/RequireAuth"
import { RequireRole } from "./app/guards/RequireRole"
import { DashboardPage } from "./app/pages/DashboardPage"
import { SalesPage } from "./features/sales/pages/SalesPage"
import StockAdjustmentsPage from "./features/stockAdjustments/pages/StockAdjustmentsPage"
import ProductsPage from "./features/products/pages/ProductsPage"
import { AppLayout } from "./app/layout/AppLayout"

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LoginPage />} />

                <Route
                    path="/"
                    element={
                        <RequireAuth>
                            <AppLayout>
                                <DashboardPage />
                            </AppLayout>
                        </RequireAuth>
                    }
                />

                <Route
                    path="/sales"
                    element={
                        <RequireAuth>
                            <AppLayout>
                                <SalesPage />
                            </AppLayout>
                        </RequireAuth>
                    }
                />

                <Route
                    path="/stock-adjustments"
                    element={
                        <RequireAuth>
                            <RequireRole role="Admin">
                                <AppLayout>
                                    <StockAdjustmentsPage />
                                </AppLayout>
                            </RequireRole>
                        </RequireAuth>
                    }
                />

                <Route
                    path="/products"
                    element={
                        <RequireAuth>
                            <RequireRole role="Admin">
                                <AppLayout>
                                    <ProductsPage />
                                </AppLayout>
                            </RequireRole>
                        </RequireAuth>
                    }
                />
            </Routes>
        </BrowserRouter>
    )
}

export default App
