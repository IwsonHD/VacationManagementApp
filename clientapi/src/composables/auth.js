import axios from '../axiosConfig';

class AuthService {
    login(user) {
        return axios
            .post('/Account/login', {
                email: user.email,
                password: user.password
            })
            .then(response => {
                if (response.data.token) {
                    localStorage.setItem('user', JSON.stringify(response.data));
                    localStorage.setItem('email', user.email);  // Storing plain email as string
                }
                return response.data;
            });
    }

    logout() {
        localStorage.removeItem('user');
        localStorage.removeItem('email');
    }

    getCurrentUser() {
        return JSON.parse(localStorage.getItem('user')) || null;  // Return null if not found
    }

    getCurrentUserEmail() {
        return localStorage.getItem('email') || '';  // Return empty string if not found
    }
}

export default new AuthService();
