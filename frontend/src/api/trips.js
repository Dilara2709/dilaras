
import { mockGetTrips, mockCreateTrip } from './mockTrips.js'

// Используем реальный backend
const USE_MOCK = false

const baseUrl = (
  import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'
).replace(/\/+$/, '')

// Идентификатор пользователя для учебного режима
const userId = import.meta.env.VITE_USER_ID || '1'

// Обработка ошибок API
async function handleError(response, fallbackMessage) {
  const errData = await response.json().catch(() => ({}))

  const validationErrors = errData.errors
    ? Object.values(errData.errors).flat().join(' ')
    : ''

  throw {
    status: response.status,
    message:
      errData.detail ||
      validationErrors ||
      errData.message ||
      errData.title ||
      fallbackMessage
  }
}

// GET /api/trips — получить список
export async function getTrips() {
  if (USE_MOCK) {
    const mock = await mockGetTrips()

    if (!mock.ok) {
      throw {
        status: mock.status,
        message: mock.data.message || 'Ошибка сервера'
      }
    }

    return mock.data
  }

  const response = await fetch(`${baseUrl}/api/trips`)

  if (!response.ok) {
    await handleError(response, 'Не удалось загрузить список')
  }

  return await response.json()
}

// POST /api/trips — создать поездку
export async function createTrip(body) {
  if (USE_MOCK) {
    const mock = await mockCreateTrip(body)

    if (!mock.ok) {
      throw {
        status: mock.status,
        message: mock.data.message || 'Ошибка сервера'
      }
    }

    return mock.data
  }

  const response = await fetch(`${baseUrl}/api/trips`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-User-Id': userId
    },
    body: JSON.stringify(body)
  })

  if (!response.ok) {
    await handleError(response, 'Не удалось создать поездку')
  }

  return await response.json()
}
