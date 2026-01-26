import { Link } from 'react-router-dom'
import { useAuth } from '../../features/auth/useAuth'

export function DashboardPage() {
  const { user, logout } = useAuth()

  return (
    <div className="min-h-screen bg-gray-950 text-white p-6">
      <div className="mx-auto max-w-3xl space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-semibold">Dashboard</h1>
            <p className="mt-1 text-sm text-gray-300">
              {user?.email} ({user?.role})
            </p>
          </div>
          <button
            onClick={logout}
            className="rounded-md border border-gray-800 bg-gray-900 px-3 py-2 text-sm hover:border-gray-700"
          >
            Logout
          </button>
        </div>

        <div className="rounded-xl border border-gray-800 bg-gray-900 p-4">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm font-semibold">Sales</div>
              <div className="text-xs text-gray-300">View and create sales</div>
            </div>
            <Link
              to="/sales"
              className="rounded-md bg-white px-3 py-2 text-sm font-semibold text-black"
            >
              Open
            </Link>
          </div>
        </div>
      </div>
    </div>
  )
}
