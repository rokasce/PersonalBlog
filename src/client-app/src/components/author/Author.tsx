import React from "react";
import logo from "../../logo.svg";
import styles from "./Author.module.css";

export const Author = () => {
  return (
    <div className={styles.author}>
      <img className={styles.author__image} src={logo} alt="logo" />
      <div className={styles.author__info}>
        <span className="author__name">Rokas Cesnulevicius</span>
        <span className="author__description">Cheesy description</span>
      </div>
    </div>
  );
};
