<template>
    <div class="employee-search">
        <h2>Find Employee Details</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter employee email"
               @keyup.enter="fetchEmployeeData" />
        <button @click="fetchEmployeeData" :disabled="loading">Search</button>

        <div v-if="loading">Loading...</div>
        <div v-if="error" class="error">{{ error }}</div>
        <div v-if="data && !error" class="employee-details">
            <h3>Employee Details</h3>
            <p><strong>Name:</strong> {{ data.firstName }} {{ data.lastName }}</p>
            <p><strong>Email:</strong> {{ data.email }}</p>
            <p><strong>Phone:</strong> {{ data.phoneNumber }}</p>
            <p><strong>Employer:</strong> {{ data.employersEmail }}</p>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { useApiRequest } from '../composables/useApiRequest';
    import { API_BASE_URL } from '../config';

    const email = ref('');
    const { data, loading, error, fetchData } = useApiRequest();

    const fetchEmployeeData = () => {
        if (email.value.trim() === '') {
            error.value = 'Please enter a valid email address.';
            return;
        }
        fetchData(`${API_BASE_URL}/Users/employee/${email.value}`);
    };
</script>
