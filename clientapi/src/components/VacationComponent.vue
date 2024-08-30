<template>
    <div class="vacation-search">
        <h2>Find Vacations</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter user email"
               @keyup.enter="fetchVacationData" />
        <button @click="fetchVacationData" :disabled="loading">Search</button>

        <div v-if="loading" class="loading">Loading...</div>
        <div v-if="error" class="error">{{ error }}</div>
        <div v-if="data && data.length > 0" class="vacation-details">
            <h3>Vacations</h3>
            <div v-for="(vacation, index) in data" :key="vacation.id" class="vacation-item">
                <p><strong>#{{ index + 1 }}</strong></p>
                <p><strong>How Many Days:</strong> {{ vacation.howManyDays }}</p>
                <p><strong>State:</strong> {{ vacation.state }}</p>
                <p><strong>When:</strong> {{ formatDate(vacation.when) }}</p>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { useApiRequest } from '../composables/useApiRequest';
    import { API_BASE_URL } from '../config';

    const email = ref('');
    const { data, loading, error, fetchData } = useApiRequest();

    // Function to format the date
    const formatDate = (dateString) => {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(dateString).toLocaleDateString(undefined, options);
    };

    const fetchVacationData = () => {
        if (email.value.trim() === '') {
            error.value = 'Please enter a valid email address.';
            return;
        }
        fetchData(`${API_BASE_URL}/Vacations/${email.value}`);
    };
</script>