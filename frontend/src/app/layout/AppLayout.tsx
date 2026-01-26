import type { ReactElement } from "react"
import { Link, useLocation } from "react-router-dom"
import { useAuth } from "../../features/auth/useAuth"

function NavItem({ to, label }: { to: string; label: string }) {
    const location = useLocation()
    const isActive = location.pathname === to

    return (
        <Link
            to={to}
            className={[
                "rounded-md border px-3 py-2 text-sm",
                isActive
                    ? "border-gray-600 bg-gray-900 text-white"
                    : "border-gray-800 bg-gray-950 text-gray-200 hover:border-gray-600",
            ].join(" ")}
        >
            {label}
        </Link>
    )
}

export function AppLayout({ children }: { children: ReactElement }) {
    const { user, logout } = useAuth()

    return (
        <div className="min-h-screen bg-gray-950 text-white">
            <header className="border-b border-gray-800 bg-gray-950">
                <div className="mx-auto flex w-full max-w-6xl items-center justify-between gap-4 px-6 py-4">
                    <div className="flex items-center gap-3">
                        <Link to="/" className="text-sm font-semibold">
                            Inventory & Sales
                        </Link>

                        <div className="hidden items-center gap-2 md:flex">
                            <NavItem to="/" label="Dashboard" />
                            <NavItem to="/sales" label="Sales" />

                            {user?.role === "Admin" && (
                                <>
                                    <NavItem to="/products" label="Products" />
                                    <NavItem
                                        to="/stock-adjustments"
                                        label="Stock"
                                    />
                                </>
                            )}
                        </div>
                    </div>

                    <div className="flex items-center gap-3">
                        {user && (
                            <div className="hidden text-xs text-gray-300 md:block">
                                {user.email} • {user.role}
                            </div>
                        )}

                        <button
                            onClick={logout}
                            className="rounded-md bg-white px-3 py-2 text-sm font-semibold text-black"
                        >
                            Logout
                        </button>
                    </div>
                </div>

                {/* Mobile nav */}
                <div className="mx-auto w-full max-w-6xl px-6 pb-4 md:hidden">
                    <div className="flex flex-wrap gap-2">
                        <NavItem to="/" label="Dashboard" />
                        <NavItem to="/sales" label="Sales" />
                        {user?.role === "Admin" && (
                            <>
                                <NavItem to="/products" label="Products" />
                                <NavItem
                                    to="/stock-adjustments"
                                    label="Stock"
                                />
                            </>
                        )}
                    </div>
                </div>
            </header>

            <main className="mx-auto w-full max-w-6xl p-6">{children}</main>
        </div>
    )
}
