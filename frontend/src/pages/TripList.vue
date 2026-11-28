
<script setup>
import { ref, onMounted } from 'vue'

import TripTable from '../components/TripTable.vue'
import { items, carriages, passengers, loadTrips } from '../tripsStore.js'

// 'loading' | 'empty' | 'data' | 'error'
const screen = ref('loading')
const errorMessage = ref('')

// Фильтры
const selectedStatus = ref('')
const selectedCarriage = ref('')

async function fetchTrips() {
  screen.value = 'loading'
  errorMessage.value = ''

  try {
    const data = await loadTrips({
      status: selectedStatus.value,
      carriageId: selectedCarriage.value
    })

    if (data.length === 0) {
      screen.value = 'empty'
    } else {
      screen.value = 'data'
    }
  } catch (err) {
    errorMessage.value =
      err.message || 'Не удалось загрузить список'

    screen.value = 'error'
  }
}

function resetFilters() {
  selectedStatus.value = ''
  selectedCarriage.value = ''
  fetchTrips()
}

onMounted(fetchTrips)
</script>

<template>
  <main>
    <h2>Поездки дальнего следования</h2>

    <p class="hint">
      Сервис «РЖД Соседи» — знакомьтесь с попутчиками до посадки.
    </p>

    <!-- Фильтры -->
    <div class="filters">
      <label>
        Статус:
        <select
          v-model="selectedStatus"
          :disabled="screen === 'loading'"
          @change="fetchTrips"
        >
          <option value="">Все статусы</option>
          <option value="New">Новые</option>
          <option value="InProgress">В процессе</option>
          <option value="Closed">Завершённые</option>
          <option value="Cancelled">Отменённые</option>
        </select>
      </label>

      <label>
        Вагон:
        <select
          v-model="selectedCarriage"
          :disabled="screen === 'loading'"
          @change="fetchTrips"
        >
          <option value="">Все вагоны</option>

          <option
            v-for="carriage in carriages"
            :key="carriage.id"
            :value="carriage.id"
          >
            {{ carriage.name }}
          </option>
        </select>
      </label>

      <button
        type="button"
        :disabled="screen === 'loading'"
        @click="resetFilters"
      >
        Сбросить фильтры
      </button>
    </div>

    <!-- Состояние 1: загрузка -->
    <div v-if="screen === 'loading'">
      Загрузка…
    </div>

    <!-- Состояние 2: пусто -->
    <div v-else-if="screen === 'empty'">
      Поездки не найдены
    </div>

    <!-- Состояние 3: ошибка -->
    <div v-else-if="screen === 'error'" class="error">
      {{ errorMessage }}

      <button type="button" @click="fetchTrips">
        Повторить
      </button>
    </div>

    <!-- Состояние 4: данные -->
    <TripTable
      v-else
      :items="items"
      :carriages="carriages"
      :passengers="passengers"
    />
  </main>
</template>

<style scoped>
.hint {
  color: #555;
  font-size: 14px;
  margin: 4px 0 16px;
}

.filters {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 12px;
  margin-bottom: 20px;
  padding: 12px;
  background: #f5f5f5;
  border: 1px solid #ddd;
  border-radius: 6px;
}

.filters select,
.filters button {
  padding: 7px 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  background: white;
}

.filters button {
  cursor: pointer;
}

.filters button:disabled {
  cursor: default;
  opacity: 0.6;
}

.error {
  color: #c00;
}

.error button {
  margin-left: 8px;
}
</style>
