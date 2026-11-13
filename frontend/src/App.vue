<script setup>
import AppHeader from './components/AppHeader.vue'
</script>

<template>
  <div class="app">
    <AppHeader />
    <router-view />
  </div>
</template>

<style scoped>
.app {
  max-width: 900px;
  margin: 0 auto;
  padding: 16px;
  font-family: system-ui, sans-serif;
}
</style>