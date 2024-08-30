<template>
    <div class="your-employees">
        <h2>Your employees</h2>
        <div v-if="loading" class="loading">Loading...</div>
        <div v-if="error" class="error">{{ error }}</div>
        <div v-if="data && data.length > 0" class="employee-details">
            <h3>Employees</h3>
            <div v-for="(employee, index) in data" :key="employee.email" class="employee-item">
                <h3>Employee #{{ index + 1 }} Details</h3>
                <p><strong>Name:</strong> {{ employee.firstName }} {{ employee.lastName }}</p>
                <p><strong>Email:</strong> {{ employee.email }}</p>
                <p><strong>Phone:</strong> {{ employee.phoneNumber }}</p>
                <p><strong>Employer:</strong> {{ employee.employersEmail }}</p>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { useApiRequest } from '../composables/useApiRequest';
    import { API_BASE_URL } from '../config';
    import AuthService from '../composables/auth.js';

    const email = AuthService.getCurrentUserEmail().trim();

    const { data, loading, error, fetchData } = useApiRequest();

    onMounted(() => {
       
       fetchData(`${API_BASE_URL}/Users/yourEmployees/${email}`);
        
    });
</script>