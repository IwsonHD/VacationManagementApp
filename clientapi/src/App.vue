<script setup>
    import { ref } from 'vue'
    import EmployeeComponent from './components/EmployeeComponent.vue'
    import EmployerComponent from './components/EmployerComponent.vue'
    import VacationComponent from './components/VacationComponent.vue'

    const currentView = ref('')  // Przechowuje aktualnie wybrany widok

    const setView = (view) => {
        currentView.value = view
    }
</script>

<template>
    <header class="header">
        <h1>Vacation Management API </h1>
    </header>

    <main class="main-container">
        <nav class="sidebar">
            <button @click="setView('employer')">Show Employer</button>
            <button @click="setView('employee')">Show Employee</button>
            <button @click="setView('vacation')">Show Vacations</button>
        </nav>

        <section class="content">
            <EmployerComponent v-if="currentView === 'employer'" />
            <EmployeeComponent v-if="currentView === 'employee'" />
            <VacationComponent v-if="currentView === 'vacation'" />
        </section>
    </main>
</template>

<style scoped>
    header {
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
        padding: 1rem;
        background-color: #f0f0f0;
        border-radius: 8px; 
        margin: 1rem; 
    }

    .main-container {
        display: flex;
        min-height: 100vh;
        padding: 1rem;
    }

    .sidebar {
        width: 200px;
        background-color: #f5f5f5;
        padding: 1rem;
        display: flex;
        flex-direction: column;
        border-radius: 8px; 
        margin-right: 1rem;
        margin-top: 1px;
    }

    .sidebar button {
        margin-bottom: 1rem;
        padding: 0.5rem;
        background-color: #007bff;
        color: white;
        border: none;
        cursor: pointer;
        border-radius: 4px;
        transition: background-color 0.3s;
    }

    .sidebar button:hover {
        background-color: #0056b3;
    }

    .content {
        flex-grow: 1;
        padding: 1rem;
        border-radius: 8px;
        background-color: #ffffff; 
    }
</style>
