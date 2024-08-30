<script setup>
    import { ref } from 'vue';
    import { useApiRequest } from '../composables/useApiRequest';

    const vacationDto = ref({
        HowManyDays: '',
        When: ''
    });

    const { data, loading, error, postData } = useApiRequest();

    const addVacation = async () => {
        await postData('/vacations/add', vacationDto.value);
        if (!error.value) {
            console.log('Vacation added successfully:', data.value);
            resetForm(); // Resetowanie formularza po pomyœlnym dodaniu
        }
    };

    const resetForm = () => {
        vacationDto.value.startDate = '';
        vacationDto.value.duration = '';
    };
</script>

<template>
    <div class="add-vacation-container">
        <h2>Add Vacation</h2>
        <form @submit.prevent="addVacation">
            <div>
                <label for="startDate">Start Date:</label>
                <input type="date" id="When" v-model="vacationDto.When" required />
            </div>
            <div>
                <label for="duration">Duration (days):</label>
                <input type="number" id="HowManyDays" v-model="vacationDto.HowManyDays" required />
            </div>
            <button type="submit" :disabled="loading">Add Vacation</button>
        </form>
        <p v-if="loading">Adding vacation...</p>
        <p v-if="error" class="error">{{ error }}</p>
        <p v-if="!error && data">Vacation added successfully!</p>
    </div>
</template>