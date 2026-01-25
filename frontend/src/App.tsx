import { BrowserRouter, Routes, Route } from "react-router-dom"
import { LoginPage } from "./features/auth/pages/LoginPage"
import { RequireAuth } from "./app/guards/RequireAuth"

function Dashboard() {
    return <div className="p-6 text-white">Dashboard</div>
}

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route
                    path="/"
                    element={
                        <RequireAuth>
                            <Dashboard />
                        </RequireAuth>
                    }
                />
            </Routes>
        </BrowserRouter>
    )
}

export default App
