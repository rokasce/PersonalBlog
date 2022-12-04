import Head from 'next/head';
import Image from 'next/image';
import styles from '../styles/Home.module.css';
import { LoginForm } from '../components/login-form';
import { NavBar } from '../components/nav-bar';

export default function Login() {
  return (
    <div className={styles.container}>
      <Head>
        <title>Login</title>
        <meta name='description' content='Generated by create next app' />
        <link rel='icon' href='/favicon.ico' />
      </Head>

      <NavBar />

      <main className={styles.main}>
        <h1 className={styles.title}>Login</h1>

        <LoginForm />
      </main>
    </div>
  );
}