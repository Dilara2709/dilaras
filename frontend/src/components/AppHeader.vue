<script setup>
// Ничего не нужно — только разметка со ссылками
</script>

<template>
  <header class="app-header">
    <nav>
      <router-link to="/">Поездки</router-link>
      <router-link to="/new">Создать поездку</router-link>
      <router-link to="/login">Вход</router-link>
    </nav>
  </header>
</template>

<style scoped>
.app-header {
  padding: 8px 0;
  margin-bottom: 16px;
  border-bottom: 1px solid #ddd;
}

nav {
  display: flex;
  gap: 16px;
}

nav a {
  text-decoration: none;
  color: #1f4b7a;
  font-weight: 500;
}

nav a.router-link-active {
  text-decoration: underline;
  color: #000;
}
</style>