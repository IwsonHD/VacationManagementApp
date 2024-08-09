<template>
    <div>
        <h2>Find Employee Details</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter employee email"
               @keyup.enter="fetchEmployee" />
        <button @click="fetchEmployee">Search</button>

        <div v-if="employee">
            <h2>Employee Details</h2>
            <p><strong>Name:</strong> {{ employee.name }}</p>
            <p><strong>Email:</strong> {{ employee.email }}</p>
        </div>
        <div v-else-if="loading">
            <p>Loading...</p>
        </div>
        <div v-else-if="error">
            <p>{{ error }}</p>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import axios from 'axios';
    import { API_BASE_URL } from '../config';

    const email = ref('');
    const employee = ref(null);
    const loading = ref(false);
    const error = ref('');

    const fetchEmployee = async () => {
        if (!email.value) {
            error.value = 'Please enter a valid email address.';
            return;
        }

        loading.value = true;
        error.value = '';
        try {
            const response = await axios.get(`${API_BASE_URL}/Users/employee/${email.value}`);
            employee.value = response.data;
        } catch (err) {
            console.error('Error fetching employee data:', err);
            error.value = 'Employee not found or an error occurred.';
        } finally {
            loading.value = false;
        }
    };
</script>

<style scoped>
    input {
        margin-bottom: 10px;
        padding: 8px;
        border-radius: 4px;
        border: 1px solid #ccc;
    }

    button {
        margin-bottom: 20px;
        padding: 8px 12px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        transition: background-color 0.3s;
    }

        button:hover {
            background-color: #0056b3;
        }
</style>
