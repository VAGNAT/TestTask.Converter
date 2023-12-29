import axios from 'axios';

export const createAxiosInstance = () => {

	const axiosInstance = axios.create({
		baseURL: process.env.REACT_APP_BACKEND_API_URL,
		withCredentials: true,
		headers: {
			"Access-Control-Allow-Origin": "*",
			"Content-Type": "application/json",
			mode: "cors"
		}
	});

	return axiosInstance;
};