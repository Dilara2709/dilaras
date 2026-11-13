<script setup>
// Объявляем props: таблица ждёт массив items
defineProps({
  items: {
    type: Array,
    required: true
  },
  carriages: {
    type: Array,
    required: true
  },
  passengers: {
    type: Array,
    required: true
  }
})

// Функция: получить название вагона по id
function carriageName(id, carriages) {
  const found = carriages.find(c => c.id === id)
  return found ? found.name : '—'
}

function isQuietCarriage(id, carriages) {
  const found = carriages.find(c => c.id === id)
  return found ? found.isQuiet : false
}

function passengerName(id, passengers) {
  const found = passengers.find(p => p.id === id)
  return found ? found.fullName : '—'
}
</script>

<template>
  <table>
    <thead>
      <tr>
        <th>Номер поезда</th>
        <th>Маршрут</th>
        <th>Вагон</th>
        <th>Тихий</th>
        <th>Исполнитель</th>
        <th>Статус</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in items" :key="item.id">
        <td>{{ item.number }}</td>
        <td>{{ item.title }}</td>
        <td>{{ carriageName(item.carriageId, carriages) }}</td>
        <td>{{ isQuietCarriage(item.carriageId, carriages) ? 'Да' : 'Нет' }}</td>
        <td>{{ item.assigneeUserId ? passengerName(item.assigneeUserId, passengers) : '—' }}</td>
        <td>{{ item.status }}</td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped>
table {
  border-collapse: collapse;
  width: 100%;
}

th,
td {
  border: 1px solid #ccc;
  padding: 6px 10px;
  text-align: left;
}

th {
  background: #f5f5f5;
}
</style>