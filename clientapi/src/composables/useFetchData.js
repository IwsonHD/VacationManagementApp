// src/composables/useFetchData.js
import { ref } from 'vue';
import axios from 'axios';

export function useFetchData() {
    const data = ref(null);
    const loading = ref(false);
    const error = ref('');

    const fetchData = async (url) => {
        loading.value = true;
        error.value = '';
        try {
            const response = await axios.get(url);
            data.value = response.data;
        } catch (err) {
            if (err.response && err.response.data === 404) {
                error.value = 'Data not found';
            } else {
                console.error('Error fetching data:', err);
                error.value = 'An error occurred while fetching the data.';
            }
        } finally {
            loading.value = false;
        }
    };

    return { data, loading, error, fetchData };
}
