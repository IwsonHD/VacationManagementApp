<template>
    <div class="login-container">
        <h2>Login</h2>
        <input type="email"
               v-model="email"
               placeholder="Enter your email"
               class="login-input" />
        <input type="password"
               v-model="password"
               placeholder="Enter your password"
               class="login-input" />
        <button @click="login" class="login-button">Login</button>

        <div v-if="loading" class="loading-message">Loading...</div>
        <div v-if="error" class="error-message">{{ error }}</div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { API_BASE_URL } from '../config';
    import AuthService from '../composables/auth';

    const email = ref('');
    const password = ref('');
    const loading = ref(false);
    const error = ref('');

    const login = async () => {

        if (!email.value || !password.value) {
            error.value = 'Please fill in both fields';
            return;
        }

        loading.value = true;
        error.value = '';

        try {
            const user = { email: email.value, password: password.value };
            const response = await AuthService.login(user);

            console.log('Login successful', response);
            loading.value = false;
        } catch (err) {
            error.value = 'Failed to login. Please check your credentials.';
            loading.value = false;
        }
    };

</script>

<style scoped>
    .login-container {
        max-width: 300px;
        margin: 0 auto;
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 1rem;
        border: 1px solid #ddd;
        border-radius: 8px;
        background-color: #f9f9f9;
    }

    .login-input {
        width: 100%;
        padding: 0.5rem;
        margin-bottom: 1rem;
        border: 1px solid #ccc;
        border-radius: 4px;
        box-sizing: border-box;
    }

    .login-button {
        width: 100%;
        padding: 0.5rem;
        margin-bottom: 1rem;
        background-color: #007bff;
        color: white;
        border: none;
        cursor: pointer;
        border-radius: 4px;
        transition: background-color 0.3s;
    }

        .login-button:hover {
            background-color: #0056b3;
        }

    .loading-message, .error-message {
        margin-top: 1rem;
        color: #ff0000;
        text-align: center;
    }
</style>
