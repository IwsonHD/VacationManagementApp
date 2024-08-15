<template>
    <div>
        <h2>Find Employee Details</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter employee email"
               @keyup.enter="fetchEmployeeData" />
        <button @click="fetchEmployeeData">Search</button>

        <div v-if="loading">Loading...</div>
        <div v-if="error">{{ error }}</div>
        <div v-if="data">
            <h2>Employee Details</h2>
            <p><strong>Name:</strong> {{ data.name }}</p>
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

    const fetchEmployeeData = () => {
        if (email.value) {
            fetchData(`${API_BASE_URL}/Users/employee/${email.value}`);
        }
    };
</script>
