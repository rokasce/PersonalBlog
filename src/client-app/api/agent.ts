import axios, { AxiosError, AxiosResponse } from 'axios';
import { User, UserFormValues } from '../models/user';

const sleep = async (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.defaults.baseURL = 'https://localhost:7264/api';
axios.defaults.withCredentials = true;
axios.interceptors.response.use(
  async (response) => {
    return response;
  },
  (error: AxiosError) => {
    if (!error.response) return Promise.reject(error);

    const { status } = error.response;

    switch (status) {
      case 400:
        console.log('Bad Request');
        break;
      case 401:
        console.log('Unauthorized Request');
        break;
      case 404:
        console.log('Not found');
        break;
      case 500:
        console.log('Server error');
        break;
    }

    return Promise.reject(error);
  }
);

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Account = {
  current: () => requests.get<User>('/account'),
  login: (user: UserFormValues): Promise<{ token: string }> =>
    requests.post<User>('/account/login', user),
  register: (user: UserFormValues) =>
    requests.post<User>('/account/register', user),
  refresh: () => requests.post<User>('/account/refresh', {}),
};

const agent = {
  Account,
};

export default agent;
