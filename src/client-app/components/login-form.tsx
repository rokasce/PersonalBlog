import { Field, Form, Formik } from 'formik';
import Router from 'next/router';
import { useContext } from 'react';
import agent from '../api/agent';
import ApplicationContext from '../context/AppContext';
import { UserFormValues } from '../models/user';

export const LoginForm = () => {
  const user = useContext(ApplicationContext);

  const handleFormSubmit = async (values: UserFormValues) => {
    const login = async () => {
      return await agent.Account.login({
        email: values.email,
        password: values.password,
      });
    };

    const result = await login();
    if (user && result?.token) {
      user.setToken(result);
      Router.push('/');
    }
  };

  return (
    <Formik
      initialValues={{ email: 'bob@test.com', password: 'Test1234!' }}
      onSubmit={(values) => {
        handleFormSubmit(values);
      }}
    >
      {({ handleSubmit }) => (
        <Form
          onSubmit={handleSubmit}
          autoComplete='off'
          style={{ display: 'flex', flexDirection: 'column' }}
        >
          <Field type='text' name='email' placeholder='Email' />
          <Field type='password' name='password' placeholder='Password' />
          <input type='submit' />
        </Form>
      )}
    </Formik>
  );
};
