import { ref } from 'vue';
import axiosInstance from '../axiosConfig'; // Zaktualizuj œcie¿kê do pliku z konfiguracj¹ axios

export function useApiRequest() {
    const data = ref(null);
    const loading = ref(false);
    const error = ref('');

    // Funkcja do wykonania zapytania HTTP
    const sendRequest = async (method, url, payload = null) => {
        loading.value = true;
        error.value = '';

        try {
            const response = await axiosInstance({
                method,
                url,
                data: payload,
            });
            data.value = response.data;
        } catch (err) {
            if (err.response) {
                if (err.response.status === 404) {
                    error.value = 'Data not found';
                } else if (err.response.status === 401) {
                    error.value = 'Unauthorized';
                } else if (err.response.status === 500) {
                    console.error('Server error:', err);
                    error.value = 'An error occurred on the server.';
                } else if (err.response.status === 403) {
                    error.value = 'Forbidden for your role';
                } else {
                    console.error('Error:', err);
                    error.value = 'An unknown error occurred.';
                }
            } else {
                console.error('Request error:', err);
                error.value = 'An error occurred during the request.';
            }
        } finally {
            loading.value = false;
        }
    };

    // Metody dla ró¿nych typów zapytañ
    const fetchData = (url) => sendRequest('get', url);
    const postData = (url, payload) => sendRequest('post', url, payload);
    const updateData = (url, payload) => sendRequest('put', url, payload);
    const deleteData = (url) => sendRequest('delete', url);

    return { data, loading, error, fetchData, postData, updateData, deleteData };
}
