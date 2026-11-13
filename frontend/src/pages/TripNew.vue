<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { carriages, addTrip } from '../tripsStore.js'

const router = useRouter()

const form = ref({
  title: '',
  carriageId: 0,
  description: ''
})

const errors = ref({ title: '', carriageId: '', description: '' })
const submitError = ref('')
const isSubmitting = ref(false)

async function onCreate() {
  if (isSubmitting.value) return
  isSubmitting.value = true
  submitError.value = ''
  errors.value = { title: '', carriageId: '', description: '' }

  const title = form.value.title.trim()
  const description = form.value.description.trim()

  // Клиентская проверка — «вежливость»
  let hasError = false
  if (title.length < 5 || title.length > 80) {
    errors.value.title = 'Тема: от 5 до 80 символов'
    hasError = true
  }
  if (!Number.isInteger(form.value.carriageId) || form.value.carriageId <= 0) {
    errors.value.carriageId = 'Выберите вагон'
    hasError = true
  }
  if (description.length < 10 || description.length > 500) {
    errors.value.description = 'Описание: от 10 до 500 символов'
    hasError = true
  }
  if (hasError) {
    isSubmitting.value = false
    return
  }

  try {
    await addTrip({
      title,
      carriageId: form.value.carriageId,
      description
    })
    form.value = { title: '', carriageId: 0, description: '' }
    router.push('/')
  } catch (err) {
    // Различаем отказы
    if (err.status === 400) {
      submitError.value = err.message || 'Проверьте поля'
    } else if (err.status === 500) {
      submitError.value = 'Сервер не смог обработать запрос'
    } else {
      submitError.value = 'Нет связи. Проверьте, что приложение запущено'
    }
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main>
    <h2>Новая поездка</h2>

    <form @submit.prevent="onCreate">
      <label for="title">Маршрут (5–80 символов)</label>
      <input id="title" v-model="form.title" autofocus />
      <p v-if="errors.title" class="error">{{ errors.title }}</p>

      <label for="carriageId">Тип вагона</label>
      <select id="carriageId" v-model.number="form.carriageId">
        <option :value="0">— выберите —</option>
        <option v-for="c in carriages" :key="c.id" :value="c.id">
          {{ c.name }}{{ c.isQuiet ? ' (тихий)' : '' }}
        </option>
      </select>
      <p v-if="errors.carriageId" class="error">{{ errors.carriageId }}</p>

      <label for="description">Описание (10–500 символов)</label>
      <textarea id="description" v-model="form.description"></textarea>
      <p v-if="errors.description" class="error">{{ errors.description }}</p>

      <p v-if="submitError" class="error">{{ submitError }}</p>

      <button type="submit" :disabled="isSubmitting">
        {{ isSubmitting ? 'Создаём…' : 'Создать' }}
      </button>
    </form>
  </main>
</template>

<style scoped>
label { display: block; margin-top: 12px; margin-bottom: 4px; font-weight: 500; }
input, select, textarea { display: block; width: 100%; max-width: 400px; padding: 6px; box-sizing: border-box; }
textarea { min-height: 80px; resize: vertical; }
.error { color: #c00; font-size: 13px; margin: 4px 0 0; }
form button[type='submit'] { margin-top: 12px; }
form button[disabled] { opacity: 0.6; cursor: not-allowed; }
</style>