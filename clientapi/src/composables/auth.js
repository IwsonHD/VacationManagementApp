import axios from 'axios';
import { API_BASE_URL } from '../config';

class AuthService {
    login(user) {
        return axios
            .post(`${ API_BASE_URL }/Account/login`, {
                email: user.email,
                password: user.password
            })
            .then(response => {
                if (response.data.token) {
                    localStorage.setItem('user', JSON.stringify(response.data));
                }
                return response.data;
            })
    }

    logout() {
        localStorage.removeItem('user');
    }

    getCurrentUser() {
        return JSON.parse(localStorage.getItem('user'));
    }
 }

export default new AuthService();

