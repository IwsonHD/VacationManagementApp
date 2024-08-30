<template>
    <div class="employer-search">
        <h2>Find Employer Details</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter employer email"
               @keyup.enter="fetchEmployerData" />
        <button @click="fetchEmployerData" :disabled="loading">Search</button>

        <div v-if="loading" class="loading">Loading...</div>
        <div v-if="error" class="error">{{ error }}</div>
        <div v-if="data && !error" class="employer-details">
            <h3>Employer Details</h3>
            <p><strong>Name:</strong> {{ data.firstName }} {{ data.lastName }}</p>
            <p><strong>Email:</strong> {{ data.email }}</p>
            <p><strong>Phone:</strong> {{ data.phoneNumber }}</p>
            <p><strong>Company:</strong> {{ data.companyName }}</p>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { useApiRequest } from '../composables/useApiRequest';
    import { API_BASE_URL } from '../config';

    const email = ref('');
    const { data, loading, error, fetchData } = useApiRequest();

    const fetchEmployerData = () => {
        if (email.value.trim() === '') {
            error.value = 'Please enter a valid email address.';
            return;
        }
        fetchData(`${API_BASE_URL}/Users/employer/${email.value}`);
    };
</script>