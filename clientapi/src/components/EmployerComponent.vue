<template>
    <div>
        <h2>Find Employer Details</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter employer email"
               @keyup.enter="fetchEmployerData" />
        <button @click="fetchEmployerData">Search</button>

        <div v-if="loading">Loading...</div>
        <div v-if="error">{{ error }}</div>
        <div v-if="data">
            <h2>Employer Details</h2>
            <p><strong>Name:</strong> {{ data.firstName }}</p>
            <p><strong>Email:</strong> {{ data.email }}</p>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { useFetchData } from '../composables/useFetchData';
    import { API_BASE_URL } from '../config';

    const email = ref('');
    const { data, loading, error, fetchData } = useFetchData();

    const fetchEmployerData = () => {
        if (email.value) {
            fetchData(`${API_BASE_URL}/Users/employer/${email.value}`);
        }
    };
</script>
