import { createRouter, createWebHistory } from 'vue-router'
import TripList from './pages/TripList.vue'
import TripNew from './pages/TripNew.vue'
import Login from './pages/Login.vue'

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: TripList },
    { path: '/new', component: TripNew },
    { path: '/login', component: Login }
  ]
})