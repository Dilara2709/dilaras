
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

// Определение тихого вагона
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

// Форматирование даты создания
function formatDate(value) {
  if (!value) return '—'

  const date = new Date(value)

  if (Number.isNaN(date.getTime())) {
    return '—'
  }

  return date.toLocaleString('ru-RU', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<template>
  <div class="table-wrapper">
    <table>
      <thead>
        <tr>
          <th>Номер поездки</th>
          <th>Маршрут</th>
          <th>Вагон</th>
          <th>Тихий</th>
          <th>Проводник</th>
          <th>Статус</th>
          <th>Дата создания</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td>{{ item.number }}</td>
          <td>{{ item.title }}</td>
          <td>{{ carriageName(item.carriageId, carriages) }}</td>
          <td>
            {{ isQuietCarriage(item.carriageId, carriages) ? 'Да' : 'Нет' }}
          </td>
          <td>
            {{ conductorName(item.assigneeUserId, passengers) }}
          </td>
          <td>{{ item.status }}</td>
          <td>{{ formatDate(item.createdAt) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-wrapper {
  overflow-x: auto;
}

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
