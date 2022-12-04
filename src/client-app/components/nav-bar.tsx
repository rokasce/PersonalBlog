import Link from 'next/link';
import React from 'react';

export const NavBar = () => {
  return (
    <nav>
      <ul>
        <li>
          <Link href={'/'}>Home</Link>
        </li>
        <li>
          <Link href={'/about'}>About</Link>
        </li>
        <li>
          <Link href={'/login'}>Login</Link>
        </li>
      </ul>
    </nav>
  );
};
