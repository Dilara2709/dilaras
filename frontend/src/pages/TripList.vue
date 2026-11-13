<script setup>
import { ref, onMounted } from 'vue'
import TripTable from '/Users/acer/Desktop/dilaras/frontend/src/components/TrIpTable.vue'
import { items, carriages, passengers, loadTrips } from '../tripsStore.js'

// 'loading' | 'empty' | 'data' | 'error'
const screen = ref('loading')
const errorMessage = ref('')

async function fetchTrips() {
  screen.value = 'loading'
  errorMessage.value = ''
  try {
    const data = await loadTrips()
    if (data.length === 0) {
      screen.value = 'empty'
    } else {
      screen.value = 'data'
    }
  } catch (err) {
    errorMessage.value = err.message || 'Не удалось загрузить список'
    screen.value = 'error'
  }
}

onMounted(fetchTrips)
</script>

<template>
  <main>
    <h2>Поездки дальнего следования</h2>
    <p class="hint">Сервис «РЖД Соседи» — знакомьтесь с попутчиками до посадки.</p>

    <!-- Состояние 1: загрузка -->
    <div v-if="screen === 'loading'">Загрузка…</div>

    <!-- Состояние 2: пусто -->
    <div v-else-if="screen === 'empty'">Записей пока нет</div>

    <!-- Состояние 3: ошибка -->
    <div v-else-if="screen === 'error'" class="error">
      {{ errorMessage }}
      <button type="button" @click="fetchTrips">Повторить</button>
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
.hint { color: #555; font-size: 14px; margin: 4px 0 16px; }
.error { color: #c00; }
.error button { margin-left: 8px; }
</style>