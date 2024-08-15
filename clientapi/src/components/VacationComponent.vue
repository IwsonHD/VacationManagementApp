<template>
    <div>
        <h2>Find Vacations</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter user email"
               @keyup.enter="fetchVacationData" />
        <button @click="fetchVacationData">Search</button>

        <div v-if="loading">Loading...</div>
        <div v-if="error">{{ error }}</div>
        <div v-if="data">
            <h2>Vacations</h2>
            <div v-for="vacation in data" :key="vacation.id">
                <p><strong>How Many Days:</strong> {{ vacation.howManyDays }}</p>
                <p><strong>State:</strong> {{ vacation.state }}</p>
                <p><strong>When:</strong> {{ vacation.when }}</p>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { useFetchData } from '../composables/useFetchData';
    import { API_BASE_URL } from '../config';

    const email = ref('');
    const { data, loading, error, fetchData } = useFetchData();

    const fetchVacationData = () => {
        if (email.value) {
            fetchData(`${API_BASE_URL}/Vacations/${email.value}`);
        }
    };
</script>
