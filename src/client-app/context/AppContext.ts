import { createContext } from 'react';

export interface Context {
  token: string;
  setToken: (token: { token: string }) => void;
}

const ApplicationContext = createContext<Context>({
  token: '',
  setToken: ({ token: string }) => {},
});

export default ApplicationContext;
