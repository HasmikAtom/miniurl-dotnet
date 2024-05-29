import axios from 'axios';

const baseUrl = 'http://localhost:5214/'

const axiosClient = axios.create({
    baseURL: baseUrl,
});

export default axiosClient;