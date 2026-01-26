import { BrowserRouter, Routes, Route } from "react-router-dom"
import { LoginPage } from "./features/auth/pages/LoginPage"
import { RequireAuth } from "./app/guards/RequireAuth"
import { DashboardPage } from "./app/pages/DashboardPage"
import { SalesPage } from "./features/sales/pages/SalesPage"

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LoginPage />} />

                <Route
                    path="/"
                    element={
                        <RequireAuth>
                            <DashboardPage />
                        </RequireAuth>
                    }
                />

                <Route
                    path="/sales"
                    element={
                        <RequireAuth>
                            <SalesPage />
                        </RequireAuth>
                    }
                />
            </Routes>
        </BrowserRouter>
    )
}

export default App
