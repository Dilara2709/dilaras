
import { ref } from 'vue'

import {
  getTrips as apiGetTrips,
  createTrip as apiCreateTrip
} from './api/trips.js'

import {
  getCarriages,
  getUsers
} from './api/dictionaries.js'

// Справочники из PostgreSQL
export const carriages = ref([])
export const passengers = ref([])

// Список поездок из PostgreSQL
export const items = ref([])

// Загрузка справочников
export async function loadDictionaries() {
  const [carriagesData, usersData] = await Promise.all([
    getCarriages(),
    getUsers()
  ])

  carriages.value = carriagesData
  passengers.value = usersData
}

// Загрузка поездок с учётом фильтров
export async function loadTrips(filters = {}) {
  const [data] = await Promise.all([
    apiGetTrips(filters),
    loadDictionaries()
  ])

  items.value = data

  return data
}

// Создание поездки на сервере
export async function addTrip(body) {
  const created = await apiCreateTrip(body)

  items.value.unshift(created)

  return created
}
