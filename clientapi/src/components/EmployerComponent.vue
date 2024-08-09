<!-- EmployerComponent.vue -->
<template>
    <div v-if="employer">
        <h2>Employer Details</h2>
        <p><strong>Name:</strong> {{ employer.firstName }}</p>
        <p><strong>Email:</strong> {{ employer.email }}</p>
    </div>
    <div v-else>
        <p>Loading...</p>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import { API_BASE_URL } from '../config';

    const employer = ref(null);

    onMounted(async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Users/employer/iwo@czart.net`);
            employer.value = response.data || {}; // Upewnij siê, ¿e employer nie jest null
            console.log(employer.value);
        } catch (error) {
            console.error('Error fetching employer data:', error);
        }
    });
</script>

<style scoped>
    /* Dodaj style wed³ug potrzeby */
</style>
