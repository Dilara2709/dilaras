// Имитация задержки сети
const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms))

// Учебная «база» — массив поездок
let mockItems = [
  {
    id: 1, number: 'П-101', title: 'Москва — Казань',
    description: 'Ночной поезд, 12 часов в пути. Есть тихий вагон.',
    status: 'New', carriageId: 4, createdByUserId: 1, assigneeUserId: null
  },
  {
    id: 2, number: 'П-102', title: 'Санкт-Петербург — Москва',
    description: 'Дневной экспресс, 4 часа. Ищем попутчиков для настольных игр.',
    status: 'InProgress', carriageId: 2, createdByUserId: 1, assigneeUserId: 2
  }
]

// Счётчик для новых id (учебный, «серверный»)
let mockNextId = 3

// Флаг для проверки 500 (включим вручную для теста)
let force500 = false

export function setForce500(value) {
  force500 = value
}

// GET — список поездок
export async function mockGetTrips() {
  await delay(500)
  if (force500) {
    return { ok: false, status: 500, data: { message: 'Сервер не смог обработать запрос' } }
  }
  return { ok: true, status: 200, data: [...mockItems] }
}

// POST — создание поездки
export async function mockCreateTrip(body) {
  await delay(500)
  if (force500) {
    return { ok: false, status: 500, data: { message: 'Сервер не смог обработать запрос' } }
  }
  // Проверка как на сервере
  const title = (body.title || '').trim()
  if (title.length < 5 || title.length > 80) {
    return { ok: false, status: 400, data: { message: 'Тема: от 5 до 80 символов' } }
  }
  if (!body.carriageId || body.carriageId <= 0) {
    return { ok: false, status: 400, data: { message: 'Выберите вагон' } }
  }

  const newItem = {
    id: mockNextId,
    number: `П-${100 + mockNextId}`,   // «серверный» номер
    title,
    description: (body.description || '').trim(),
    status: 'New',
    carriageId: body.carriageId,
    createdByUserId: 1,
    assigneeUserId: null
  }
  mockItems.push(newItem)
  mockNextId++
  return { ok: true, status: 201, data: newItem }
}