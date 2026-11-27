
<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import {
  carriages,
  addTrip,
  loadDictionaries
} from '../tripsStore.js'

const router = useRouter()

const form = ref({
  title: '',
  carriageId: 0,
  description: ''
})

const errors = ref({
  title: '',
  carriageId: '',
  description: ''
})

const submitError = ref('')
const dictionaryError = ref('')
const isSubmitting = ref(false)
const isLoading = ref(true)

// Загрузка справочников при открытии страницы
async function loadData() {
  isLoading.value = true
  dictionaryError.value = ''

  try {
    await loadDictionaries()
  } catch (err) {
    dictionaryError.value =
      'Не удалось загрузить справочник вагонов'
  } finally {
    isLoading.value = false
  }
}

onMounted(loadData)

// Создание поездки
async function onCreate() {
  if (
    isSubmitting.value ||
    isLoading.value ||
    dictionaryError.value
  ) return

  isSubmitting.value = true
  submitError.value = ''

  errors.value = {
    title: '',
    carriageId: '',
    description: ''
  }

  const title = form.value.title.trim()
  const description = form.value.description.trim()

  let hasError = false

  if (title.length < 5 || title.length > 80) {
    errors.value.title = 'Маршрут: от 5 до 80 символов'
    hasError = true
  }

  if (
    !Number.isInteger(form.value.carriageId) ||
    !carriages.value.some(c => c.id === form.value.carriageId)
  ) {
    errors.value.carriageId = 'Выберите вагон'
    hasError = true
  }

  if (description.length < 10 || description.length > 500) {
    errors.value.description =
      'Описание: от 10 до 500 символов'
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

    form.value = {
      title: '',
      carriageId: 0,
      description: ''
    }

    router.push('/')
  } catch (err) {
    if (err.status === 400) {
      submitError.value = err.message || 'Проверьте поля'
    } else if (err.status === 500) {
      submitError.value = 'Сервер не смог обработать запрос'
    } else {
      submitError.value =
        'Нет связи. Проверьте, что приложение запущено'
    }
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main>
    <h2>Новая поездка</h2>

    <p v-if="isLoading">
      Загрузка справочников…
    </p>

    <div v-else-if="dictionaryError">
      <p class="error">{{ dictionaryError }}</p>
      <button type="button" @click="loadData">
        Повторить
      </button>
    </div>

    <form v-else @submit.prevent="onCreate">
      <label for="title">
        Маршрут (5–80 символов)
      </label>

      <input
        id="title"
        v-model="form.title"
        autofocus
      />

      <p v-if="errors.title" class="error">
        {{ errors.title }}
      </p>

      <label for="carriageId">Тип вагона</label>

      <select
        id="carriageId"
        v-model.number="form.carriageId"
      >
        <option :value="0">— выберите —</option>

        <option
          v-for="c in carriages"
          :key="c.id"
          :value="c.id"
        >
          {{ c.name }}
        </option>
      </select>

      <p v-if="errors.carriageId" class="error">
        {{ errors.carriageId }}
      </p>

      <label for="description">
        Описание (10–500 символов)
      </label>

      <textarea
        id="description"
        v-model="form.description"
      ></textarea>

      <p v-if="errors.description" class="error">
        {{ errors.description }}
      </p>

      <p v-if="submitError" class="error">
        {{ submitError }}
      </p>

      <button
        type="submit"
        :disabled="isSubmitting"
      >
        {{ isSubmitting ? 'Создаём…' : 'Создать' }}
      </button>
    </form>
  </main>
</template>

<style scoped>
label {
  display: block;
  margin-top: 12px;
  margin-bottom: 4px;
  font-weight: 500;
}

input,
select,
textarea {
  display: block;
  width: 100%;
  max-width: 400px;
  padding: 6px;
  box-sizing: border-box;
}

textarea {
  min-height: 80px;
  resize: vertical;
}

.error {
  color: #c00;
  font-size: 13px;
  margin: 4px 0 0;
}

form button[type='submit'] {
  margin-top: 12px;
}

form button[disabled] {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
