<script setup>
// Пока ничего — только разметка
</script>

<template>
  <main>
    <h2>Вход в «РЖД Соседи»</h2>
    <p class="hint">
      Авторизация через электронный документооборот РЖД.
      Анкета пассажира заполняется один раз.
    </p>
    <form @submit.prevent>
      <label>
        Логин
        <input name="login" autofocus />
      </label>

      <label>
        Пароль
        <input type="password" name="password" />
      </label>

      <button type="submit">Войти</button>
    </form>
  </main>
</template>

<style scoped>
.hint {
  color: #555;
  font-size: 14px;
  margin: 4px 0 16px;
}

label {
  display: block;
  margin-bottom: 12px;
}

input {
  display: block;
  width: 100%;
  max-width: 400px;
  padding: 6px;
  margin-top: 4px;
  box-sizing: border-box;
}

form button[type='submit'] {
  margin-top: 8px;
}
</style>