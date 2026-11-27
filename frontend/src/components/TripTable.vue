
<script setup>
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

// Получение названия вагона
function carriageName(id, carriages) {
  const found = carriages.find(c => c.id === id)
  return found ? found.name : '—'
}

// Определение тихого вагона по его типу
function isQuietCarriage(id, carriages) {
  const found = carriages.find(c => c.id === id)
  return found?.type === 'quiet'
}

// Получение имени назначенного проводника
function conductorName(id, users) {
  if (id == null) return '—'

  const found = users.find(u => u.id === id)

  if (!found) return `Пользователь #${id}`

  if (found.role !== 'conductor') {
    return 'Некорректное назначение'
  }

  return found.fullName
}
</script>

<template>
  <table>
    <thead>
      <tr>
        <th>Номер поездки</th>
        <th>Маршрут</th>
        <th>Вагон</th>
        <th>Тихий</th>
        <th>Проводник</th>
        <th>Статус</th>
      </tr>
    </thead>

    <tbody>
      <tr v-for="item in items" :key="item.id">
        <td>{{ item.number }}</td>
        <td>{{ item.title }}</td>
        <td>{{ carriageName(item.carriageId, carriages) }}</td>
        <td>{{ isQuietCarriage(item.carriageId, carriages) ? 'Да' : 'Нет' }}</td>
        <td>{{ conductorName(item.assigneeUserId, passengers) }}</td>
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
