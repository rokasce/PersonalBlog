import { Field, Form, Formik } from 'formik';

export const LoginForm = () => {
  return (
    <Formik
      initialValues={{ email: '', password: '' }}
      onSubmit={(values) =>
        console.log(`Values ${values.email} ${values.password}`)
      }
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
