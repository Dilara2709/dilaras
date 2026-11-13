import { ref } from 'vue'
import { getTrips as apiGetTrips, createTrip as apiCreateTrip } from './api/trips.js'

// Справочники (пока зашиты)
export const carriages = ref([
  { id: 1, name: 'Плацкарт', type: 'economy', isQuiet: false },
  { id: 2, name: 'Купе', type: 'comfort', isQuiet: false },
  { id: 3, name: 'СВ', type: 'business', isQuiet: false },
  { id: 4, name: 'Тихий вагон', type: 'quiet', isQuiet: true }
])

export const passengers = ref([
  { id: 1, login: 'ivan_p', fullName: 'Иванов Иван', role: 'passenger', interests: 'Настольные игры, музыка', quietPref: false, rating: 4.8 },
  { id: 2, login: 'petrova_a', fullName: 'Петрова Анна', role: 'passenger', interests: 'Экскурсии, книги', quietPref: true, rating: 5.0 }
])

// Массив поездок — приходит с сервера (сейчас из mock)
export const items = ref([])

// Загрузить список с сервера
export async function loadTrips() {
  const data = await apiGetTrips()
  items.value = data
  return data
}

// Создать поездку на сервере
export async function addTrip(body) {
  const created = await apiCreateTrip(body)
  items.value.push(created)
  return created
}