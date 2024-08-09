<template>
    <div v-if="vacations">
        <h2>Vacations</h2>
        <div v-for="vacation in vacations" :key="vacation.id">
            <p><strong>HowManyDays:</strong> {{ vacation.howManyDays }}</p>
            <p><strong>State:</strong> {{ vacation.state }}</p>
            <p><strong>When:</strong> {{ vacation.when }}</p>
            
        </div>
    </div>
    <div v-else>
        <p>Loading...</p>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import { API_BASE_URL } from '../config';

    const vacations = ref(null);

    onMounted(async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Vacations/jvc35414@nowni.com`);
            vacations.value = response.data;
            console.log(vacations.value);
        } catch (error) {
            console.error('Error fetching vacation data:', error);
        }
    });
</script>
