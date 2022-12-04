import '../styles/globals.css';
import ApplicationContext, { Context } from '../context/AppContext';
import { useEffect, useState } from 'react';
import { AppProps } from 'next/app';
import Router from 'next/router';
import agent from '../api/agent';

export default function App({ Component, pageProps }: AppProps) {
  const [user, setUser] = useState({ token: '' });
  const [loading, setLoading] = useState(true);

  const value: Context = {
    token: user.token,
    setToken: (token: { token: string }) => {
      setUser(token);
    },
  };

  useEffect(() => {
    agent.Account.refresh()
      .then((response) => {
        console.log('refreshing damn token', response);
        setUser({ token: response.token });
        Router.push('/');
      })
      .catch((error) => {
        Router.push('/login');
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  if (loading) {
    return <div>loading...</div>;
  }

  return (
    <ApplicationContext.Provider value={value}>
      <Component {...pageProps} />
    </ApplicationContext.Provider>
  );
}
