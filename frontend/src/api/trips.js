import { mockGetTrips, mockCreateTrip } from './mockTrips.js'

// Пока сервера нет — используем mock
const USE_MOCK = true

const baseUrl = import.meta.env.VITE_API_BASE_URL

// GET /api/trips — получить список
export async function getTrips() {
  if (USE_MOCK) {
    const mock = await mockGetTrips()
    if (!mock.ok) {
      throw { status: mock.status, message: mock.data.message || 'Ошибка сервера' }
    }
    return mock.data
  }

  // Настоящий fetch
  const response = await fetch(`${baseUrl}/api/trips`)
  if (!response.ok) {
    throw { status: response.status, message: 'Не удалось загрузить список' }
  }
  return await response.json()
}

// POST /api/trips — создать поездку
export async function createTrip(body) {
  if (USE_MOCK) {
    const mock = await mockCreateTrip(body)
    if (!mock.ok) {
      throw { status: mock.status, message: mock.data.message || 'Ошибка сервера' }
    }
    return mock.data
  }

  const response = await fetch(`${baseUrl}/api/trips`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  })
  if (!response.ok) {
    const errData = await response.json().catch(() => ({}))
    throw { status: response.status, message: errData.message || 'Не удалось создать' }
  }
  return await response.json()
}