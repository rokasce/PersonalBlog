import React from "react";
import { BlogTitle } from "./BlogTitle/BlogTitle";
import { DarkModeToggle } from "./DarkModeToggle/DarkModeToggle";
import styles from "./Header.module.css";

export const Header = () => {
  return (
    <div className={styles.header}>
      <BlogTitle />
      <DarkModeToggle />
    </div>
  );
};
