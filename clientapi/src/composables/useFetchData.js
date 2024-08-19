import { ref } from 'vue';
import axiosInstance from '../axiosConfig'; // Zaktualizuj œcie¿kê do pliku z konfiguracj¹ axios

export function useFetchData() {
    const data = ref(null);
    const loading = ref(false);
    const error = ref('');

    // Funkcja do pobierania danych
    const fetchData = async (url) => {
        loading.value = true;
        error.value = '';

        try {
            const response = await axiosInstance.get(url);
            data.value = response.data;
        } catch (err) {
            if (err.response) {
                if (err.response.status === 404) {
                    error.value = 'Data not found';
                } else if (err.response.status === 401) {
                    error.value = 'Unauthorized';
                    // Tutaj mo¿na dodaæ logikê do wylogowania u¿ytkownika
                } else {
                    console.error('Error fetching data:', err);
                    error.value = 'An error occurred while fetching the data.';
                }
            } else {
                console.error('Error fetching data:', err);
                error.value = 'An error occurred while fetching the data.';
            }
        } finally {
            loading.value = false;
        }
    };

    // Funkcja do aktualizacji danych
    const updateData = async (url, payload) => {
        loading.value = true;
        error.value = '';

        try {
            const response = await axiosInstance.put(url, payload);
            data.value = response.data;
        } catch (err) {
            if (err.response) {
                if (err.response.status === 404) {
                    error.value = 'Data not found';
                } else if (err.response.status === 401) {
                    error.value = 'Unauthorized';
                } else {
                    console.error('Error updating data:', err);
                    error.value = 'An error occurred while updating the data.';
                }
            } else {
                console.error('Error updating data:', err);
                error.value = 'An error occurred while updating the data.';
            }
        } finally {
            loading.value = false;
        }
    };

    return { data, loading, error, fetchData, updateData };
}
