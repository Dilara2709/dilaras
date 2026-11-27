
const baseUrl = (
  import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'
).replace(/\/+$/, '')

async function getDictionary(path) {

  const response = await fetch(`${baseUrl}${path}`)

  if (!response.ok) {
    throw {
      status: response.status,
      message: 'Не удалось загрузить справочник'
    }
  }

  return await response.json()
}

// Получение справочника вагонов
export async function getCarriages() {
  return await getDictionary('/api/carriages')
}

// Получение справочника пользователей
export async function getUsers() {
  return await getDictionary('/api/users')
}
